using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosRenderer : MonoBehaviour
{
    public GameGizmos gizmo;

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, 10);
        gizmo.Draw(this);
    }
}