using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using TMPro;

namespace RPGToolkit
{
    public class RPGToolkitModules
    {
        public const string EventsPath = "Assets/RPGToolkit/Prefabs/RPGToolkitEventSystem.prefab";
        public const string UIPath = "Assets/RPGToolkit/Prefabs/RPGToolkitUI.prefab";
        public const string PlayerPath = "Assets/RPGToolkit/Prefabs/RPGToolkitPlayer.prefab";
        public const string InventoryPath = "Assets/RPGToolkit/Prefabs/RPGToolkitInventory.prefab";
        public const string QuestPath = "Assets/RPGToolkit/Prefabs/RPGToolkitQuest.prefab";
        public const string DialoguePath = "Assets/RPGToolkit/Prefabs/RPGToolkitDialogue.prefab";
        public const string HealthUIPath = "Assets/RPGToolkit/Prefabs/Player/HealthUI.prefab";
        public const string ManaUIPath = "Assets/RPGToolkit/Prefabs/Player/ManaUI.prefab";

        public const string InventoryUIPath = "Assets/RPGToolkit/Prefabs/Inventory/InventoryUI.prefab";
        public const string ItemSOPath = "Assets/Resources/RPGToolkit/Items";
        public const string QuestUIPath = "Assets/RPGToolkit/Prefabs/Quest/QuestUI.prefab";
        public const string QuestSOPath = "Assets/Resources/RPGToolkit/Quests";
        public const string NPCPrefabPath = "Assets/RPGToolkit/Prefabs/Quest/QuestPoint2D.prefab";
        public const string DialogueUIPath = "Assets/RPGToolkit/Prefabs/Dialogue/DialogueUI.prefab";
        
        public static GameObject uiEvents, uiCanvas;
        public static GameObject inventoryUI, questUI, dialogueUI, healthUI, manaUI;
        public static GameObject playerModule, inventoryModule, questModule, dialogueModule;
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

        // Health UI
        [MenuItem("RPG Toolkit/Create Health UI", false, 13)]
        public static GameObject CreateHealthUI()
        {
            if (healthUI == null || !GameObject.FindWithTag("RPGToolkitHealthUI"))
            {
                return CreateUI(HealthUIPath);
            }

            return null;
        }

        [MenuItem("RPG Toolkit/Create Health UI", true, 13)]
        public static bool ValidateCreateHealthUI()
        {
            return GameObject.FindWithTag("RPGToolkitPlayer") != null && GameObject.FindWithTag("RPGToolkitHealthUI") == null;
        }

        // Mana UI
        [MenuItem("RPG Toolkit/Create Mana UI", false, 14)]
        public static GameObject CreateManaUI()
        {
            if (manaUI == null || !GameObject.FindWithTag("RPGToolkitManaUI"))
            {
                return CreateUI(ManaUIPath);
            }

            return null;
        }

        [MenuItem("RPG Toolkit/Create Mana UI", true, 14)]
        public static bool ValidateCreateManaUI()
        {
            return GameObject.FindWithTag("RPGToolkitPlayer") != null && GameObject.FindWithTag("RPGToolkitManaUI") == null;
        }

        // Inventory Module
        [MenuItem("RPG Toolkit/Create Inventory Module", false, 15)]
        public static GameObject CreateInventory()
        {
            if (inventoryModule == null)
            {
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerTrigger"), LayerMask.NameToLayer("Item"), true);
                return CreateModuleWithUI(InventoryPath, "Inventory Module", true);
            }

            return null;
        }

        [MenuItem("RPG Toolkit/Create Inventory Module", true, 15)]
        public static bool ValidateCreateInventory()
        {
            return inventoryModule == null && GameObject.FindWithTag("RPGToolkitInventory") == null;
        }

        // Create new Item
        [MenuItem("RPG Toolkit/Create New Item", false, 16)]
        public static void CreateNewItemSO()
        {
            CreateNewItem();
        }

        [MenuItem("RPG Toolkit/Create New Item", true, 16)]
        public static bool ValidateCreateNewItemSO()
        {
            return true;
        }

        // Quest Module
        [MenuItem("RPG Toolkit/Create Quest Module", false, 17)]
        public static GameObject CreateQuest()
        {
            if (questModule == null)
            {
                return CreateModuleWithUI(QuestPath, "Quest Module", false);
            }

            return null;
        }

