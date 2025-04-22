using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using System;
using PublicSet;
using System.Collections.Generic;


public class CallbackManager : Singleton<CallbackManager>
{

    // 스크립트로 편집
    private eStage currentStage { get { return GameManager.Instance.currentStage; } }
    private Dictionary<eStage, string> stageMessageDict { get { return GameManager.Instance.stageMessageDict; } }
    private Connector_InGame connector_InGame { get { return GameManager.connector_InGame; } }
    private sPlayerStatus playerStatus { get { return PlayManager.Instance.currentPlayerStatus; } }
    
    Image blackViewImage;
    //private bool isBlakcViewReady;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        //isBlakcViewReady = true;
    }

    public void PlaySequnce_BlackViewProcess(float delay, Action middleCallBack, Action endCallback = null)
    {
        //isBlakcViewReady = false;

        // 대화창 끄고 일시정지
        GameManager.connector_InGame.textWindowView.SetActive(false);
        GameManager.Instance.Pause_theGame();

        // 먼저 화면가림막 활성화
        GameManager.connector.blackView.SetActive(true);

        // 화면이 검게 변했다가 다시 원상복귀됨
        if (blackViewImage == null)
        {
            blackViewImage = GameManager.connector.blackView.GetComponent<Image>();
        }

        // 시퀀스 생성
        Sequence sequence = DOTween.Sequence();

        // 시퀀스 설정
        sequence.AppendCallback(() => blackViewImage.color = Color.clear);
        sequence.Append(blackViewImage.DOColor(Color.black, delay / 2));

        if(middleCallBack != null)
        {
            sequence.AppendCallback(() => middleCallBack());
        }

        sequence.Append(blackViewImage.DOColor(Color.clear, delay / 2));

        sequence.AppendCallback(
        () =>
        {
            // 비활성화를 해야 화면 클릭이 가능함
            GameManager.connector.blackView.SetActive(false);

            // 게임 지속
            GameManager.Instance.Continue_theGame();

            //isBlakcViewReady = true;
        }
        );

        if (endCallback != null)
        {
            sequence.AppendCallback(() => endCallback());
        }


        sequence.SetLoops(1);

        // 시퀀스 플레이(시퀀스 플레이는 생성후 최초 1회만 플레이 가능함)
        sequence.Play();
    }

    public void TrashFuc()
    {
        Debug.LogAssertion("정의되지 않은 콜백함수");
    }

    // csv에서 인덱스만으로 함수를 선택할 수있도록 만듬
    public UnityAction CallBackList_DefaultText(int index)
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
    

    // 0
    public virtual void TextWindowPopUp_Open()
    {
        GameManager.connector_InGame.textWindowView.SetActive(true);
        GameManager.connector_InGame.interfaceView.SetActive(false);
    }

    // 1
    public virtual void TextWindowPopUp_Close()
    {
        GameManager.connector_InGame.textWindowView.SetActive(false);

        // 카지노 게임뷰가 아닌 경우에만 인터페이스를 활성화
        if(GameManager.Instance.isCasinoGameView == false)
        {
            GameManager.connector_InGame.interfaceView.SetActive(true);
        }
        
    }

    // 2
    public void ChangeMapToOutsideOfHouse()
    {
        float delay = 2.0f;
        // 암막 중에 실행될 처리를 람다함수로 전달
        PlaySequnce_BlackViewProcess(delay, 
            () => GameManager.connector_InGame.map_Script.ChangeMapTo(eMap.OutsideOfHouse), 
            () => GameManager.connector_InGame.interfaceView.SetActive(true)
        );
        
    }

    // 3
    public void ChangeMapToInsideOfHouse()
    {
        float delay = 2.0f;
        // 암막 중에 실행될 처리를 람다함수로 전달
        PlaySequnce_BlackViewProcess(delay,
            () =>GameManager.connector_InGame.map_Script.ChangeMapTo(eMap.InsideOfHouse),
            () =>GameManager.connector_InGame.interfaceView.SetActive(true)
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
                ()=>
                {
                    PlayManager.Instance.StartPlayerMonologue_OnPlayerWakeUp();

                    // 퀘스트를 수주한 경우
                    sQuest quest = new sQuest(0, eQuestType.GoToSleep);
                    if(QuestManager.questHashSet.Contains(quest))
                    {
                        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);
                        if (questInfo.isComplete == false) questInfo.callback_endConditionCheck();
                    }
                }
            );

    }

    

    // 5
    public virtual void BoxOpen()
    {
        // false키에 "Interactable_Box_Empty"저장되어있음
        GameManager.connector_InGame.box_Script.EmptyOutBox();
        TextWindowPopUp_Close();

        // 퀘스트를 수주한 경우
        sQuest quest = new sQuest(0, eQuestType.LetsOpenBoxForTheFirstTime);
        if (QuestManager.questHashSet.Contains(quest))
        {
            // 첫 아이템 획득이니 아이콘을 해방
            GameManager.connector_InGame.iconView_Script.GetComponent<IconView>().TryIconUnLock(eIcon.Inventory);

            // 퀘스트의 완료조건 확인하여 퀘스트 완료
            cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);
            if (!questInfo.isComplete) questInfo.callback_endConditionCheck();
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
        GameManager.connector_InGame.popUpView_Script.SaveDataPopUpOpen();
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
        //Debug.Log("카지노 입장");
        float delay = 2.0f;
        PlaySequnce_BlackViewProcess(delay,
            ()=>
            {
                GameManager.connector_InGame.canvas0_InGame.CasinoViewOpen();
            },
            ()=>  
            {
                GameManager.connector_InGame.canvas0_InGame.casinoView.GetComponent<CasinoView>().StartDealerDialogue();
            }
            );
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
        if(result)
        {
            // 아이템은 퀘스트를 수주한 경우 1번만 획득함
            List<eItemType> list = new List<eItemType>();
            list.Add(eItemType.CasinoEntryTicket);
            list.Add(eItemType.Notice_Stage1);
            GameManager.connector_InGame.box_Script.FillUpBox(list);
        }
    }





    public UnityAction CallbackList_OnlyOneLivesText(int index)
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
        if(CardGamePlayManager.Instance.Deffender.closedCardList.Count <= 0)
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
        EnterCasino();
    }




    /// <summary>
    /// 콜백함수를 반환하는 함수
    /// </summary>
    /// <param name="index">값으로 콜백함수를 선택함</param>
    /// <returns> 버튼에 연결할 콜백함수 </returns>
    public UnityAction CallBackList_Item(eItemCallback index)
    {
        switch (index)
        {
            case eItemCallback.CasinoTicket : return CasinoTicket;
            case eItemCallback.EatMeal: return EatMeal;

            default: return TrashFuc;
        }
    }

    
    public void CasinoTicket()
    {
        GameManager.Instance.NextStage();
        EventManager.Instance.SetEventMessage(stageMessageDict[currentStage]);
        EventManager.Instance.PlaySequnce_EventAnimation();

        // 퀘스트를 수주한 경우
        sQuest quest = new sQuest(0, eQuestType.UseCasinoEntryTicket);
        if (QuestManager.questHashSet.Contains(quest))
        {
            cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);
            if (questInfo.isComplete == false) questInfo.callback_endConditionCheck();
        }
            
    }

    
    public void EatMeal()
    {
        InventoryPopUp inven = connector_InGame.popUpView_Script.inventoryPopUp.GetComponent<InventoryPopUp>();
        if (inven != null)
        {
            cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(inven.currentClickItem.type);
            Debug.Log($"허기를 {itemInfo.value_Use.ToString()}만큼 회복했습니다.");
        }
        else
        {
            Debug.LogAssertion("InventoryPopUp이 없음");
        }
    }

    public UnityAction CallBackList_QuestCheck(eQuestType type)
    {
        switch (type)
        {
            case eQuestType.LetsLookAroundOutside: return LetsLookAroundOutside;
            case eQuestType.LetsOpenBoxForTheFirstTime: return LetsOpenBoxForTheFirstTime;
            case eQuestType.UseCasinoEntryTicket: return UseCasinoEntryTicket;
            case eQuestType.LearnHowToSave: return LearnHowToSave;
            case eQuestType.StartFirstGame: return StartFirstGame;
            case eQuestType.GoToSleep: return GoToSleep;
            case eQuestType.Collect2000Coins: return Collect2000Coins;
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
    public void Collect2000Coins()
    {
        if(playerStatus.coin >= 2000)
        {
            QuestManager.Instance.TryPlayerCompleteQuest(eQuestType.Collect2000Coins);
        }
    }
}
