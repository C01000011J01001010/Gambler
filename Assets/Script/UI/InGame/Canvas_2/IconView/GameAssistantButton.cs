using UnityEngine;

public class GameAssistantButton : IconButtonBase
{
    private void Start()
    {
        if (popUpView == null)
            Debug.LogAssertion("PopUpView == null");

        SetButtonCallback(popUpView.GameAssistantPopUpOpen_OnlyOneLives);
    }


}
