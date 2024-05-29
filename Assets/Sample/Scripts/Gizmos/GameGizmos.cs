using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public abstract class GameGizmos : ScriptableObject
{
    public abstract void Draw(GizmosRenderer r);
}
