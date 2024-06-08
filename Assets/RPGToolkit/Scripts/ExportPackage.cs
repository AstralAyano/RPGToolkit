using UnityEngine;
using System.Collections;
using UnityEditor;
 
public static class ExportPackage
{
    [MenuItem("Export/Export with Tags and Layers, Input settings")]
    public static void Export()
    {
        string[] projectContent = new string[] {"Assets", "ProjectSettings/TagManager.asset","ProjectSettings/InputManager.asset","ProjectSettings/ProjectSettings.asset"};
        AssetDatabase.ExportPackage(projectContent, "RPGToolkit.unitypackage",ExportPackageOptions.Interactive | ExportPackageOptions.Recurse |ExportPackageOptions.IncludeDependencies);
        Debug.Log("Project Exported");
    }
 
}