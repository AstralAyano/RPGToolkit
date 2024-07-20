using UnityEngine;
using UnityEditor;

public static class ExportPackage
{
    [MenuItem("File/Export Assets and Settings")]
    public static void Export()
    {
        // Define the asset folders to include in the package
        string[] assetFolders = new string[]
        {
            "Assets/RPGToolkit/Animations",
            "Assets/RPGToolkit/Editor",
            "Assets/RPGToolkit/Fonts",
            "Assets/RPGToolkit/Items",
            "Assets/RPGToolkit/Materials",
            "Assets/RPGToolkit/PostProcessing",
            "Assets/RPGToolkit/Prefabs",
            "Assets/RPGToolkit/Resources",
            "Assets/RPGToolkit/Sample",
            "Assets/RPGToolkit/Scripts",
            "Assets/RPGToolkit/Sprites",
            "Assets/RPGToolkit/Textures",
            "Assets/RPGToolkit/Tilemaps",
            "Assets/RPGToolkit/RPGToolkitManager.asset"
        };

        // Define the project settings files to include in the package
        string[] projectSettings = new string[]
        {
            "ProjectSettings/TagManager.asset",
            "ProjectSettings/InputManager.asset",
            "ProjectSettings/ProjectSettings.asset",
            "ProjectSettings/EditorBuildSettings.asset", // Include Editor Build Settings
            "ProjectSettings/DynamicsManager.asset", // Include Physics settings
            "ProjectSettings/GraphicsSettings.asset", // Include Graphics settings
            "ProjectSettings/QualitySettings.asset" // Include Quality settings
        };

        // Combine asset folders and project settings into a single array
        string[] exportItems = new string[assetFolders.Length + projectSettings.Length];
        assetFolders.CopyTo(exportItems, 0);
        projectSettings.CopyTo(exportItems, assetFolders.Length);

        // Export the package with selected assets and settings
        AssetDatabase.ExportPackage(
            exportItems,
            "RPGToolkit.unitypackage",
            ExportPackageOptions.Interactive |
            ExportPackageOptions.Recurse
        );

        Debug.Log("Assets and settings exported");
    }
}