using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RPGToolkitModules
{
    private const string UIPath = "Assets/Prefabs/RPGToolkitUI.prefab";
    private const string InventoryPath = "Assets/Prefabs/Inventory/InventoryManager.prefab";
    private const string QuestPath = "Assets/Prefabs/Quest/QuestManager.prefab";

    private static GameObject uiModule;
    private static string modulePath;
    private static string moduleName;
    private static bool inventoryModuleExist = false;

    [MenuItem("GameObject/RPG Toolkit Modules/Create RPGToolkit UI", false, 10)]
    static void CreateUI()
    {
        CreateModule(UIPath, "UI Module");
    }

    [MenuItem("GameObject/RPG Toolkit Modules/Create Inventory Module", false, 11)]
    static void CreateInventory()
    {
        CreateModuleWithUI(InventoryPath, "Inventory Module", true);
    }

    [MenuItem("GameObject/RPG Toolkit Modules/Create Quest Module", false, 12)]
    static void CreateQuest()
    {
        CreateModuleWithUI(QuestPath, "Quest Module", false);
    }

    static GameObject CreateModule(string prefabPath, string moduleName)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError("Module Creation Error : Prefab cannot be found at " + prefabPath);
        }

        GameObject moduleInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        if (moduleInstance != null)
        {
            Undo.RegisterCreatedObjectUndo(moduleInstance, "Create " + moduleInstance.name);
            Selection.activeObject = moduleInstance;

            Debug.Log("Module Creation Successful : " + moduleName + " is active.");

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
