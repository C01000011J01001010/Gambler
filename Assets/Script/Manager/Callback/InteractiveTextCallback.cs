using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveTextCallback : CallbackBase, ICallback<int>
{

    // csv���� �ε��������� �Լ��� ������ ���ֵ��� ����
    public UnityAction CallbackList(int index)
    {
        // csv���������� �����Ӱ� �ݹ��Լ��� �� �� ����
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
        // �ϸ� �߿� ����� ó���� �����Լ��� ����
        PlaySequnce_BlackViewProcess(delay,
            () => GameManager.connector_InGame.map_Script.ChangeMapTo(eMap.OutsideOfHouse),
            () => GameManager.connector_InGame.interfaceView.SetActive(true)
        );

    }

    // 3
    public void ChangeMapToInsideOfHouse()
    {
        float delay = 2.0f;
        // �ϸ� �߿� ����� ó���� �����Լ��� ����
        PlaySequnce_BlackViewProcess(delay,
            () => GameManager.connector_InGame.map_Script.ChangeMapTo(eMap.InsideOfHouse),
            () => GameManager.connector_InGame.interfaceView.SetActive(true)
        );

    }

    // 4
    public void GoToNextDay()
    {
        float delay = 4.0f;

        PlaySequnce_BlackViewProcess(delay,
                () =>
                {
                    GameManager.Instance.CountDownRemainingPeriod();
                    GameManager.connector_InGame.canvas0_InGame.casinoView.onlyOneLivesButton.InitButtonCallback();
                },
                () =>
                {
                    PlayManager.Instance.StartPlayerMonologue_OnPlayerWakeUp();

                    // ����Ʈ�� ������ ���
                    sQuest quest = new sQuest(0, eQuestType.GoToSleep);
                    if (QuestManager.questHashSet.Contains(quest))
                    {
                        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);
                        if (questInfo.isComplete == false) questInfo.endConditionCheck();
                    }
                }
            );

    }



    // 5
    public virtual void BoxOpen()
    {
        // falseŰ�� "Interactable_Box_Empty"����Ǿ�����
        GameManager.connector_InGame.box_Script.EmptyOutBox();
        TextWindowPopUp_Close();

        // ����Ʈ�� ������ ���
        sQuest quest = new sQuest(0, eQuestType.LetsOpenBoxForTheFirstTime);
        if (QuestManager.questHashSet.Contains(quest))
        {
            // ù ������ ȹ���̴� �������� �ع�
            GameManager.connector_InGame.iconView_Script.GetComponent<IconView>().TryIconUnLock(eIcon.Inventory);

            // ����Ʈ�� �Ϸ����� Ȯ���Ͽ� ����Ʈ �Ϸ�
            cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);
            if (!questInfo.isComplete) questInfo.endConditionCheck();
        }

    }

    //6
    public virtual void TextHoldOn()
    {
        TextWindowView textWindowView_Script = GameManager.connector_InGame.textWindowView.GetComponent<TextWindowView>();
        textWindowView_Script.PrintText();
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
        Debug.Log("�߰� �ʿ�");
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
        // �ϸ� �߿� ����� ó���� �����Լ��� ����
        PlaySequnce_BlackViewProcess(delay,
            () =>
            {
                GameManager.connector_InGame.map_Script.ChangeMapTo(eMap.Casino);
                GameManager.connector_InGame.interfaceView.SetActive(true);
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
        GameManager.connector_InGame.textWindowView.GetComponent<TextWindowView>().TextIndexInit(0);
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
        GameManager.connector_InGame.iconView_Script.TryIconUnLock(eIcon.Quest);
    }

    // 15
    public void GetQuest_BoxOpen()
    {
        bool result = QuestManager.Instance.TryPlayerGetQuest(eQuestType.LetsOpenBoxForTheFirstTime);
        if (result)
        {
            // �������� ����Ʈ�� ������ ��� 1���� ȹ����
            List<eItemType> list = new List<eItemType>();
            list.Add(eItemType.CasinoEntryTicket);
            list.Add(eItemType.Notice_Stage1);
            GameManager.connector_InGame.box_Script.FillUpBox(list);
        }
    }
}
