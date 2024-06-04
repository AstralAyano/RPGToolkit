using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public string skillName;
    [TextArea] public string skillDesc;

    private void OnMouseEnter()
    {
        ToolTipManager.instance.SetAndShowToolTip(skillName, skillDesc);
    }

    private void OnMouseExit()
    {
        ToolTipManager.instance.HideToolTip();
    }
}
