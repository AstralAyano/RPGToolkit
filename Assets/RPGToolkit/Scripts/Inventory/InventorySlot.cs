using UnityEngine;
using UnityEngine.EventSystems;

namespace RPGToolkit
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        [HideInInspector] public Transform parentAfterDrag;

        public void OnDrop(PointerEventData eventData)
        {
            if (transform.childCount == 0)
            {
                InventoryItem invItem = eventData.pointerDrag.GetComponent<InventoryItem>();
                invItem.parentAfterDrag = transform;
            }
        }
    }
}