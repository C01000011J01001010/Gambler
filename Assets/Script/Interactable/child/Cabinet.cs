using UnityEngine;
using PublicSet;
public class Cabinet : InteractableObjectBase
{
    public override eTextScriptFile GetInteractableEnum()
    {
        return eTextScriptFile.Cabinet;
    }
}
