using UnityEditor;
using UnityEngine;

namespace WolverineSoft.SaveSystem.Editor
{

    /// <summary>
    /// Editor Window for easy access to Save Settings and Tools without searching assets or components
    /// </summary>
    public class SaveSystemWindow : EditorWindow
    {
        SaveManager assignedSaveManager;

        private GUIContent ManagerIDContent => 
            new GUIContent("Manager ID", SaveManager.Styles.managerIDTooltip);
        
        private GUIContent ManagerSettingsContent => 
            new GUIContent("Settings", SaveManager.Styles.settingsTooltip);

        private GUIContent SaveFileContent =>
            new GUIContent("Active Save File", SaveSystemSettings.Styles.saveFileTooltip);

        private GUIContent UseTempSaveContent =>
            new GUIContent("Use Temporary Save", SaveSystemSettings.Styles.useTemporarySaveTooltip);

        private GUIContent ShowLogsContent =>
            new GUIContent("Show Log Output", SaveSystemSettings.Styles.showLogsTooltip);
        
        private GUIContent showWarningContent =>
            new GUIContent("Show Warning Output", SaveSystemSettings.Styles.showLogsTooltip);


        [MenuItem("Window/WolverineSoft/Save System Settings")]
        public static void ShowWindow()
        {
            var window = GetWindow<SaveSystemWindow>("Save System");
            window.Show();
        }

        private void OnGUI()
        {
            //manual assignment
            assignedSaveManager = (SaveManager)EditorGUILayout.ObjectField("Override Manager", assignedSaveManager,
                typeof(SaveManager), true);
            var primarySaveManager = SaveManager.Instance;

            //Show editor tools if Manager is available
            if (assignedSaveManager == null && primarySaveManager != null)
            {
                EditorGUILayout.HelpBox("Editing Primary SaveManager", MessageType.Info);
                ShowManagerOptions(primarySaveManager);
            }
            else
            {
                ShowAssignableManager();
            }
        }

        private void ShowManagerOptions(SaveManager manager)
        {
            ShowSettings(manager);

            EditorGUILayout.Separator();

            ShowManagerTools(manager);
        }

        private void ShowSettings(SaveManager manager)
        {
            SetManagerFields(manager);

            var settings = manager.settings;

            if (settings == null)
                EditorGUILayout.HelpBox("Assign a SaveSystemSettings asset to edit.", MessageType.Error);
            else
                SetSettingsFields(settings);
        }

        private void SetManagerFields(SaveManager manager)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Save Manager", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();

            manager.managerID =
                EditorGUILayout.TextField(ManagerIDContent, manager.managerID);
            manager.settings = (SaveSystemSettings)
                EditorGUILayout.ObjectField(ManagerSettingsContent, manager.settings, typeof(SaveSystemSettings),
                    false);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(manager);
            }
        }

        private void SetSettingsFields(SaveSystemSettings settings)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Save System Settings", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();

            settings.saveFile = (SaveFile)
                EditorGUILayout.ObjectField(SaveFileContent, settings.saveFile, typeof(SaveFile), false);
            settings.useTemporarySave =
                EditorGUILayout.Toggle(UseTempSaveContent, settings.useTemporarySave);
            settings.showLogs =
                EditorGUILayout.Toggle(ShowLogsContent, settings.showLogs);
            settings.showWarnings =
                EditorGUILayout.Toggle(showWarningContent, settings.showWarnings);

            if (EditorGUI.EndChangeCheck())
            {
                settings.TryUpdateTempFile();
                EditorUtility.SetDirty(settings);
            }
        }

        private void ShowManagerTools(SaveManager manager)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Save Manager Tools", EditorStyles.boldLabel);

            SaveManagerEditor.ShowTools(manager);
        }

        private void ShowAssignableManager()
        {
            if (assignedSaveManager != null)
            {
                EditorGUILayout.HelpBox($"Editing {assignedSaveManager.gameObject.name}", MessageType.Info);
                ShowManagerOptions(assignedSaveManager);
            }
            else
            {
                EditorGUILayout.HelpBox("No Primary SaveManager found; Assign SaveManagaer to Edit", MessageType.Error);
            }
        }
    }
}