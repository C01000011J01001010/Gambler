using UnityEngine;

public class QuestButton : IconButtonBase
{
    private void Start()
    {
        if (popUpView == null)
            Debug.LogAssertion("PopUpView == null");

        SetButtonCallback(popUpView.QuestPopUpOpen);
    }
}
