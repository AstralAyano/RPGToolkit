using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

namespace RPGToolkit
{
    public class RPGToolkitInteractSettings : MonoBehaviour
    {
        [Header("Resources")]
        public RPGToolkitManager RPGToolkitAsset;
        public InputActionAsset inputActionAsset;

        private void Awake()
        {
            // Change key binds for specific actions
            ChangeKeybind("RPGToolkitControls", "Interact", RPGToolkitAsset.interactKey);
            ChangeKeybind("RPGToolkitControls", "Quest", RPGToolkitAsset.questBookKey);

            SaveChanges();
        }

        private void ChangeKeybind(string actionMapName, string actionName, KeyCode newKey)
        {
            // Convert KeyCode to InputSystem binding string
            string newPath = $"<Keyboard>/{newKey.ToString().ToLower()}";

            // Find the ActionMap that contains the action
            InputActionMap actionMap = inputActionAsset.FindActionMap(actionMapName);

            if (actionMap != null)
            {
                // Find the action within the ActionMap
                InputAction action = actionMap.FindAction(actionName);

                if (action != null)
                {
                    // Apply the new binding
                    InputBinding inputBinding = action.bindings[0];
                    Debug.Log("Before changing to " + newPath + " : " + inputBinding.path);
                    action.ApplyBindingOverride(0, new InputBinding { overridePath = newPath });
                    Debug.Log("After : " + inputBinding.path);
                    
                    //Debug.Log($"Keybind for {actionName} changed to {newPath}");
                }
                else
                {
                    Debug.LogWarning($"Action '{actionName}' not found in the ActionMap '{actionMapName}'.");
                }
            }
            else
            {
                Debug.LogWarning($"ActionMap '{actionMapName}' not found in the InputActionAsset.");
            }

            // Debug statements to verify
            Debug.Log($"ActionMap: {actionMapName}, Action: {actionName}, New Binding: {newPath}");
        }

        private void SaveChanges()
        {
            // Save assets using AssetDatabase
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(); // Optional: Refreshes the AssetDatabase to ensure changes are visible
            Debug.Log("Changes saved to assets.");
        }
    }
}