using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeRange : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        transform.parent.GetComponent<PlayerController>().MeleeRangeDetected(this, other);
    }
}
