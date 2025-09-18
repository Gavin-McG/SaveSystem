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
        }
        
        public override void OnInspectorGUI()
        {
            // Draw the default inspector first
            base.OnInspectorGUI();

            //
            // Extra Editor Tools
            //
        
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);
        
            var saveFiles = targets.OfType<SaveFile>().ToList();
        
            // Clear all Button
            if (GUILayout.Button(Styles.ClearAllContent))
            {
                foreach (var saveFile in saveFiles)
                {
                    saveFile.ClearSave();
                }
            }
            
            // Clear temp button
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button(Styles.ClearTempContent))
            {
                foreach (var saveFile in saveFiles)
                {
                    saveFile.ClearTempSave();
                }
            }
            GUI.enabled = true;
        }

        public static void ClearButtons(SaveFile saveFile, bool showall=true, bool showLogs=true)
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Save File Tools");
            
            // Clear all Button
            GUI.enabled = showall;
            if (GUILayout.Button(Styles.ClearAllContent))
            {
                saveFile.ClearSave(showLogs);
            }
            GUI.enabled = true;
            
            //  temp button
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button(Styles.ClearTempContent))
            {
                saveFile.ClearTempSave(showLogs);
            }
            GUI.enabled = true;
        }
    }
}


