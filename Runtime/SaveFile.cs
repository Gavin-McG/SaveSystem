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
        public string NonTempFileName => "/" + fileName + ".json";
        public string NonTempFilePath => Application.persistentDataPath + NonTempFileName;
        public string TempFileName => "/" + fileName + "-temp.json";
        public string TempFilePath => Application.persistentDataPath + TempFileName;

        public void ClearSave()
        {
            // Clear non-temp file
            if (File.Exists(NonTempFilePath))
            {
                Debug.Log($"Clearing {NonTempFilePath}");
                File.Delete(NonTempFilePath);
            }

            // Clear temp file
            if (File.Exists(TempFilePath))
            {
                Debug.Log($"Clearing {TempFilePath}");
                File.Delete(TempFilePath);
            }
        }
    }
}
