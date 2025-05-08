using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace PublicSet
{
    //ENUM

    public enum ePlayerSaveKey
    {
        None,
        Player_1,
        Player_2,
        Player_3,
        Player_4,
        Player_5,
        Player_6,
        Player_7,
        Player_8,
        Player_9,
    }

    public enum eScene
    {
        Title,
        Lobby,
        InGame
    }

    public enum eMap
    {
        InsideOfHouse,
        OutsideOfHouse,
        Casino,
        UnknownIsland
    }

    public enum eStage
    {
        Defualt, // 스테이지 상관없이 실행
        Stage1,
        Stage2,
        Stage3,
    }

    public enum eTextType
    {
        TextScriptFile,
        OnlyOneLivesProgress,
        SystemGuide
    }

    public enum eTextScriptFile
    {
        None,
        // --------------Interactable----------------
        // 실내
        Bed,
        Cabinet,
        Clock,
        Computer,
        InsideDoor,

        // 야외
        Box_Empty,
        Box_Full,
        OutsideDoor,
        NPC_MunDuckBea_Acquaintance,
        NPC_MunDuckBea_Encounter,

        // 카지노
        NPC_MunDuckBea_InCasino,
        NPC_Caesar,
        CasinoDoor,

        // -----------None Interactable--------------
        // 플레이어 혼잣말
        PlayerMonologue,
        OnPlayerWakeUp,

        // 카지노
        //GameMaster,
        PlayerCantPlayThis
    }

    /// <summary>
    /// == onlyOneLivesProgress
    /// </summary>
    public enum eOOLProgress
    {
        num101_BeforeStartGame = 101, // 게임에 대한 기본적인 설명을 하는 단계
        num102_BeforeRotateDiceAndDistribution, // 주사위를 던지고 카드를 분배하는 단계
        num103_BeforeChooseCardsToReveal, // 각 플레이어가 공개할 카드를 선택하는 단계
        num104_OnChooseFirstPlayer, // 게임에서 첫 공격을 실행할 플레이어를 선택하는 단계

        // 공격차례, computer의 경우 AttackTurn_Player를 스킵
        num201_AttackTurnPlayer = 201,
        num202_AttackDone,
        num203_PlayerCantAttack,

        // 수비차례, computer의 경우 DefenseTrun_Player를 스킵
        num301_DefenseTrun_Player = 301,
        num302_DefenseDone,
        num303_PlayerCantDefense,


        // 공격, 방어를 진행한 후 카드를 동시에 오픈
        num401_CardOpenAtTheSameTime = 401,

        // 결과 발표
        num402_OnJokerAppear,
        num403_OnAttackSuccess,
        num404_OnDefenceSuccess,
        num405_OnHuntPrey,

        num406_OnPlayerBankrupt_GetOut,
        num407_OnPlayerBankrupt_GoToMining,

        num408_OnChooseNextPlayer,


        num501_final = 501,
        num502_EndGame

    }

    public enum eCriteria
    {
        None = 0,
        JokerWin,
        AttakkerWin,
        DeffenderWin,
        HuntingTime
    }

    public enum eSystemGuide
    {
        None = 0,

        WelcomeFromGM = 100,
        ExplainFeature = 101,

        
        GameAssistantNotAvailable = 1001,

        HowToCardSelect = 2001,
        HowToUseGameAssistant_OnlyOneLives = 2002,
        HowToUseLoupe = 2003,

        violationOfGameSettingRules = 3000,
        makeSureOfYourChoice = 3001
    }


    public enum eCharacterType
    {
        None,

        System = 101,
        Narration,
        GameMaster,
        Unknown = 404,

        Player = 1001,
        MunDuckBea,
        Caesar,

        CasinoDealer = 10001,
        KangDoYun,
        SeoJiHoo,
        LeeHaRin,
        ChoiGeonWoo,
        YoonChaeYoung,
        ParkMinSeok,
        JangSeoYoon,
        OhJinSoo
    }

    public enum eItemType
    {
        None,

        // 퀘스트 아이템
        CasinoEntryTicket = 101,

        // 소모성 아이템
        Meat = 1001,
        Fish = 2001,
        Egg = 3001,

        // 기타 잡템
        Notice_Stage1 = 10001
    }
    public enum eItemCallback
    {
        None,
        CasinoTicket,
        EatMeal
    }

    public enum eQuestType
    {
        None,

        //튜토리얼 관련
        LetsLookAroundOutside,
        LetsOpenBoxForTheFirstTime,
        UseCasinoEntryTicket,
        LearnHowToSave,
        StartFirstGame,
        GoToSleep,
        CheckTheBoxEveryDay,
        TryUseDarkWebMarket,

        // 게임 목적 관련
        Collect2000Coins,
        Collect5000Coins,
        Collect8000Coins,
        Collect10000Coins
    }


    public enum eHasEndCallback
    {
        No,
        yes
    }
    public enum eHasSelection
    {
        No,
        yes
    }


    public enum eIcon
    {
        IconViewOnOff,
        Quest,
        Inventory,
        GameAssistant,
        Message
    }

    public enum ePopUpState
    {
        Open,
        Close
    }

    public enum eCardType
    {
        Joker,
        Spades,
        Clubs,
        Hearts,
        Diamonds
    }

    //Class
    public class cTextScriptInfo
    {

        public eCharacterType characterEnum { get; set; }
        public int DialogueIconIndex { get; set; }
        public string script { get; set; }
        public eHasEndCallback hasEndCallback { get; set; }
        public UnityAction endCallback { get; set; }
        public eHasSelection hasSelection { get; set; }
        public List<string> selectionScript { get; set; }
        public List<UnityAction> SelectionCallback { get; set; }

        public cTextScriptInfo()
        {
            characterEnum = eCharacterType.None;
            hasEndCallback = eHasEndCallback.No;
            endCallback = null;
            hasSelection = eHasSelection.No;
            selectionScript = new List<string>();
            SelectionCallback = new List<UnityAction>();
        }
    }

    public interface INeedCheck
    {
        public bool _isNeedCheck { get;}
        public bool isNeedCheck { get; set; }

        public void InitBool();
    }

    public class cItemInfo : INeedCheck
    {
        // 기본정보
        public eItemType type { get; set; }
        public string name { get; set; }
        

        // 사용이 가능한 경우 
        public bool isAvailable { get; set; }
        public bool isConsumable { get; set; } // 소모성 아이템 여부
        public float value_Use { get; set; } // 사용 가능할시 적용할 값

        // 판매가 가능한 경우
        public bool isForSale { get; set; }
        public int value_Sale { get; set; }

        // 추가 정보
        public bool _isNeedCheck { get; private set; }

        /// <summary>
        /// 팝업 리프레시와 클릭가이드 활성화를 포함
        /// </summary>
        public bool isNeedCheck
        {
            get
            {
                return _isNeedCheck;
            }

            set
            {
                if (value)
                {
                    //GameManager.connector_InGame.iconView_Script.TryClickGuideOn(eIcon.Inventory);
                    GameManager.connector_InGame.popUpViewAsInGame.inventoryPopUp.RefreshPopUp();
                }
                _isNeedCheck = value;
            }
        }

        public void InitBool()
        {
            _isNeedCheck = true;
        }

        public List<string> descriptionList { get; set; }

        public cItemInfo()
        {
            descriptionList = new List<string>();
            isAvailable = false;
            isConsumable = false;
            value_Use = 0;
            isForSale = false;
            value_Sale = 0;

            // 로비씬 로드시 다시 초기화되는 값
            InitBool();
        }

        // 스크립트에서 별도로 추가할 값들
        public UnityAction itemCallback { get; set; }

    }

    public class cQuestInfo : INeedCheck
    {
        // type으로 묶어서 처리
        public eQuestType type { get; set; }
        public UnityAction checkEndCondition { get; set; }
        public UnityAction endEvent {  get; set; }


        // 나머지 컬럼으로 처리
        public string name { get; set; }
        public int rewardCoin { get; set; }
        public eItemType rewardItemType { get; set; }
        public bool isRepeatable { get; set; }

        // 별도 관리
        public bool _isNeedCheck { get; private set; }
        public bool isNeedCheck
        {
            get
            {
                return _isNeedCheck;
            }

            set
            {
                if(value) 
                { 
                    //GameManager.connector_InGame.iconView_Script.TryClickGuideOn(eIcon.Quest);
                    GameManager.connector_InGame.popUpViewAsInGame.questPopUp.RefreshPopUp();
                }

                _isNeedCheck = value;
            }
        }
        public bool isComplete { get; set; }
        public bool hasReceivedReward { get; set; }

        public void InitBool()
        {
            _isNeedCheck = true;
            isComplete = hasReceivedReward = false;
        }

        public List<string> descriptionList { get; set; }

        public cQuestInfo()
        {
            descriptionList = new List<string>();

            // 로비씬 로드시 다시 초기화 되는 값
            InitBool();
        }

    }

    public class cOnlyOneLivesGameRule
    {
        public string Title { get; set; }
        public List<string> DescriptionList { get; set; }

        public cOnlyOneLivesGameRule()
        {
            DescriptionList = new List<string>();
        }
    }
    public class cQuestDescription
    {
        

        public cQuestDescription()
        {
            
        }
    }


    public class cTrumpCardInfo
    {
        public int cardIndex { get; set; }
        public string cardName { get; set; }
        public eCardType cardType { get; set; }
        public int cardValue { get; set; }
    }

    public class cCharacterInfo
    {
        public eCharacterType CharaterIndex { get; set; }
        public string CharacterName { get; set; }
        public string CharacterAge { get; set; }
        public string CharacterClan { get; set; }
        public string CharacterFeature { get; set; }
    }

}
