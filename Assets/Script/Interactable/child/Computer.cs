using UnityEngine;
using PublicSet;

public class Computer : InteractableObjectBase
{
    public override eTextScriptFile GetInteractableEnum()
    {
        return eTextScriptFile.Computer;
    }
}
