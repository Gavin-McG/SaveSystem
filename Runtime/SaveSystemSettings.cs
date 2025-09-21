using System;
using UnityEngine;

namespace WolverineSoft.SaveSystem
{
    /// <summary>
    /// ScriptableObject for storing all settings related to save system
    /// </summary>
    [CreateAssetMenu(fileName = "SaveSystemSettings", menuName = "Save System/Settings")]
    public sealed class SaveSystemSettings : ScriptableObject
    {
        public static class Styles
        {
            public const string saveFileTooltip =
                "Save File to actively use";

            public const string useTemporarySaveTooltip =
                "Create a temporary copy of the current save file that will persist until end of session. Useful for testing.";

            public const string showLogsTooltip =
                "Show Logs from Save System, Errors will be shown regardless";
            
            public const string showWarningsTooltip =
                "Show Warnings from Save System, Errors will be shown regardless";
        }

        [Header("Save File")] 
        [Tooltip(Styles.saveFileTooltip)] [SerializeField]
        public SaveFile saveFile;

        [Tooltip(Styles.useTemporarySaveTooltip)] [SerializeField]
        public bool useTemporarySave = false;

        [Header("Debug Settings")] 
        [Tooltip(Styles.showLogsTooltip)] [SerializeField]
        public bool showLogs = false;
        
        [Tooltip(Styles.showLogsTooltip)] [SerializeField]
        public bool showWarnings = true;

        //Property for whether temporary file should be used
        public bool UseTemp => useTemporarySave && Application.isEditor && Application.isPlaying;
        
        //Properties for file paths
        internal string NonTempFileName => saveFile?.NonTempFileName;
        internal string NonTempFilePath => saveFile?.NonTempFilePath;
        internal string TempFileName => saveFile?.TempFileName;
        internal string TempFilePath => saveFile?.TempFilePath;

        //Full current file path
        internal string FilePath => UseTemp ? saveFile?.TempFilePath : saveFile?.NonTempFilePath;

        private void OnValidate()
        {
            TryUpdateTempFile();
        }

        public void TryUpdateTempFile()
        {
            if (Application.isEditor && saveFile != null)
            {
                try
                {
                    UpdateTempFile();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        void UpdateTempFile()
        {
            if (useTemporarySave)
            {
                if (SaveSystemIO.Exists(saveFile.NonTempFilePath))
                {
                    SaveSystemIO.Copy(saveFile.NonTempFilePath, saveFile.TempFilePath, overwrite: true);
                    if (showLogs)
                        Debug.Log($"[SaveSystemSettings] Copied {saveFile.NonTempFileName} → {saveFile.TempFileName}");
                }
            }
            else
            {
                if (SaveSystemIO.Exists(saveFile.TempFilePath))
                {
                    SaveSystemIO.Delete(saveFile.TempFilePath);
                    if (showLogs) Debug.Log($"[SaveSystemSettings] Deleted {saveFile.TempFileName}");
                }
            }
        }
    }
}