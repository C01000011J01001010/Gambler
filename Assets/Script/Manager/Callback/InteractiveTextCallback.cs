using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveTextCallback : CallbackBase, ICallback<int>
{

    // csv에서 인덱스만으로 함수를 선택할 수있도록 만듬
    public UnityAction CallbackList(int index)
    {
        // csv선택지에서 자유롭게 콜백함수를 고를 수 있음
        switch (index)
        {
            case 0: return TextWindowPopUp_Open;
            case 1: return TextWindowPopUp_Close;
            case 2: return ChangeMapToOutsideOfHouse;
            case 3: return ChangeMapToInsideOfHouse;
            case 4: return GoToNextDay;
            case 5: return BoxOpen;
            case 6: return TextHoldOn;
            case 7: return SavePlayerData;
            case 8: return StartComputer;
            case 9: return PlayerGetGamblingCoin;
            case 10: return GotoCasinoPlace;
            case 11: return GotoUnknownIsland;
            case 12: return TellmeOneMoreTime;
            case 13: return EnterCasino;
            case 14: return TutorialStart;
            case 15: return GetQuest_BoxOpen;
        }

        return TrashFuc;
    }


    // 2
    public void ChangeMapToOutsideOfHouse()
    {
        float delay = 2.0f;
        // 암막 중에 실행될 처리를 람다함수로 전달
        PlaySequnce_BlackViewProcess(delay,
            () => GameManager.connector_InGame.Map.ChangeMapTo(eMap.OutsideOfHouse),
            () => GameManager.connector_InGame.Canvas0.InterfaceView.SetActive(true)
        );

    }

    // 3
    public void ChangeMapToInsideOfHouse()
    {
        float delay = 2.0f;
        // 암막 중에 실행될 처리를 람다함수로 전달
        PlaySequnce_BlackViewProcess(delay,
            () => GameManager.connector_InGame.Map.ChangeMapTo(eMap.InsideOfHouse),
            () => GameManager.connector_InGame.Canvas0.InterfaceView.SetActive(true)
        );

    }

    // 4
    public void GoToNextDay()
    {
        float delay = 2.0f;
        bool isGameOver = false;

        PlaySequnce_BlackViewProcess(delay,
                () =>
                {
                    GameManager.Instance.CountDownRemainingPeriod(out isGameOver);
                    GameManager.connector_InGame.Canvas0.CasinoView.onlyOneLivesButton.InitButtonCallback();
                },
                () =>
                {
                    // 게임 오버되지 않았으면 게임을 속행
                    if (!isGameOver)
                    {
                        PlayManager.Instance.StartPlayerMonologue_OnPlayerWakeUp();

                        // 퀘스트를 수주한 경우
                        eQuestType questType = eQuestType.LetsLookAroundOutside;
                        if (QuestManager.Instance.PlayerQuestDict.ContainsKey(questType))
                        {
                            cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(questType);
                            if (questInfo.isComplete == false) questInfo.checkEndCondition();
                        }
                    }
                }
            );

    }



    // 5
    public virtual void BoxOpen()
    {
        // false키에 "Interactable_Box_Empty"저장되어있음
        GameManager.connector_InGame.Box.EmptyOutBox();
        TextWindowPopUp_Close();

        // 퀘스트를 수주한 경우
        eQuestType questType = eQuestType.LetsOpenBoxForTheFirstTime;
        if (QuestManager.Instance.PlayerQuestDict.ContainsKey(questType))
        {
            // 첫 아이템 획득이면 아이콘을 개방
            GameManager.connector_InGame.Canvas1.IconView.GetComponent<IconView>().TryIconUnLock(eIcon.Inventory);

            // 퀘스트의 완료조건 확인하여 퀘스트 완료
            cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(questType);
            if (!questInfo.isComplete) questInfo.checkEndCondition();
        }

    }

    //6
    public virtual void TextHoldOn()
    {
        GameManager.connector_InGame.Canvas1.TextWindowView.PrintText();
        return;
    }

    // 7
    public virtual void SavePlayerData()
    {
        TextWindowPopUp_Close();
        GameManager.connector_InGame.popUpViewAsInGame.SaveDataPopUpOpen();
    }

    // 8
    public virtual void StartComputer()
    {
        Debug.Log("추가 필요");
    }

    // 9
    public virtual void PlayerGetGamblingCoin()
    {
        PlayManager.Instance.AddPlayerMoney(100);
        TextHoldOn();
    }

    // 10
    public virtual void GotoCasinoPlace()
    {
        float delay = 2.0f;
        // 암막 중에 실행될 처리를 람다함수로 전달
        PlaySequnce_BlackViewProcess(delay,
            () =>
            {
                GameManager.connector_InGame.Map.ChangeMapTo(eMap.Casino);
                GameManager.connector_InGame.Canvas0.InterfaceView.SetActive(true);
            }
        );
    }

    // 11
    public virtual void GotoUnknownIsland()
    {

    }

    // 12
    public virtual void TellmeOneMoreTime()
    {
        GameManager.connector_InGame.Canvas1.TextWindowView.gameObject.GetComponent<TextWindowView>().TextIndexInit(0);
        TextHoldOn();
    }

    // 13
    public virtual void EnterCasino()
    {
        CasinoViewOpen();
    }

    // 14
    public void TutorialStart()
    {
        EventManager.Instance.SetEventMessage(stageMessageDict[currentStage]);
        EventManager.Instance.PlaySequnce_EventAnimation();
        QuestManager.Instance.TryPlayerGetQuest(eQuestType.LetsLookAroundOutside);
        GameManager.connector_InGame.Canvas1.IconView.TryIconUnLock(eIcon.Quest);
    }

    // 15
    public void GetQuest_BoxOpen()
    {
        bool result = QuestManager.Instance.TryPlayerGetQuest(eQuestType.LetsOpenBoxForTheFirstTime);
        if (result)
        {
            // 아이템은 퀘스트를 수주한 경우 1번만 획득함
            List<eItemType> list = new List<eItemType>();
            list.Add(eItemType.CasinoEntryTicket);
            list.Add(eItemType.Notice_Stage1);
            GameManager.connector_InGame.Box.FillUpBox(list);
        }
    }
}
