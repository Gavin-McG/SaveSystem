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

        internal string NonTempFileName => "/" + fileName + ".json";
        internal string NonTempFilePath => SaveSystemIO.BasePath + NonTempFileName;

        internal string TempFileName => "/" + fileName + "-temp.json";
        internal string TempFilePath => SaveSystemIO.BasePath + TempFileName;

        /// <summary>
        /// Clears the Contents of a Save file, including temp save if applicable
        /// </summary>
        public void ClearSave(bool showLogs = true)
        {
            ClearNonTempSave(showLogs);
            if (Application.isPlaying)
                ClearTempSave(showLogs);
        }

        /// <summary>
        /// Clears the Contents of a Save file's temp data
        /// </summary>
        public void ClearTempSave(bool showLogs = true)
        {
            if (SaveSystemIO.Exists(TempFilePath))
            {
                SaveSystemIO.Delete(TempFilePath);
                if (showLogs) Debug.Log($"Clearing {TempFilePath}");
            }
            else if (showLogs) Debug.Log($"No such file {TempFilePath} to clear");
        }

        /// <summary>
        /// Clears the Contents of a Save file's persistent data
        /// </summary>
        public void ClearNonTempSave(bool showLogs = true)
        {
            if (SaveSystemIO.Exists(NonTempFilePath))
            {
                SaveSystemIO.Delete(NonTempFilePath);
                if (showLogs) Debug.Log($"Clearing {NonTempFilePath}");
            }
            else if (showLogs) Debug.Log($"No such file {NonTempFilePath} to clear");
        }
    }
}
