using UnityEngine;

public class Object_2D : MonoBehaviour
{
    public DynamicInteractableBase[] dynamicArr;

    private void Awake()
    {
        foreach (var dynamic in dynamicArr)
        {
            dynamic.InitDict();
        }
    }
}
