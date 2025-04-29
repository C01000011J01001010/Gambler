using UnityEngine;
using UnityEngine.Events;

public class OnlyOneLivesCallback : CallbackBase, ICallback<int>
{
    public UnityAction CallbackList(int index)
    {
        switch (index)
        {
            case 0: return NextProgress;
            case 1: return GameStartButtonOn;
            case 10: return AttackPrgress;
            case 11: return DeffenceProgress;
            case 20: return CardOpen;
            case 21: return OnJokerWin;
            case 22: return OnAttackerWin;
            case 23: return OnDeffenderWin;
            case 24: return OnHuntingTime;
            case 25: return OnPlayerBackrupt;
            case 30: return NextGame;
            case 31: return EndGame;

            default: return TrashFuc;
        }
    }

    public void NextProgress()
    {
        CardGamePlayManager.Instance.NextProgress();
    }

    public void GameStartButtonOn()
    {
        CardGamePlayManager.Instance.cardGameView.PlaySequnce_StartButtonFadeIn();
    }

    public void AttackPrgress()
    {
        CardGamePlayManager.Instance.StartPlayerAttack();
    }

    public void DeffenceProgress()
    {
        if (CardGamePlayManager.Instance.Deffender.closedCardList.Count <= 0)
        {
            CardGamePlayManager.Instance.NextProgress();
            return;
        }
        else
        {
            CardGamePlayManager.Instance.StartPlayerDeffence();
            return;
        }

    }

    public void CardOpen()
    {
        CardGamePlayManager.Instance.CardOpenAtTheSameTime();
    }

    public void OnJokerWin()
    {
        CardGamePlayManager.Instance.OnJokerAppear();
    }
    public void OnAttackerWin()
    {
        CardGamePlayManager.Instance.OnAttackSuccess();
    }

    public void OnDeffenderWin()
    {
        CardGamePlayManager.Instance.OnDefenceSuccess();
    }

    public void OnHuntingTime()
    {
        CardGamePlayManager.Instance.OnHuntPrey();
    }

    public void OnPlayerBackrupt()
    {
        CardGamePlayManager.Instance.OnPlayerBankrupt();
    }

    private void NextGame()
    {
        CardGamePlayManager.Instance.InitCurrentGame();
        CardGamePlayManager.Instance.cardGameView.PlaySequnce_StartButtonFadeIn();
    }

    private void EndGame()
    {
        CasinoViewOpen();
    }
}
