using UnityEngine;

public class IventoryButton : IconButtonBase
{
    private void Start()
    {
        if (popUpView == null)
            Debug.LogAssertion("PopUpView == null");

        SetButtonCallback(popUpView.InventoryPopUpOpen);
    }
}
