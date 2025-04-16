using PublicSet;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RuleButton : Selection_ButtonBase<RuleButton>
{
    private void Start()
    {
        SetButtonCallback(()=>TrySelectThisButton(this));
    }
}
