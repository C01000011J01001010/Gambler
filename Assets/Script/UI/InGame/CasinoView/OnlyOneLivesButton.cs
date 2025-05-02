using UnityEngine;
using PublicSet;

public class OnlyOneLivesButton : GameEnterButtonBase
{
    public CardGameView CardGameView;
    public CardGamePlayManager cardGamePlayManager => CardGamePlayManager.Instance;
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

        CallbackBase.PlaySequnce_BlackViewProcess(2.0f,
            () =>
            {
                GameManager.connector_InGame.Canvas0.CloseAllOfView();
                CardGameView.gameObject.SetActive(true);
                cardGamePlayManager.EnterCardGame();
            }
            );
    }

    public override void SetPlayerCantPlayThis()
    {
        SetButtonCallback(
            () =>
            {
                GameManager.connector_InGame.Canvas1.TextWindowView.StartTextWindow(eTextScriptFile.PlayerCantPlayThis);

                QuestManager.Instance.TryPlayerGetQuest(eQuestType.GoToSleep);
            }

            );
    }

}
