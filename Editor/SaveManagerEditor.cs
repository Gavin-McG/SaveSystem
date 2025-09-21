using UnityEngine;

namespace WolverineSoft.SaveSystem.Editor
{
    using UnityEditor;

    [CustomEditor(typeof(SaveManager))]
    internal sealed class SaveManagerEditor : Editor
    {
        private static class Styles
        {
            public static GUIContent SaveButtonContent => 
                new GUIContent("Save Data", "Save current data of objects to file.");
        
            public static GUIContent LoadButtonContent =>
                new GUIContent("Load Data", "Load saved data into SaveManager.");
        
            public static GUIContent RestoreButtonContent =>
                new GUIContent("Restore Data", "Restore loaded data to objects.");
        
            public static GUIContent LoadAndRestoreButtonContent =>
                new GUIContent("Load And Restore Data", "Load saved data into SaveManager and restore to objects.");
        }
        
        public override void OnInspectorGUI()
        {
            // Draw the default inspector first
            base.OnInspectorGUI();

            //
            // Extra Editor Tools
            //

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Save Tools", EditorStyles.boldLabel);

            SaveManager manager = (SaveManager)target;

            ShowTools(manager);
        }

        public static void ShowTools(SaveManager manager)
        {
            SaveSystemSettings settings = manager.settings;
            SaveFile saveFile = settings?.saveFile;

            if (saveFile)
            {
                SaveButton(manager);
                
                //only show load buttons while Scene is playing (Prevents breaking scenes by loading)
                GUI.enabled = Application.isPlaying;
                LoadButton(manager);
                RestoreButton(manager);
                LoadAndRestoreButton(manager);
                GUI.enabled = true;

                SaveFileEditor.ClearButtons(saveFile, !settings.UseTemp, settings.showLogs);
            }
            else
            {
                EditorGUILayout.HelpBox("Assign Save File for Tools", MessageType.Error);
            }

        }

        private static void SaveButton(SaveManager manager)
        {
            if (GUILayout.Button(Styles.SaveButtonContent))
            {
                manager.SaveData();
            }
        }

        private static void LoadButton(SaveManager manager)
        {
            if (GUILayout.Button(Styles.LoadButtonContent))
            {
                manager.LoadData(restore: false);
            }
        }

        private static void RestoreButton(SaveManager manager)
        {
            if (GUILayout.Button(Styles.RestoreButtonContent))
            {
                manager.RestoreData();
            }
        }

        private static void LoadAndRestoreButton(SaveManager manager)
        {
            if (GUILayout.Button(Styles.LoadAndRestoreButtonContent))
            {
                manager.LoadData(restore: false);
            }
        }
    }
}