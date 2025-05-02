using PublicSet;
using UnityEngine;

public abstract class GameEnterButtonBase : ButtonBase
{
    private void Start()
    {
        InitButtonCallback();
    }
    public void InitButtonCallback()
    {
        SetButtonCallback(EnterGame);
    }
    public abstract void EnterGame();

    public virtual void SetPlayerCantPlayThis()
    {
        SetButtonCallback(
            () => GameManager.connector_InGame.Canvas1.TextWindowView.StartTextWindow(eTextScriptFile.PlayerCantPlayThis)
            );
    }
}
