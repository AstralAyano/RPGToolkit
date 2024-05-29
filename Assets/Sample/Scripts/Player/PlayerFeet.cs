using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        transform.parent.GetComponent<PlayerController>().OnGround(other);
    }
}
