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
    private static bool inventoryModuleExist = false;

    [MenuItem("GameObject/RPG Toolkit Modules/Create RPGToolkit UI", false, 10)]
    static void CreateUI()
    {
        CreateModule(UIPath);
    }

    [MenuItem("GameObject/RPG Toolkit Modules/Create Inventory Module", false, 11)]
    static void CreateInventory()
    {
        CreateModuleWithUI(InventoryPath, true);
    }

    [MenuItem("GameObject/RPG Toolkit Modules/Create Quest Module", false, 12)]
    static void CreateQuest()
    {
        CreateModuleWithUI(QuestPath, false);
    }

    static void CreateModule(string prefabPath)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError("Prefab not found at " + prefabPath);
            return;
        }

        GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        if (instance != null)
        {
            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);
            Selection.activeObject = instance;
        }
        else
        {
            Debug.LogError("Failed to instantiate prefab.");
        }
    }

    static void CreateModuleWithUI(string modulePath, bool needUIReference)
    {
        RPGToolkitModules.modulePath = modulePath;

        uiModule = GameObject.FindWithTag("RPGToolkitUI");
        if (uiModule == null)
        {
            Debug.Log("UI Module not found. Creating it...");

            GameObject uiPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(UIPath);
            if (uiPrefab == null)
            {
                Debug.LogError("UI Module prefab not found at " + UIPath);
                return;
            }

            uiModule = PrefabUtility.InstantiatePrefab(uiPrefab) as GameObject;
            if (uiModule != null)
            {
                Undo.RegisterCreatedObjectUndo(uiModule, "Create " + uiModule.name);
            }
            else
            {
                Debug.LogError("Failed to instantiate UI Module prefab.");
                return;
            }
        }

        if (needUIReference)
        {
            // Start waiting for the UIModule to become active
            EditorApplication.update += WaitForUIModule;
        }
        else
        {
            CreateModule(modulePath);
        }
    }

    static void WaitForUIModule()
    {
        Debug.Log("Waiting for UI Module...");

        // Wait until the UIModule is active
        if (uiModule.activeSelf)
        {
            Debug.Log("UI Module is active.");

            // Create the InventoryModule
            GameObject moduleInstance = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(modulePath)) as GameObject;
            if (moduleInstance != null)
            {
                Debug.Log("Inventory Module instantiated.");

                Undo.RegisterCreatedObjectUndo(moduleInstance, "Create " + moduleInstance.name);
                Selection.activeObject = moduleInstance;

                if (modulePath.Contains("Inventory"))
                {
                    Debug.Log("Needs UI References.");

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

                    Debug.Log("Found " + taggedObjects.Count + " InventorySlots.");

                    // Set the references in InventoryManager.cs of the InventoryPrefab
                    InventoryManager inventoryManager = moduleInstance.GetComponent<InventoryManager>();
                    if (inventoryManager != null)
                    {
                        // Assign the found slots to the InventoryManager
                        inventoryManager.invSlots = taggedObjects.ToArray();

                        // Save the changes to the InventoryManager prefab
                        PrefabUtility.SaveAsPrefabAsset(moduleInstance, modulePath);
                        Debug.Log("InventorySlots assigned to InventoryManager.");
                    }
                    else
                    {
                        Debug.LogWarning("InventoryManager component not found in InventoryPrefab.");
                    }
                }

                // Unsubscribe from the EditorApplication.update event to prevent further calls to WaitForUIModule
                EditorApplication.update -= WaitForUIModule;
            }
            else
            {
                Debug.LogError("Failed to instantiate module prefab.");
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
