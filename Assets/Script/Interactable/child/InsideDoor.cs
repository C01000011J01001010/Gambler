using UnityEngine;
using PublicSet;

public class InsideDoor : InteractableObjectBase
{
    public override eTextScriptFile GetInteractableEnum()
    {
        return eTextScriptFile.InsideDoor;
    }
}
