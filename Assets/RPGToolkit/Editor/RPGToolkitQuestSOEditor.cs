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
            // Update the serialized object to reflect the latest changes
            serializedObject.Update();

            // Draw the default inspector properties excluding questSteps
            DrawPropertiesExcluding(serializedObject, "questSteps");

            // Draw the questSteps property
            EditorGUILayout.PropertyField(questStepsProp, true);

            // Draw the button directly below the Steps property
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

            // Apply the modified properties to update the serialized object
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
                // Update the questSteps array
                int newLength = questInfo.questSteps.Length + 1;
                GameObject[] newQuestSteps = new GameObject[newLength];
                questInfo.questSteps.CopyTo(newQuestSteps, 0);
                newQuestSteps[newLength - 1] = prefab;
                questInfo.questSteps = newQuestSteps;
                
                // Apply changes to the serialized object
                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();

                // Mark the asset as dirty
                EditorUtility.SetDirty(questInfo);

                EditorGUIUtility.PingObject(prefab);
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