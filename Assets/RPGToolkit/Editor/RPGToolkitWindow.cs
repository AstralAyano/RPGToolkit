using UnityEngine;
using UnityEditor;

namespace RPGToolkit
{
    public class RPGToolkitWindow : EditorWindow
    {
        [MenuItem("RPG Toolkit/Manager Window", false, 0)]
        public static void ShowMyEditor()
        {
            EditorWindow window = GetWindow<RPGToolkitWindow>();
            window.titleContent = new GUIContent("RPG Toolkit Manager");

            window.minSize = new Vector2(450, 200);
            window.maxSize = new Vector2(1920, 720);
        }

        public SerializedObject gui = null;
        public void Init(SerializedObject item)
        {
            // Copy the Item targetObject to not lose reference when you
            // click another element on the project window.
            SerializedObject itemcopy = new SerializedObject(item.targetObject);
            gui = itemcopy;
        }

        public void CreateGUI()
        {

        }
    }
}
