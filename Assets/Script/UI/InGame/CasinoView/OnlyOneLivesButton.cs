using UnityEngine;
using PublicSet;

public class OnlyOneLivesButton : GameEnterButtonBase
{
    public CardGameView CardGameView;
    public CardGamePlayManager cardGamePlayManager;
    public Bed bed;

    public override void EnterGame()
    {
        if(CardGameView == null)
        {
            Debug.LogAssertion("CardGameView == null");
            return;
        }
        if(cardGamePlayManager == null)
        {
            Debug.LogAssertion("PlayManager == null");
            return;
        }

        CallbackManager.Instance.PlaySequnce_BlackViewProcess(2.0f,
            () =>
            {
                GameManager.connector_InGame.canvas0_InGame.CloseAllOfView();
                CardGameView.gameObject.SetActive(true);
            }
            );
    }

    public override void SetPlayerCantPlayThis()
    {
        SetButtonCallback(
            () =>
            {
                GameManager.connector_InGame.textWindowView_Script.StartTextWindow(eTextScriptFile.PlayerCantPlayThis);

                QuestManager.Instance.PlayerGetQuest(eQuestType.GoToSleep);
            }

            );
    }

}
