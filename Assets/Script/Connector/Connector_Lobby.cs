using UnityEngine;

public class Connector_Lobby : Connector
{
    public PopUPView_Lobby popUpViewAsLobby { get { return popUpView as PopUPView_Lobby; } }

    protected override void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.InitConnector(this);
    }
}
