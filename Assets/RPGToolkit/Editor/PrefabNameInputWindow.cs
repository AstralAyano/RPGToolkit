using System;
using UnityEngine;
using UnityEditor;

namespace RPGToolkit
{
    public class PrefabNameInputWindow : EditorWindow
    {
        public static Action<string> OnNameEntered;
        private string prefabName = "";

        public static void ShowWindow()
        {
            var window = GetWindow<PrefabNameInputWindow>("Enter Quest Step Name");
            window.ShowUtility();
        }

        private void OnGUI()
        {
            GUILayout.Label("Enter name of the Quest Step : ", EditorStyles.label);
            prefabName = EditorGUILayout.TextField("Quest Step Name", prefabName);

            if (GUILayout.Button("Confirm"))
            {
                OnNameEntered?.Invoke(prefabName);
                Close();
            }

            if (GUILayout.Button("Cancel"))
            {
                Close();
            }
        }
    }
}