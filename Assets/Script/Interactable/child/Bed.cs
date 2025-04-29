using UnityEngine;
using PublicSet;

public class Bed : InteractableObjectBase
{

    public override eTextScriptFile GetInteractableEnum()
    {
        return eTextScriptFile.Bed;
    }
}
