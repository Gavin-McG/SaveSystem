using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WolverineSoft.SaveSystem
{
    /// <summary>
    /// Component for managing the save data and communicating with ISaveData Objects.
    /// Requires a reference to Save Settings
    /// </summary>
    public sealed class SaveManager : Singleton<SaveManager>
    {
        public static class Styles
        {
            public const string managerIDTooltip =
                "ID used for matching with ISaveData objects. Keep empty for default";

            public const string settingsTooltip =
                "Save Settings to be used by SaveManager";
        }

        [Tooltip(Styles.managerIDTooltip)] [SerializeField]
        public string managerID;

        [Tooltip(Styles.settingsTooltip)] [SerializeField]
        public SaveSystemSettings settings;

        private bool _loaded = false; //whether a save has been loaded
        private Dictionary<string, object> _data = new(); //dictionary corresponding object identifiers to their data

        // Contract resolver for NewtonSoft serialization. Used to target only fields and exclude properties
        private class FieldsOnlyContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                // Only serialize fields
                var fields = type
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                var jsonProperties = new List<JsonProperty>();
                foreach (var field in fields)
                {
                    var property = base.CreateProperty(field, memberSerialization);
                    property.Writable = true;
                    property.Readable = true;
                    jsonProperties.Add(property);
                }

                return jsonProperties;
            }
        }

        // Settings for NewtonSoft serialization
        private JsonSerializerSettings JsonSettings => new JsonSerializerSettings()
        {
            ContractResolver = new FieldsOnlyContractResolver(),
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented
        };

        // returns all the ISaveData Components within the scene
        private IEnumerable<ISaveData> SaveObjects =>
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ISaveData>()
                .OfType<ISaveData>()
                .Where(s => s.ManagerId == managerID);

        // returns a set of KVP for all ISaveData objects (identifier - data)
        private List<KeyValuePair<string, object>> SaveEntries => SaveObjects
            .GroupBy(x => x.Identifier)
            .Select(x =>
            {
                if (x.Count() > 1)
                    Debug.LogError(
                        $"Multiple ISavaData Objects contained identifier {x.Key}. One or more will be skipped.");
                return new KeyValuePair<string, object>(x.Key, x.First().GetDataObject());
            })
            .ToList();



        /// <summary>
        /// Save data from current ISaveData objects to the active save file. Returns true if no errors encountered
        /// </summary>
        /// <remarks>
        /// Only adds or replaces data to the save file based on ISaveData identifier values.
        /// Does not remove any data of items not present in the current scene
        /// </remarks>
        public bool SaveData()
        {
            //check for active save file
            if (settings?.saveFile == null)
            {
                Debug.LogError("Cannot save: No active Save File assigned.");
                return false;
            }

            //fetch all save data and update data dict
            var saveData = SaveEntries;
            foreach (var entry in saveData)
            {
                _data[entry.Key] = entry.Value;
            }

            //convert to json
            var json = JsonConvert.SerializeObject(_data, JsonSettings);

            //save to file
            if (settings.showLogs) 
                Debug.Log("Saving to " + settings.FilePath);
            File.WriteAllText(settings.FilePath, json);

            return true;
        }


        /// <summary>
        /// Load data from the current active save file. Returns true if no error was encountered while loading
        /// </summary>
        public bool LoadData(bool restore = true)
        {
            //check for active save file
            if (settings?.saveFile == null)
            {
                Debug.LogError("Cannot load save: No active Save File assigned.");
                return false;
            }

            //load data to dict if file exists
            try
            {
                //try to load from file
                string path = settings.FilePath;
                if (File.Exists(path))
                {
                    return LoadFromPath(path, restore);
                }

                //try to load from non-temp file as backup
                string nonTempPath = settings.NonTempFilePath;
                if (settings.UseTemp && File.Exists(nonTempPath))
                {
                    if (settings.showWarnings)
                        Debug.LogWarning("No temp file found; copying main save to temp save");
                    
                    File.Copy(nonTempPath, settings.TempFilePath, true);
                    
                    LoadFromPath(nonTempPath, restore);
                }

                //no save file exists
                if (settings.showWarnings)
                    Debug.LogWarning("No data to load: save file does not exist");
                ClearData(restore);
                return false;
            }
            catch (Exception e)
            {
                //error is opening/deserializing file
                Debug.LogError($"Cannot load save: {e}");
                ClearData(restore);
                return false;
            }
        }

        private bool LoadFromPath(string path, bool restore = true)
        {
            string json = File.ReadAllText(path);
            _data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json, JsonSettings);
            _loaded = true;
            return restore ? RestoreData() : true;
        }

        private void ClearData(bool restore = true)
        {
            _data.Clear();
            _loaded = false;
            if (restore) RestoreToDefault();
        }


        /// <summary>
        /// Uses data from previous load to instruct all ISavaData Objects in the scene to restore their state.
        /// Returns true if no error was encountered while restoring
        /// </summary>
        private bool RestoreData()
        {
            //restore each object
            foreach (var saveObject in SaveObjects)
            {
                if (_data.ContainsKey(saveObject.Identifier))
                {
                    saveObject.RestoreFromObjectData(_data[saveObject.Identifier]);
                }
                else
                {
                    saveObject.RestoreToDefault();
                }
            }

            return true;
        }

        private bool RestoreToDefault()
        {
            foreach (var saveObject in SaveObjects)
            {
                saveObject.RestoreToDefault();
            }
            
            return true;
        }

        /// <summary>
        /// Get the data for a particular identifier. Returns false if no data exists or is of incorrect type
        /// </summary>
        public bool TryGetSaveData<T>(string identifier, out T data)
        {
            //load data if necessary
            if (!_loaded) LoadData(restore: false);

            //check that key exists
            if (!_data.ContainsValue(identifier))
            {
                if (settings.showWarnings)
                    Debug.LogWarning($"No save data found for identifier {identifier}");

                data = default(T);
                return false;
            }

            //check that data is correct type
            var dataObject = _data[identifier];
            if (dataObject is T tData)
            {
                data = tData;
                return true;
            }

            Debug.LogError($"Attempted to get Save data of incorrect type with identifier {identifier}. " +
                           $"Attempted to get {nameof(T)}, was {dataObject.GetType().Name}");
            data = default(T);
            return false;
        }
    }
}
