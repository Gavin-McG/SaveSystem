using System.Linq;
using UnityEngine;

namespace WolverineSoft.SaveSystem.Editor
{
    using UnityEditor;
    
    [CustomEditor(typeof(SaveFile)), CanEditMultipleObjects]
    internal sealed class SaveFileEditor : Editor
    {
        private static class Styles
        {
            public static GUIContent ClearAllContent => 
                new GUIContent("Clear All Save Data", "Clears all file data, Including Temp save data. Will not alter the currently playing scene or loaded data.");

            public static GUIContent ClearTempContent =>
                new GUIContent("Clear Temp Save Data", "Clears the temp save data. Will not alter the currently playing scene or loaded data.");

            public static GUIContent OpenFolderContent =>
                new GUIContent("Open Save Folder", "Opens the save system base directory in Explorer/Finder.");
        }
        
        public override void OnInspectorGUI()
        {
            // Draw default inspector
            base.OnInspectorGUI();

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);

            var saveFiles = targets.OfType<SaveFile>().ToList();

            // Open folder button
            if (GUILayout.Button(Styles.OpenFolderContent))
            {
                EditorUtility.RevealInFinder(SaveSystemIO.BasePath);
            }

            // Clear all
            if (GUILayout.Button(Styles.ClearAllContent))
            {
                foreach (var saveFile in saveFiles)
                    saveFile.ClearSave();
            }

            // Clear temp
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button(Styles.ClearTempContent))
            {
                foreach (var saveFile in saveFiles)
                    saveFile.ClearTempSave();
            }
            GUI.enabled = true;
        }

        public static void ClearButtons(SaveFile saveFile, bool showall=true, bool showLogs=true)
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Save File Tools");

            // Open folder button
            if (GUILayout.Button(Styles.OpenFolderContent))
            {
                string filePath = SaveSystemIO.BasePath + "/";
                EditorUtility.RevealInFinder(filePath);
            }

            // Clear all
            GUI.enabled = showall;
            if (GUILayout.Button(Styles.ClearAllContent))
            {
                saveFile.ClearSave(showLogs);
            }
            GUI.enabled = true;

            // Clear temp
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button(Styles.ClearTempContent))
            {
                saveFile.ClearTempSave(showLogs);
            }
            GUI.enabled = true;
        }
    }
}