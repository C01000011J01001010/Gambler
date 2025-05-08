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


    private Text _playerMoneyViewText; // �÷��̾��� ���� ȭ�鿡 ǥ���� �ؽ�Ʈ
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
    /// �÷��̾ �����ϴ� ���ΰ����� �ʱ�ȭ
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
    /// �÷��̾ �����ִ� ���� �߰����� ����, �Ļ꿩�δ� ���� ��ũ��Ʈ�� TryMinusCoin���� �Ǻ�
    /// </summary>
    /// <param name="Value"></param>
    /// <returns>�Ļ꿩�θ� Ȯ��</returns>
    public void AddPlayerMoney(int Value)
    {
        sPlayerStatus update = currentPlayerStatus;
        update.coin += Value;
        currentPlayerStatus = update;

        // ������ �ʱ�ȭ
        playerMoneyViewText.text = "x" + currentPlayerStatus.coin.ToString();

        // ��ȭ�� �ִϸ��̼�
        if (Value > 0) // AddCoin�� ���� ȣ��� ���
        {
            moneyAnimation.PlaySequnce_PlayerMoneyPlus(Value);
        }
        else if (Value < 0) // TryMinusCoin�� ���� ȣ��� ���
        {
            Value = (-Value); // �����ǿ� ���� ���� ����� �ٲ�
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
