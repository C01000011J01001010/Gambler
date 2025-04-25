using PublicSet;
using UnityEngine;

public abstract class SaveAndContinue_ButtonBase : ButtonBase
{
    protected PopUpViewBase popUpView
    {
        get
        {
            switch (GameManager.Instance.currentScene)
            {
                case eScene.Lobby: return GameManager.connector_Lobby.popUpViewAsLobby;
                case eScene.InGame: return GameManager.connector_InGame.popUpViewAsInGame;
            }
            Debug.LogAssertion("�߸��� ����");
            return null;
        }
    }
    protected YesOrNoPopUp yesOrNoPopUp
    {
        get
        {
            return popUpView.yesOrNoPopUp;
        }
    }
    protected CheckPopUp checkPopUp
    {
        get
        {
            return popUpView.checkPopUp;
        }
    }
    protected ePlayerSaveKey saveKey = ePlayerSaveKey.None;

    protected virtual void Start()
    {
        SetButtonCallback(Callback);
    }

    public virtual void SetPlayerSaveKey(ePlayerSaveKey value)
    {
        saveKey = value;
    }

    public abstract void Callback();
}
