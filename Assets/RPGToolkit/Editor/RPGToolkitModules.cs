using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace RPGToolkit
{
    public class RPGToolkitModules
    {
        public const string EventsPath = "Assets/RPGToolkit/Prefabs/RPGToolkitEventSystem.prefab";
        public const string UIPath = "Assets/RPGToolkit/Prefabs/RPGToolkitUI.prefab";
        public const string PlayerPath = "Assets/RPGToolkit/Prefabs/RPGToolkitPlayer.prefab";
        public const string InventoryPath = "Assets/RPGToolkit/Prefabs/RPGToolkitInventory.prefab";
        public const string QuestPath = "Assets/RPGToolkit/Prefabs/RPGToolkitQuest.prefab";

        public const string InventoryUIPath = "Assets/RPGToolkit/Prefabs/Inventory/InventoryUI.prefab";
        public const string QuestUIPath = "Assets/RPGToolkit/Prefabs/Quest/QuestUI.prefab";
        public const string QuestSOPath = "Assets/Resources/RPGToolkit/Quests";
        public const string NPCPrefabPath = "Assets/RPGToolkit/Prefabs/Quest/QuestPoint2D.prefab";
        
        public static GameObject uiEvents, uiCanvas;
        public static GameObject inventoryUI, questUI;
        public static GameObject playerModule, inventoryModule, questModule;
        private static string modulePath;
        private static string moduleName;

        // RPG Toolkit UI
        [MenuItem("RPG Toolkit/Create RPGToolkit UI", false, 10)]
        public static GameObject CreateUI()
        {
            if (uiCanvas == null)
            {
                return CreateModule(UIPath, "UI Module");
            }

            return null;
        }

        [MenuItem("RPG Toolkit/Create RPGToolkit UI", true, 10)]
        public static bool ValidateCreateUI()
        {
            return uiCanvas == null && GameObject.FindWithTag("RPGToolkitUI") == null;
        }

        [MenuItem("RPG Toolkit/Create RPGToolkit UI Event System", false, 11)]
        public static GameObject CreateUIEventSystem()
        {
            if (uiEvents == null)
            {
                return CreateModule(EventsPath, "UI Events");
            }

            return null;
        }

        [MenuItem("RPG Toolkit/Create RPGToolkit UI Event System", true, 11)]
        public static bool ValidateCreateUIEventSystem()
        {
            return uiEvents == null && GameObject.FindWithTag("RPGToolkitUIEventSystem") == null;
        }

        // Player Module
        [MenuItem("RPG Toolkit/Create Player Module", false, 12)]
        public static GameObject CreatePlayer()
        {
            if (playerModule == null)
            {
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerTrigger"), LayerMask.NameToLayer("Item"), true);
                return CreateModule(PlayerPath, "Player Module");
            }

            return null;
        }

        [MenuItem("RPG Toolkit/Create Player Module", true, 12)]
        public static bool ValidateCreatePlayer()
        {
            return playerModule == null && GameObject.FindWithTag("RPGToolkitPlayer") == null;
        }

        // Inventory Module
        [MenuItem("RPG Toolkit/Create Inventory Module", false, 13)]
        public static GameObject CreateInventory()
        {
            if (inventoryModule == null)
            {
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerTrigger"), LayerMask.NameToLayer("Item"), true);
                return CreateModuleWithUI(InventoryPath, "Inventory Module", true);
            }

            return null;
        }

        [MenuItem("RPG Toolkit/Create Inventory Module", true, 13)]
        public static bool ValidateCreateInventory()
        {
            return inventoryModule == null && GameObject.FindWithTag("RPGToolkitInventory") == null;
        }

        // Quest Module
        [MenuItem("RPG Toolkit/Create Quest Module", false, 14)]
        public static GameObject CreateQuest()
        {
            if (questModule == null)
            {
                return CreateModuleWithUI(QuestPath, "Quest Module", false);
            }

            return null;
        }

        [MenuItem("RPG Toolkit/Create Quest Module", true, 14)]
        public static bool ValidateCreateQuest()
        {
            return questModule == null && GameObject.FindWithTag("RPGToolkitQuest") == null;
        }

        // Create new Quest
        [MenuItem("RPG Toolkit/Create New Quest", false, 15)]
        public static void CreateNewQuestSO()
        {
            if (questModule != null || GameObject.FindWithTag("RPGToolkitQuest") != null)
            {
                CreateNewQuest();
            }
        }

        [MenuItem("RPG Toolkit/Create New Quest", true, 15)]
        public static bool ValidateCreateNewQuestSO()
        {
            return questModule != null || GameObject.FindWithTag("RPGToolkitQuest") != null;
        }

        // Create new NPC
        [MenuItem("RPG Toolkit/Create New NPC", false, 16)]
        public static void CreateNewNPC()
        {
            if (questModule != null || GameObject.FindWithTag("RPGToolkitQuest") != null)
            {
                RPGToolkitNPCWindow.ShowCreateNPCWindow();
            }
            else
            {
                CreateBaseNPC();
            }
        }

        [MenuItem("RPG Toolkit/Create New NPC", true, 16)]
        public static bool ValidateCreateNewNPC()
        {
            return true;
        }

        private static GameObject CreateModule(string prefabPath, string moduleName)
        {
            GameObject modulePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (modulePrefab == null)
            {
                Debug.LogError("Module Creation Error : Prefab cannot be found at " + prefabPath);
            }

            GameObject moduleInstance = PrefabUtility.InstantiatePrefab(modulePrefab) as GameObject;
            if (moduleInstance != null)
            {
                Debug.Log("Module Creation Successful : " + moduleName + " is active.");
                Undo.RegisterCreatedObjectUndo(moduleInstance, "Create " + moduleInstance.name);
                Selection.activeObject = moduleInstance;

                switch (moduleName)
                {
                    case "UI Module":
                        uiCanvas = moduleInstance;
                        uiCanvas.GetComponent<Canvas>().worldCamera = Camera.main;

                        if (uiEvents == null)
                        {
                            GameObject eventsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(EventsPath);
                            GameObject eventsInstance = PrefabUtility.InstantiatePrefab(eventsPrefab) as GameObject;

                            if (eventsInstance != null)
                            {
                                uiEvents = eventsInstance;
                            }
                        }

                        if (inventoryModule != null)
                        {
                            InventoryReferences(inventoryModule);
                        }
                        break;
                    case "Player Module":
                        playerModule = moduleInstance;
                        playerModule.GetComponent<RPGToolkitPlayerSettings>().RPGToolkitAsset = AssetDatabase.LoadAssetAtPath<RPGToolkitManager>("Assets/RPGToolkit/RPGToolkitManager.asset");
                        break;
                    case "Inventory Module":
                        inventoryModule = moduleInstance;
                        break;
                    case "Quest Module":
                        questModule = moduleInstance;
                        questModule.GetComponent<RPGToolkitQuestSettings>().RPGToolkitAsset = AssetDatabase.LoadAssetAtPath<RPGToolkitManager>("Assets/RPGToolkit/RPGToolkitManager.asset");
                        break;
                }

                return moduleInstance;
            }
            else
            {
                Debug.LogError("Module Creation Error : Failed to instantiate prefab.");
                return null;
            }
        }

        private static GameObject CreateModuleWithUI(string prefabPath, string moduleName, bool needUIReference)
        {
            GameObject currModule = null;
            modulePath = prefabPath;
            RPGToolkitModules.moduleName = moduleName;

            uiCanvas = GameObject.FindWithTag("RPGToolkitUI");
            if (uiCanvas == null)
            {
                Debug.Log("Module Creation : UI Module not found, creating UI Module...");

                uiCanvas = CreateModule(UIPath, "UI Module");
                uiCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
            }

            if (uiCanvas.activeSelf)
            {
                switch (moduleName)
                {
                    case "Inventory Module":
                        if (inventoryUI != null && GameObject.FindWithTag("RPGToolkitInventoryUI") != null)
                        {
                            break;
                        }
                        inventoryUI = CreateModule(InventoryUIPath, "Inventory UI");
                        inventoryUI.transform.SetParent(uiCanvas.transform);
                        RectTransform invRect = inventoryUI.GetComponent<RectTransform>();
                        SetPadding(invRect, 0, 0);
                        currModule = inventoryUI;
                        break;
                    case "Quest Module":
                        if (questUI != null && GameObject.FindWithTag("RPGToolkitQuestUI") != null)
                        {
                            break;
                        }
                        questUI = CreateModule(QuestUIPath, "Quest UI");
                        questUI.transform.SetParent(uiCanvas.transform);
                        RectTransform questRect = questUI.GetComponent<RectTransform>();
                        SetPadding(questRect, 0, 0);
                        currModule = questUI;
                        break;
                }
            }

            if (needUIReference)
            {
                // Start waiting for the UI Module to become active
                EditorApplication.update += WaitForUIModule;
            }
            else
            {
                CreateModule(modulePath, moduleName);
            }

            return currModule;
        }

        private static void WaitForUIModule()
        {
            Debug.Log("Module Creation : Waiting for UI Module to fully initialize.");

            // Wait until the UI Module is active
            if (uiCanvas.activeSelf)
            {
                // Create the Module
                GameObject moduleInstance = CreateModule(modulePath, moduleName);
                if (moduleInstance != null)
                {
                    if (moduleName.Contains("Inventory"))
                    {
                        InventoryReferences(moduleInstance);
                    }

                    // Unsubscribe from the EditorApplication.update event to prevent further calls to WaitForUIModule
                    EditorApplication.update -= WaitForUIModule;
                }
            }
        }

        private static void InventoryReferences(GameObject moduleInstance)
        {
            Debug.Log("Inventory Module : Need References.");

            // Find objects with specific tags within the RPGToolkitUI
            List<InventorySlot> taggedObjects = new List<InventorySlot>();

            // Find InventoryHotbar objects and their children under uiModule
            foreach (Transform child in inventoryUI.transform)
            {
                if (child.CompareTag("RPGToolkitInventoryBar"))
                {
                    // Add InventorySlots from the hotbar with the name containing "BarSlot"
                    taggedObjects.AddRange(FindChildObjectsWithName(child, "BarSlot"));
                }
            }

            // Find InventoryBag objects and their children (including inactive) under uiModule
            foreach (Transform child in inventoryUI.transform)
            {
                // Check if the child's tag matches "InventoryBag" regardless of its active state
                if (child.tag == "RPGToolkitInventoryBag")
                {
                    // Add InventorySlots from the bag with the name containing "BarSlot"
                    taggedObjects.AddRange(FindChildObjectsWithNameRecursive(child, "BarSlot"));
                }
            }

            Debug.Log("Inventory Module : Found " + taggedObjects.Count + " InventorySlots.");

            // Set the references in InventoryManager.cs of the InventoryPrefab
            InventoryManager inventoryManager = moduleInstance.GetComponent<InventoryManager>();
            if (inventoryManager != null)
            {
                // Assign the found slots to the InventoryManager
                inventoryManager.invSlots = taggedObjects.ToArray();

                // Save the changes to the InventoryManager prefab
                PrefabUtility.SaveAsPrefabAsset(moduleInstance, modulePath);
                Debug.Log("Inventory Module : InventorySlots assigned to InventoryManager.");
            }
            else
            {
                Debug.LogWarning("Inventory Module : InventoryManager component not found in InventoryManager Prefab.");
            }
        }

        private static void CreateNewQuest()
        {
            string defaultFileName = "NewQuest.asset";

            if (!Directory.Exists(QuestSOPath))
            {
                Debug.Log("Quest Directory could not be found. Creating Directory...");

                Directory.CreateDirectory(QuestSOPath);

                Debug.Log("Quest Directory created: " + QuestSOPath);
            }

            // Prompt the user to specify the file name and path
            string fileName = EditorUtility.SaveFilePanel(
            "Save Quest ScriptableObject",
            QuestSOPath,
            defaultFileName,
            "asset"
            );

            // If the user cancels the dialog, fileName will be empty
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            Debug.Log("Creating new Quest Scriptable Object...");
            // Extract just the file name from the full path
            string assetFileName = Path.GetFileName(fileName);

            // Create the asset path using the predetermined directory and the user-specified file name
            string assetPath = Path.Combine(QuestSOPath, assetFileName);

            // Ensure the asset path has the correct extension
            if (!assetPath.EndsWith(".asset"))
            {
                assetPath += ".asset";
            }

            // Generate a unique path for the asset (if needed)
            assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            // Create the asset
            QuestInfoSO asset = ScriptableObject.CreateInstance<QuestInfoSO>();

            // Create the asset in the specified path
            AssetDatabase.CreateAsset(asset, assetPath);

            // Save the changes
            AssetDatabase.SaveAssets();

            Debug.Log("Successfully created new Quest Scriptable Object at: " + assetPath);

            // Focus on the project window
            EditorUtility.FocusProjectWindow();

            assetFileName = assetFileName.Replace(".asset", "");
            asset.questID = assetFileName;

            // Select the created asset
            Selection.activeObject = asset;
        }

        private static void CreateBaseNPC()
        {
            // Load the NPC prefab
            GameObject npcPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(NPCPrefabPath);
            if (npcPrefab == null)
            {
                Debug.LogError("NPC Creation Error: Prefab cannot be found at " + NPCPrefabPath);
                return;
            }

            // Instantiate the NPC prefab in the scene
            GameObject npcInstance = PrefabUtility.InstantiatePrefab(npcPrefab) as GameObject;
            if (npcInstance != null)
            {
                // Remove QuestPoint2D component
                QuestPoint2D questPointComponent = npcInstance.GetComponent<QuestPoint2D>();
                if (questPointComponent != null)
                {
                    Object.DestroyImmediate(questPointComponent);
                    Debug.Log("Removed QuestPoint2D component from the NPC instance.");
                }

                // Select the instantiated NPC object in the hierarchy
                Selection.activeObject = npcInstance;

                // Focus on the hierarchy window
                EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");

                // Rename the NPC instance
                npcInstance.name = "NewNPC";

                // Register the creation undo
                Undo.RegisterCreatedObjectUndo(npcInstance, "Create " + npcInstance.name);

                // Focus on the NPC instance to let the user rename it
                EditorGUIUtility.PingObject(npcInstance);

                Debug.Log("NPC Creation Successful: New NPC is created.");
            }
            else
            {
                Debug.LogError("NPC Creation Error: Failed to instantiate prefab.");
            }
        }

        private static List<InventorySlot> FindChildObjectsWithName(Transform parent, string slotNameContains)
        {
            List<InventorySlot> taggedObjects = new List<InventorySlot>();

            foreach (Transform child in parent)
            {
                // Check if the child's name contains the specified string
                if (child.name.Contains(slotNameContains))
                {
                    // Get the InventorySlot component from the child
                    InventorySlot slot = child.GetComponent<InventorySlot>();
                    if (slot != null)
                    {
                        taggedObjects.Add(slot);
                    }
                }
            }

            return taggedObjects;
        }

        private static List<InventorySlot> FindChildObjectsWithNameRecursive(Transform parent, string slotNameContains)
        {
            List<InventorySlot> taggedObjects = new List<InventorySlot>();

            FindChildObjectsWithNameRecursive(parent, slotNameContains, taggedObjects);

            return taggedObjects;
        }

        private static void FindChildObjectsWithNameRecursive(Transform parent, string slotNameContains, List<InventorySlot> taggedObjects)
        {
            foreach (Transform child in parent)
            {
                // Check if the child's name contains the specified string
                if (child.name.Contains(slotNameContains))
                {
                    // Get the InventorySlot component from the child
                    InventorySlot slot = child.GetComponent<InventorySlot>();
                    if (slot != null)
                    {
                        taggedObjects.Add(slot);
                    }
                }

                // Recursively search through all child objects
                FindChildObjectsWithNameRecursive(child, slotNameContains, taggedObjects);
            }
        }

        private static void SetPadding(RectTransform rect, float horizontal, float vertical)
        {
            rect.localScale = new Vector3(1, 1, 1);
            rect.offsetMax = new Vector2(-horizontal, -vertical);
            rect.offsetMin = new Vector2(horizontal, vertical);
        }

        private static void SetPadding(RectTransform rect, float left, float top, float right, float bottom)
        {
            rect.localScale = new Vector3(1, 1, 1);
            rect.offsetMax = new Vector2(-right, -top);
            rect.offsetMin = new Vector2(left, bottom);
        }
    }

    public class RPGToolkitNPCWindow : EditorWindow
    {
        private QuestInfoSO selectedQuest;

        public static void ShowCreateNPCWindow()
        {
            var window = GetWindow<RPGToolkitNPCWindow>("Create NPC with Quest");
            Vector2 windowSize = new Vector2(300, 150);
            window.minSize = windowSize;
            window.maxSize = windowSize;
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Select Quest for NPC", EditorStyles.boldLabel);
            selectedQuest = (QuestInfoSO)EditorGUILayout.ObjectField("Quest Info", selectedQuest, typeof(QuestInfoSO), false);

            if (GUILayout.Button("Create NPC"))
            {
                CreateNPCWithQuest();
                Close();
            }
        }

        private void CreateNPCWithQuest()
        {
            if (selectedQuest == null)
            {
                EditorUtility.DisplayDialog("Warning", "You have Quest Module enabled but did not select a QuestInfoSO for this NPC.", "Ok");
            }

            GameObject npcPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RPGToolkitModules.NPCPrefabPath);

            if (npcPrefab == null)
            {
                Debug.LogError("NPC Creation Error : Prefab cannot be found at " + RPGToolkitModules.NPCPrefabPath);
                return;
            }

            // Instantiate the NPC prefab in the scene
            GameObject npcInstance = PrefabUtility.InstantiatePrefab(npcPrefab) as GameObject;

            if (npcInstance != null)
            {
                // Set the questInfoForPoint reference
                var questPointComponent = npcInstance.GetComponent<QuestPoint2D>();

                if (questPointComponent != null && selectedQuest != null)
                {
                    questPointComponent.questInfoForPoint = selectedQuest;
                    Debug.Log("Quest assigned to NPC : " + selectedQuest.name);
                }
                else if (questPointComponent == null)
                {
                    Debug.LogError("QuestPoint2D component not found in NPC prefab.");
                }
                else if (selectedQuest == null)
                {
                    Debug.LogWarning("No QuestInfoSO was selected when creating NPC.");
                }

                // Select the instantiated NPC object in the hierarchy
                Selection.activeObject = npcInstance;

                // Focus on the hierarchy window
                EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");

                // Rename the NPC instance
                npcInstance.name = "NewNPCWithQuest";

                // Register the creation undo
                Undo.RegisterCreatedObjectUndo(npcInstance, "Create " + npcInstance.name);

                // Focus on the NPC instance to let the user rename it
                EditorGUIUtility.PingObject(npcInstance);

                Debug.Log("NPC Creation Successful : New NPC created.");
            }
            else
            {
                Debug.LogError("NPC Creation Error : Failed to instantiate prefab.");
            }
        }
    }
}
