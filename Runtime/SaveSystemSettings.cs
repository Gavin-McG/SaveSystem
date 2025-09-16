using System;
using UnityEngine;
using System.IO;

namespace WolverineSoft.SaveSystem
{
    /// <summary>
    /// ScriptableObject for storing all settings related to save system
    /// </summary>
    [CreateAssetMenu(fileName = "SaveSystemSettings", menuName = "Save System/Settings")]
    public class SaveSystemSettings : ScriptableObject
    {
        public static class Styles
        {
            public const string saveFileTooltip =
                "Save File to actively use";

            public const string useTemporarySaveTooltip =
                "Create a temporary copy of the current save file that will persist until end of session. Useful for testing.";

            public const string showDebugTooltip =
                "Show Warnings and Logs from Save System, Errors will be shown regardless";
        }

        [Header("Save File")] [Tooltip(Styles.saveFileTooltip)] [SerializeField]
        public SaveFile saveFile;

        [Tooltip(Styles.useTemporarySaveTooltip)] [SerializeField]
        public bool useTemporarySave = false;

        [Header("Debug Settings")] [Tooltip(Styles.showDebugTooltip)] [SerializeField]
        public bool showDebug = true;

        //Property for whether temporary file should be used
        private bool UseTemp => useTemporarySave && Application.isEditor;

        //Full current file path
        public string FilePath => UseTemp ? saveFile?.TempFilePath : saveFile?.NonTempFilePath;

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
                // If NonTemp exists, copy it to Temp
                if (File.Exists(saveFile.NonTempFilePath))
                {
                    File.Copy(saveFile.NonTempFilePath, saveFile.TempFilePath, overwrite: true);
                    if (showDebug)
                        Debug.Log($"[SaveSystemSettings] Copied {saveFile.NonTempFileName} → {saveFile.TempFileName}");
                }
            }
            else
            {
                // Delete temp file if it exists
                if (File.Exists(saveFile.TempFilePath))
                {
                    File.Delete(saveFile.TempFilePath);
                    if (showDebug) Debug.Log($"[SaveSystemSettings] Deleted {saveFile.TempFileName}");
                }
            }
        }
    }
}