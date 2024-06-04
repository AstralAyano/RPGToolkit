using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGToolkit
{
    public class BlockRange : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D other)
        {
            transform.parent.GetComponent<PlayerController>().BlockRangeDetected(other);
        }
    }
}