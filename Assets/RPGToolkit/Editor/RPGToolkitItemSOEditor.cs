using UnityEngine;
using UnityEditor;
using System.IO;

namespace RPGToolkit
{
    [CustomEditor(typeof(ItemInfoSO))]
    public class RPGToolkitItemSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ItemInfoSO itemInfo = (ItemInfoSO)target;

            if (GUILayout.Button("Create Prefab GameObject"))
            {
                CreatePrefabGameObject(itemInfo);
            }
        }

        private void CreatePrefabGameObject(ItemInfoSO itemInfo)
        {
            // Create a new GameObject
            GameObject newGameObject = new GameObject(itemInfo.itemName);

            // Add SpriteRenderer component and set the sprite
            SpriteRenderer spriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = itemInfo.image;
            spriteRenderer.sortingOrder = 1; // Set the order in layer

            // Add CircleCollider2D component, set as trigger, and set radius
            CircleCollider2D circleCollider = newGameObject.AddComponent<CircleCollider2D>();
            circleCollider.isTrigger = true;
            circleCollider.radius = 0.15f;

            // Set tag, layer, and order in layer
            newGameObject.tag = "Item";
            newGameObject.layer = LayerMask.NameToLayer("Item");

            // Define the local path
            string directoryPath = "Assets/Resources/RPGToolkit/Items/ItemPrefabs";
            string localPath = Path.Combine(directoryPath, itemInfo.itemName + ".prefab");

            // Check if the directory exists, if not, create it
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Save the new GameObject as a prefab
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(newGameObject, localPath);

            // Destroy the temporary GameObject
            GameObject.DestroyImmediate(newGameObject);

            // Select the prefab in the Editor
            Selection.activeObject = prefab;
        }
    }
}
