using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RPGToolkitModules
{
    private const string UIPath = "Assets/Prefabs/RPGToolkitUI.prefab";
    private const string InventoryPath = "Assets/Prefabs/Inventory/InventoryManager.prefab";
    private const string QuestPath = "Assets/Prefabs/Quest/QuestManager.prefab";

    private static GameObject uiModule, inventoryModule, questModule;
    private static string modulePath;
    private static string moduleName;

    // RPG Toolkit UI
    [MenuItem("RPG Toolkit/Create RPGToolkit UI", false, 10)]
    static void CreateUI()
    {
        if (uiModule == null)
        {
            CreateModule(UIPath, "UI Module");
        }
    }
    [MenuItem("RPG Toolkit/Create RPGToolkit UI", true, 10)]
    static bool ValidateCreateUI()
    {
        return uiModule == null;
    }

    // Inventory Module
    [MenuItem("RPG Toolkit/Create Inventory Module", false, 11)]
    static void CreateInventory()
    {
        if (inventoryModule == null)
        {
            CreateModuleWithUI(InventoryPath, "Inventory Module", true);
        }
    }
    [MenuItem("RPG Toolkit/Create Inventory Module", true, 11)]
    static bool ValidateCreateInventory()
    {
        return inventoryModule == null;
    }

    // Quest Module
    [MenuItem("RPG Toolkit/Create Quest Module", false, 12)]
    static void CreateQuest()
    {
        if (questModule == null)
        {
            CreateModuleWithUI(QuestPath, "Quest Module", false);
        }
    }
    [MenuItem("RPG Toolkit/Create Quest Module", true, 12)]
    static bool ValidateCreateQuest()
    {
        return questModule == null;
    }

    static GameObject CreateModule(string prefabPath, string moduleName)
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
                    uiModule = moduleInstance;
                    break;
                case "Inventory Module":
                    inventoryModule = moduleInstance;
                    break;
                case "Quest Module":
                    questModule = moduleInstance;
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

    static void CreateModuleWithUI(string prefabPath, string moduleName, bool needUIReference)
    {
        modulePath = prefabPath;
        RPGToolkitModules.moduleName = moduleName;

        uiModule = GameObject.FindWithTag("RPGToolkitUI");
        if (uiModule == null)
        {
            Debug.Log("Module Creation : UI Module not found, creating UI Module...");

            uiModule = CreateModule(UIPath, "UI Module");
            uiModule.GetComponent<Canvas>().worldCamera = Camera.main;
        }

        if (needUIReference)
        {
            // Start waiting for the UI Module to become active
            EditorApplication.update += WaitForUIModule;
        }
        else
        {
            CreateModule(modulePath, "");
        }
    }

    static void WaitForUIModule()
    {
        Debug.Log("Module Creation : Waiting for UI Module to fully initialize.");

        // Wait until the UI Module is active
        if (uiModule.activeSelf)
        {
            // Create the Module
            GameObject moduleInstance = CreateModule(modulePath, moduleName);
            if (moduleInstance != null)
            {
                if (modulePath.Contains("Inventory"))
                {
                    Debug.Log("Inventory Module : Need References.");

                    // Find objects with specific tags within the RPGToolkitUI
                    List<InventorySlot> taggedObjects = new List<InventorySlot>();

                    // Find InventoryHotbar objects and their children under uiModule
                    foreach (Transform child in uiModule.transform)
                    {
                        if (child.CompareTag("InventoryHotbar"))
                        {
                            // Add InventorySlots from the hotbar with the name containing "BarSlot"
                            taggedObjects.AddRange(FindChildObjectsWithName(child, "BarSlot"));
                        }
                    }

                    // Find InventoryBag objects and their children (including inactive) under uiModule
                    foreach (Transform child in uiModule.transform)
                    {
                        // Check if the child's tag matches "InventoryBag" regardless of its active state
                        if (child.tag == "InventoryBag")
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

                // Unsubscribe from the EditorApplication.update event to prevent further calls to WaitForUIModule
                EditorApplication.update -= WaitForUIModule;
            }
        }
    }

    static List<InventorySlot> FindChildObjectsWithName(Transform parent, string slotNameContains)
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

    static List<InventorySlot> FindChildObjectsWithNameRecursive(Transform parent, string slotNameContains)
    {
        List<InventorySlot> taggedObjects = new List<InventorySlot>();

        FindChildObjectsWithNameRecursive(parent, slotNameContains, taggedObjects);

        return taggedObjects;
    }

    static void FindChildObjectsWithNameRecursive(Transform parent, string slotNameContains, List<InventorySlot> taggedObjects)
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
}
