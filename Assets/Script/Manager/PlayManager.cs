using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayManager : Singleton<PlayManager>
{
    public sPlayerStatus currentPlayerStatus { get; private set; }

    private readonly HashSet<eQuestType> GoalQuests = new()
    {
        eQuestType.Collect2000Coins,
        eQuestType.Collect5000Coins,
        eQuestType.Collect8000Coins,
        eQuestType.Collect10000Coins
    };


    private Text _playerMoneyViewText; // 플레이어의 돈을 화면에 표시할 텍스트
    private PlayerMoneyAnimation _moneyAnimation;

    public Text playerMoneyViewText
    {
        get
        {
            CheckPlayerMoneyViewText();
            return _playerMoneyViewText;
        }
    }
    public PlayerMoneyAnimation moneyAnimation
    {
        get
        {
            CheckMoneyAnimation();
            return _moneyAnimation;
        }
    }

    private void CheckPlayerMoneyViewText()
    {
        if (_playerMoneyViewText == null) 
            _playerMoneyViewText = GameManager.connector_InGame.Canvas1.PlayerMoneyView.coinResult;
    }
    private void CheckMoneyAnimation()
    {
        if (_moneyAnimation == null)
            _moneyAnimation = GameManager.connector_InGame.Canvas1.PlayerMoneyView.GetComponent<PlayerMoneyAnimation>();
    }

    //public int current
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        CheckPlayerMoneyViewText();
        CheckMoneyAnimation();
    }

    public void InitPlayerStatus(sPlayerStatus value = default)
    {
        currentPlayerStatus = value;
        UpdateInterface();
    }

    

    public void UpdateInterface()
    {
        playerMoneyViewText.text = "x" + currentPlayerStatus.coin.ToString();
    }

    /// <summary>
    /// 플레이어가 소지하는 코인개수를 초기화
    /// </summary>
    /// <param name="setValue"></param>
    public void SetPlayerMoney(int setValue)
    {
        var update = currentPlayerStatus;
        update.coin = setValue;
        currentPlayerStatus = update;
        UpdateInterface();
    }

    /// <summary>
    /// 플레이어가 갖고있는 돈에 추가값을 설정, 파산여부는 같은 스크립트의 TryMinusCoin에서 판별
    /// </summary>
    /// <param name="Value"></param>
    /// <returns>파산여부를 확인</returns>
    public void AddPlayerMoney(int Value)
    {
        sPlayerStatus update = currentPlayerStatus;
        update.coin += Value;
        currentPlayerStatus = update;

        // 전광판 초기화
        playerMoneyViewText.text = "x" + currentPlayerStatus.coin.ToString();

        // 변화량 애니메이션
        if (Value > 0) // AddCoin에 의해 호출된 경우
        {
            moneyAnimation.PlaySequnce_PlayerMoneyPlus(Value);
        }
        else if (Value < 0) // TryMinusCoin에 의해 호출된 경우
        {
            Value = (-Value); // 전광판에 띄우기 위해 양수로 바꿈
            moneyAnimation.PlaySequnce_PlayerMoneyMinus(Value);
        }

        CheckQuestComplete();
    }

    
    private void CheckQuestComplete()
    {
        cQuestInfo questInfo;
        foreach (eQuestType questType in GoalQuests)
        {
            questInfo = CsvManager.Instance.GetQuestInfo(questType);
            questInfo.checkEndCondition();
        }
    }
    


    public void StartPlayerMonologue()
    {
        CallbackBase.TextWindowPopUp_Open();
        GameManager.connector_InGame.Canvas1.TextWindowView.StartTextWindow(eTextScriptFile.PlayerMonologue);
    }

    public void StartPlayerMonologue_OnPlayerWakeUp()
    {
        CallbackBase.TextWindowPopUp_Open();
        GameManager.connector_InGame.Canvas1.TextWindowView.StartTextWindow(eTextScriptFile.OnPlayerWakeUp);
    }
}
