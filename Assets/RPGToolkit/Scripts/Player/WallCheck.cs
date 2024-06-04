using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGToolkit
{
    public class WallCheck : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D other)
        {
            transform.parent.GetComponent<PlayerController>().WallStay(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            transform.parent.GetComponent<PlayerController>().WallExit(other);
        }
    }
}