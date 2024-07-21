using UnityEngine;

namespace RPGToolkit
{
    [CreateAssetMenu(fileName = "ItemInfoSO", menuName = "RPGToolkit/New Item Info", order = 12)]
    public class ItemInfoSO : ScriptableObject
    {
        public Sprite image;
        public bool stackable = true;

        public string itemName;

        [TextArea]
        public string itemDesc;
    }
}