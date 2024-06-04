using UnityEngine;

namespace RPGToolkit
{
    public class MeleeRange : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D other)
        {
            transform.parent.GetComponent<PlayerController>().MeleeRangeDetected(this, other);
        }
    }
}