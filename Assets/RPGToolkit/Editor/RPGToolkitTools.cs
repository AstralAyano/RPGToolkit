using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace RPGToolkit
{
    public class RPGToolkitTools : Editor
    {
        #if UNITY_EDITOR
        private static void MakeSceneDirty(GameObject source, string sourceName)
        {
            if (Application.isPlaying == false)
            {
                Undo.RegisterCreatedObjectUndo(source, sourceName);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
        #endif
    }
}
