using PublicSet;
using UnityEngine;
using UnityEngine.Events;

public class QuestCheckCallback : CallbackBase, ICallback<eQuestType>
{
    public UnityAction CallbackList(eQuestType type)
    {
        switch (type)
        {
            case eQuestType.LetsLookAroundOutside: return LetsLookAroundOutside;
            case eQuestType.LetsOpenBoxForTheFirstTime: return LetsOpenBoxForTheFirstTime;
            case eQuestType.UseCasinoEntryTicket: return UseCasinoEntryTicket;
            case eQuestType.LearnHowToSave: return LearnHowToSave;
            case eQuestType.StartFirstGame: return StartFirstGame;
            case eQuestType.GoToSleep: return GoToSleep;
            case eQuestType.CheckTheBoxEveryDay: return CheckTheBoxEveryDay;
            case eQuestType.TryUseDarkWebMarket: return TryUseDarkWebMarket;

            case eQuestType.Collect2000Coins: return Collect2000Coins;
            case eQuestType.Collect5000Coins: return Collect5000Coins;
            case eQuestType.Collect8000Coins: return Collect8000Coins;
            case eQuestType.Collect10000Coins: return Collect10000Coins;

            default: { Debug.LogError("존재하지 않는 퀘스트"); return null; }
        }
    }

    public void LetsLookAroundOutside()
    {
        QuestManager.Instance.TryPlayerCompleteQuest(eQuestType.LetsLookAroundOutside);
    }
    public void LetsOpenBoxForTheFirstTime()
    {
        QuestManager.Instance.TryPlayerCompleteQuest(eQuestType.LetsOpenBoxForTheFirstTime);
        QuestManager.Instance.TryPlayerGetQuest(eQuestType.UseCasinoEntryTicket);
    }

    public void UseCasinoEntryTicket()
    {
        QuestManager.Instance.TryPlayerCompleteQuest(eQuestType.UseCasinoEntryTicket);
        QuestManager.Instance.TryPlayerGetQuest(eQuestType.LearnHowToSave);
        QuestManager.Instance.TryPlayerGetQuest(eQuestType.StartFirstGame);
    }
    public void LearnHowToSave()
    {
        QuestManager.Instance.TryPlayerCompleteQuest(eQuestType.LearnHowToSave);

    }
    public void StartFirstGame()
    {
        QuestManager.Instance.TryPlayerCompleteQuest(eQuestType.StartFirstGame);
    }
    public void GoToSleep()
    {
        QuestManager.Instance.TryPlayerCompleteQuest(eQuestType.GoToSleep);
        QuestManager.Instance.TryPlayerGetQuest(eQuestType.Collect2000Coins);
    }
    public void CheckTheBoxEveryDay()
    {
        Debug.LogWarning("수정중");
    }

    public void TryUseDarkWebMarket()
    {
        Debug.LogWarning("수정중");
    }



    public void Collect2000Coins()
    {
        if (playerStatus.coin >= 2000)
        {
            QuestManager.Instance.TryPlayerCompleteQuest(eQuestType.Collect2000Coins);
            QuestManager.Instance.TryPlayerGetQuest(eQuestType.Collect5000Coins);
        }
    }

    public void Collect5000Coins()
    {
        if (playerStatus.coin >= 5000)
        {
            QuestManager.Instance.TryPlayerCompleteQuest(eQuestType.Collect5000Coins);
            QuestManager.Instance.TryPlayerGetQuest(eQuestType.Collect8000Coins);
        }
    }

    public void Collect8000Coins()
    {
        if (playerStatus.coin >= 8000)
        {
            QuestManager.Instance.TryPlayerCompleteQuest(eQuestType.Collect8000Coins);
            QuestManager.Instance.TryPlayerGetQuest(eQuestType.Collect10000Coins);
        }
    }

    public void Collect10000Coins()
    {
        if (playerStatus.coin >= 10000)
        {
            QuestManager.Instance.TryPlayerCompleteQuest(eQuestType.Collect10000Coins);
        }
    }
}
