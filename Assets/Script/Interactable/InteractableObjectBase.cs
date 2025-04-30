using UnityEngine;
using PublicSet;

public abstract class InteractableObjectBase : MonoBehaviour
{
    public const int InteractableLayer = 3;
    public abstract eTextScriptFile GetInteractableEnum();
}