        [MenuItem("RPG Toolkit/Create Quest Module", true, 17)]
        public static bool ValidateCreateQuest()
        {
            return questModule == null && GameObject.FindWithTag("RPGToolkitQuest") == null;
        }

        // Create new Quest
        [MenuItem("RPG Toolkit/Create New Quest", false, 18)]
        public static void CreateNewQuestSO()
        {
            if (questModule != null || GameObject.FindWithTag("RPGToolkitQuest") != null)
            {
                CreateNewQuest();
            }
        }

        [MenuItem("RPG Toolkit/Create New Quest", true, 18)]
        public static bool ValidateCreateNewQuestSO()
        {
            return questModule != null || GameObject.FindWithTag("RPGToolkitQuest") != null;
        }

        // Create new NPC
        [MenuItem("RPG Toolkit/Create New NPC", false, 19)]
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

        [MenuItem("RPG Toolkit/Create New NPC", true, 19)]
        public static bool ValidateCreateNewNPC()
        {
            return true;
        }

        // Dialogue Module
        [MenuItem("RPG Toolkit/Create Dialogue Module", false, 20)]
        public static GameObject CreateDialogueModule()
        {
            if (dialogueModule == null)
            {
                return CreateModuleWithUI(DialoguePath, "Dialogue Module", true);
            }

            return null;
        }

