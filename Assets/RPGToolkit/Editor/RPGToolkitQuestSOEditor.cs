using UnityEngine;
using UnityEditor;

namespace RPGToolkit
{
    [CustomEditor(typeof(QuestInfoSO))]
    public class RPGToolkitQuestSOEditor : Editor
    {
        SerializedProperty questStepsProp;

        private void OnEnable()
        {
            questStepsProp = serializedObject.FindProperty("questSteps");
        }

        public override void OnInspectorGUI()
        {
            // Draw the default inspector properties excluding questSteps
            DrawPropertiesExcluding(serializedObject, "questSteps");

            // Draw the questSteps property
            EditorGUILayout.PropertyField(questStepsProp, true);

            // Draw the button below the Steps property
            if (GUILayout.Button("Add new Collection Quest Step"))
            {
                QuestInfoSO questInfo = (QuestInfoSO)target;

                string templatePath = "Assets/RPGToolkit/Resources/Quests/Templates/CollectQuestStep.prefab";
                GameObject templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(templatePath);

                if (templatePrefab != null)
                {
                    // Show the input window to get the new prefab name
                    PrefabNameInputWindow.OnNameEntered = (newName) => CreateNewPrefab(questInfo, templatePrefab, newName);
                    PrefabNameInputWindow.ShowWindow();
                }
                else
                {
                    Debug.LogError("Template prefab not found at path: " + templatePath);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void CreateNewPrefab(QuestInfoSO questInfo, GameObject templatePrefab, string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                Debug.LogError("Quest Step name cannot be empty.");
                return;
            }

            // Create a new instance of the prefab
            GameObject newObject = Instantiate(templatePrefab);
            newObject.name = newName;

            // Specify the directory to save the prefab
            string directory = "Assets/Resources/RPGToolkit/Quests/QuestPrefabs/";

            // Ensure the directory exists
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            if (!AssetDatabase.IsValidFolder("Assets/Resources/RPGToolkit"))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "RPGToolkit");
            }
            if (!AssetDatabase.IsValidFolder("Assets/Resources/RPGToolkit/Quests"))
            {
                AssetDatabase.CreateFolder("Assets/Resources/RPGToolkit", "Quests");
            }
            if (!AssetDatabase.IsValidFolder("Assets/Resources/RPGToolkit/Quests/QuestPrefabs"))
            {
                AssetDatabase.CreateFolder("Assets/Resources/RPGToolkit/Quests", "QuestPrefabs");
            }

            // Create a unique path for the new prefab
            string prefabPath = AssetDatabase.GenerateUniqueAssetPath(directory + newObject.name + ".prefab");

            // Save the new GameObject as a prefab
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(newObject, prefabPath);

            if (prefab != null)
            {
                // Add the new prefab to the questSteps array
                int newLength = questInfo.questSteps.Length + 1;
                GameObject[] newQuestSteps = new GameObject[newLength];
                questInfo.questSteps.CopyTo(newQuestSteps, 0);
                newQuestSteps[newLength - 1] = prefab;
                questInfo.questSteps = newQuestSteps;
                EditorUtility.SetDirty(questInfo);
            }
            else
            {
                Debug.LogError("Failed to create prefab at path: " + prefabPath);
            }

            // Destroy the temporary GameObject
            DestroyImmediate(newObject);
        }
    }
}