using System.IO;
using UnityEngine;

namespace WolverineSoft.SaveSystem
{

    /// <summary>
    /// ScriptbleObject representing a unique save file
    /// </summary>
    [CreateAssetMenu(fileName = "SaveFile", menuName = "Save System/SaveFile")]
    public class SaveFile : ScriptableObject
    {
        [SerializeField] public string fileName;

        //Properties for file paths
        internal string NonTempFileName => "/" + fileName + ".json";
        internal string NonTempFilePath => Application.persistentDataPath + NonTempFileName;
        internal string TempFileName => "/" + fileName + "-temp.json";
        internal string TempFilePath => Application.persistentDataPath + TempFileName;

        public void ClearSave(bool showLogs = true)
        {
            ClearNonTempSave(showLogs);
            
            if (Application.isPlaying)
                ClearTempSave(showLogs);
        }

        public void ClearTempSave(bool showLogs = true)
        {
            if (File.Exists(TempFilePath))
            {
                File.Delete(TempFilePath);
                if (showLogs)
                    Debug.Log($"Clearing {TempFilePath}");
            }
            else if (showLogs)
            {
                Debug.Log($"No such file {TempFilePath} to clear");
            }
        }

        public void ClearNonTempSave(bool showLogs = true)
        {
            if (File.Exists(NonTempFilePath))
            {
                File.Delete(NonTempFilePath);
                if (showLogs) 
                    Debug.Log($"Clearing {NonTempFilePath}");
            }
            else if (showLogs)
            {
                Debug.Log($"No such file {NonTempFilePath} to clear");
            }
        }
    }
}
