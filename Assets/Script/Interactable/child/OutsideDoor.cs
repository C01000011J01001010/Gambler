using UnityEngine;
using PublicSet;

public class OutsideDoor : InteractableObjectBase
{
    public override eTextScriptFile GetInteractableEnum()
    {
        return eTextScriptFile.OutsideDoor;
    }
}