        [MenuItem("RPG Toolkit/Create Dialogue Module", true, 20)]
        public static bool ValidateCreateDialogueModule()
        {
            return dialogueModule == null && GameObject.FindWithTag("RPGToolkitDialogue") == null;
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
                        if (dialogueModule != null)
                        {
                            DialogueReferences(dialogueModule);
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
                    case "Dialogue Module":
                        dialogueModule = moduleInstance;
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
                    case "Dialogue Module":
                        if (dialogueUI != null && GameObject.FindWithTag("RPGToolkitDialogueUI") != null)
                        {
                            break;
                        }
                        dialogueUI = CreateModule(DialogueUIPath, "Dialogue UI");
                        dialogueUI.transform.SetParent(uiCanvas.transform);
                        RectTransform dialogueRect = dialogueUI.GetComponent<RectTransform>();
                        SetPadding(dialogueRect, 0, 0);
                        currModule = dialogueUI;
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

        private static GameObject CreateUI(string prefabPath)
        {
            uiCanvas = GameObject.FindWithTag("RPGToolkitUI");

            if (uiCanvas != null)
            {
                GameObject uiPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                GameObject uiInstance = PrefabUtility.InstantiatePrefab(uiPrefab) as GameObject;

                if (uiInstance != null)
                {
                    uiInstance.transform.SetParent(uiCanvas.transform);
                    RectTransform uiRect = uiInstance.GetComponent<RectTransform>();
                    SetPadding(uiRect, 0, 0);

                    switch (uiInstance.name)
                    {
                        case "HealthUI":
                            healthUI = uiInstance;
                            break;
                        case "ManaUI":
                            manaUI = uiInstance;
                            break;
                    }

                    return uiInstance;
                }
            }

            return null;
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
                    else if (moduleName.Contains("Dialogue"))
                    {
                        DialogueReferences(moduleInstance);
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

        private static void DialogueReferences(GameObject moduleInstance)
        {
            Debug.Log("Dialogue Module : Need References.");

            DialogueManager dialogueManager = moduleInstance.GetComponent<DialogueManager>();

            dialogueManager.dialogueUI = GameObject.Find("DialogueUI");
            dialogueManager.speakerSprite = GameObject.Find("SpeakerPortrait").GetComponent<Image>();
            dialogueManager.speakerNameText = GameObject.Find("SpeakerName").GetComponent<TMP_Text>();
            dialogueManager.dialogueText = GameObject.Find("SpeakerDialogue").GetComponent<TMP_Text>();
            dialogueManager.optionsPanel = GameObject.Find("DialogueOptionsPanel");
            dialogueManager.dialoguePanel = GameObject.Find("DialoguePanel");
        }

        private static void CreateNewQuest()
        {
            string defaultFileName = "NewQuest.asset";

            if (!Directory.Exists(QuestSOPath))
            {
                Debug.Log("Quest Directory could not be found. Creating Directory...");

                Directory.CreateDirectory(QuestSOPath);

                Debug.Log("Quest Directory created : " + QuestSOPath);
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

            Debug.Log("Successfully created new Quest Scriptable Object at : " + assetPath);

            // Focus on the project window
            EditorUtility.FocusProjectWindow();

            assetFileName = assetFileName.Replace(".asset", "");
            asset.questID = assetFileName;

            // Select the created asset
            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }

        private static void CreateNewItem()
        {
            string defaultFileName = "NewItem.asset";

            if (!Directory.Exists(ItemSOPath))
            {
                Debug.Log("Item Directory could not be found. Creating Directory...");

                Directory.CreateDirectory(ItemSOPath);

                Debug.Log("Item Directory created : " + ItemSOPath);
            }

            // Prompt the user to specify the file name and path
            string fileName = EditorUtility.SaveFilePanel(
            "Save Item ScriptableObject",
            ItemSOPath,
            defaultFileName,
            "asset"
            );

            // If the user cancels the dialog, fileName will be empty
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            Debug.Log("Creating new Item Scriptable Object...");
            // Extract just the file name from the full path
            string assetFileName = Path.GetFileName(fileName);

            // Create the asset path using the predetermined directory and the user-specified file name
            string assetPath = Path.Combine(ItemSOPath, assetFileName);

            // Ensure the asset path has the correct extension
            if (!assetPath.EndsWith(".asset"))
            {
                assetPath += ".asset";
            }

            // Generate a unique path for the asset (if needed)
            assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            // Create the asset
            ItemInfoSO asset = ScriptableObject.CreateInstance<ItemInfoSO>();

            // Create the asset in the specified path
            AssetDatabase.CreateAsset(asset, assetPath);

            // Save the changes
            AssetDatabase.SaveAssets();

            Debug.Log("Successfully created new Item Scriptable Object at : " + assetPath);

            // Focus on the project window
            EditorUtility.FocusProjectWindow();

            // Select the created asset
            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
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

                // Prompt the user to select a sprite
                Sprite sprite = EditorGUILayout.ObjectField("Select Sprite", null, typeof(Sprite), false) as Sprite;
                if (sprite != null)
                {
                    // Set the SpriteRenderer's image
                    SpriteRenderer spriteRenderer = npcInstance.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.sprite = sprite;
                        Debug.Log("Sprite updated for the NPC instance.");
                    }
                    else
                    {
                        Debug.LogWarning("SpriteRenderer component not found in the NPC prefab.");
                    }
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
        private Sprite selectedSprite;

        public static void ShowCreateNPCWindow()
        {
            var window = GetWindow<RPGToolkitNPCWindow>("Create NPC with Quest");
            Vector2 windowSize = new Vector2(300, 200); // Increased height for the additional field
            window.minSize = windowSize;
            window.maxSize = windowSize;
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Select Quest and Sprite for NPC", EditorStyles.boldLabel);
            
            // Field for selecting QuestInfoSO
            selectedQuest = (QuestInfoSO)EditorGUILayout.ObjectField("Quest Info", selectedQuest, typeof(QuestInfoSO), false);
            
            // Field for selecting Sprite
            selectedSprite = (Sprite)EditorGUILayout.ObjectField("Select Sprite", selectedSprite, typeof(Sprite), false);

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
                return;
            }

            GameObject npcPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RPGToolkitModules.NPCPrefabPath);

            if (npcPrefab == null)
            {
                Debug.LogError("NPC Creation Error: Prefab cannot be found at " + RPGToolkitModules.NPCPrefabPath);
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

                // Find the SpriteRenderer component in child objects
                SpriteRenderer spriteRenderer = npcInstance.GetComponentInChildren<SpriteRenderer>();

                if (spriteRenderer != null && selectedSprite != null)
                {
                    spriteRenderer.sprite = selectedSprite;
                    Debug.Log("Sprite updated for the NPC instance.");
                }
                else if (spriteRenderer == null)
                {
                    Debug.LogWarning("SpriteRenderer component not found in the NPC prefab's children.");
                }
                else if (selectedSprite == null)
                {
                    Debug.LogWarning("No sprite selected for the NPC.");
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

                Debug.Log("NPC Creation Successful : New NPC created (Please remember to set NPCSprite's Scale size).");
            }
            else
            {
                Debug.LogError("NPC Creation Error : Failed to instantiate prefab.");
            }
        }
    }
}
