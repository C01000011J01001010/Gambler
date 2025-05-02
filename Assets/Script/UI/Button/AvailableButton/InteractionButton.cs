using UnityEngine;

public class InteractionButton : Deactivatable_ButtonBase
{
    private void Start()
    {
        SetButtonCallback(StartInteraction);
    }
    public void StartInteraction()
    {
        GameManager.connector_InGame.Canvas1.TextWindowView.StartTextWindow();
    }
}
