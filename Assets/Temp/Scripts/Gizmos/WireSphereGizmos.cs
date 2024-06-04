using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WireSphereGizmos : GameGizmos
{
    [SerializeField] private float radius = 1.0f;

    public override void Draw(GizmosRenderer r)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(r.gameObject.transform.position, radius);
    }
}