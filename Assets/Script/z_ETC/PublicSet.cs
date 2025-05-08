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
        Defualt, // �������� ������� ����
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
        // �ǳ�
        Bed,
        Cabinet,
        Clock,
        Computer,
        InsideDoor,

        // �߿�
        Box_Empty,
        Box_Full,
        OutsideDoor,
        NPC_MunDuckBea_Acquaintance,
        NPC_MunDuckBea_Encounter,

        // ī����
        NPC_MunDuckBea_InCasino,
        NPC_Caesar,
        CasinoDoor,

        // -----------None Interactable--------------
        // �÷��̾� ȥ�㸻
        PlayerMonologue,
        OnPlayerWakeUp,

        // ī����
        //GameMaster,
        PlayerCantPlayThis
    }

    /// <summary>
    /// == onlyOneLivesProgress
    /// </summary>
    public enum eOOLProgress
    {
        num101_BeforeStartGame = 101, // ���ӿ� ���� �⺻���� ������ �ϴ� �ܰ�
        num102_BeforeRotateDiceAndDistribution, // �ֻ����� ������ ī�带 �й��ϴ� �ܰ�
        num103_BeforeChooseCardsToReveal, // �� �÷��̾ ������ ī�带 �����ϴ� �ܰ�
        num104_OnChooseFirstPlayer, // ���ӿ��� ù ������ ������ �÷��̾ �����ϴ� �ܰ�

        // ��������, computer�� ��� AttackTurn_Player�� ��ŵ
        num201_AttackTurnPlayer = 201,
        num202_AttackDone,
        num203_PlayerCantAttack,

        // ��������, computer�� ��� DefenseTrun_Player�� ��ŵ
        num301_DefenseTrun_Player = 301,
        num302_DefenseDone,
        num303_PlayerCantDefense,


        // ����, �� ������ �� ī�带 ���ÿ� ����
        num401_CardOpenAtTheSameTime = 401,

        // ��� ��ǥ
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

        // ����Ʈ ������
        CasinoEntryTicket = 101,

        // �Ҹ� ������
        Meat = 1001,
        Fish = 2001,
        Egg = 3001,

        // ��Ÿ ����
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

        //Ʃ�丮�� ����
        LetsLookAroundOutside,
        LetsOpenBoxForTheFirstTime,
        UseCasinoEntryTicket,
        LearnHowToSave,
        StartFirstGame,
        GoToSleep,
        CheckTheBoxEveryDay,
        TryUseDarkWebMarket,

        // ���� ���� ����
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
        // �⺻����
        public eItemType type { get; set; }
        public string name { get; set; }
        

        // ����� ������ ��� 
        public bool isAvailable { get; set; }
        public bool isConsumable { get; set; } // �Ҹ� ������ ����
        public float value_Use { get; set; } // ��� �����ҽ� ������ ��

        // �ǸŰ� ������ ���
        public bool isForSale { get; set; }
        public int value_Sale { get; set; }

        // �߰� ����
        public bool _isNeedCheck { get; private set; }

        /// <summary>
        /// �˾� �������ÿ� Ŭ�����̵� Ȱ��ȭ�� ����
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

            // �κ�� �ε�� �ٽ� �ʱ�ȭ�Ǵ� ��
            InitBool();
        }

        // ��ũ��Ʈ���� ������ �߰��� ����
        public UnityAction itemCallback { get; set; }

    }

    public class cQuestInfo : INeedCheck
    {
        // type���� ��� ó��
        public eQuestType type { get; set; }
        public UnityAction checkEndCondition { get; set; }
        public UnityAction endEvent {  get; set; }


        // ������ �÷����� ó��
        public string name { get; set; }
        public int rewardCoin { get; set; }
        public eItemType rewardItemType { get; set; }
        public bool isRepeatable { get; set; }

        // ���� ����
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

            // �κ�� �ε�� �ٽ� �ʱ�ȭ �Ǵ� ��
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
