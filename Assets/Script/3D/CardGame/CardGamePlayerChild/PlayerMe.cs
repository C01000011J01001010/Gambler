using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMe : CardGamePlayerBase
{
    public bool isCompleteSelect_OnGameSetting {  get; private set; }
    public bool isCompleteSelect_OnPlayTime { get; private set; }
    public bool isAttack {  get; private set; }


    private SelectCompleteButton _selectCompleteButton;
    public SelectCompleteButton selectCompleteButton
    {
        get
        {
            CheckSelectCompleteButton();
            return _selectCompleteButton;
        }
    }
    private void CheckSelectCompleteButton()
    {
        if (_selectCompleteButton == null)
            _selectCompleteButton = CardGamePlayManager.Instance.cardGameView.selectCompleteButton;
    }

    private void Start()
    {
        CheckSelectCompleteButton();
        cCharacterInfo info = CsvManager.Instance.GetCharacterInfo(eCharacterType.Player);
        SetCharacterInfo(info);
    }

    public override void AddCoin(int value)
    {
        coin += value;
        PlayManager.Instance.AddPlayerMoney(value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">����� ������ ����</param>
    /// <returns></returns>
    public override void TryMinusCoin(int value, out bool isBankrupt, out bool hasDebt)
    {
        isBankrupt = false;
        hasDebt = false;

        // �Ļ� ���� üũ
        if (coin <= value)
        {
            isBankrupt = true;

            // ������ ���� �������� üũ
            if(coin < value)
            {
                hasDebt = true;
            }
        }

        // coin�� (-)���̳ʽ��� �ȴٸ� �������� ������ �ް� ��� ���شٴ� ����
        coin = coin - value;

        PlayManager.Instance.AddPlayerMoney(-value);
    }


    public override void InitAttribute_All()
    {
        base.InitAttribute_All();
        isCompleteSelect_OnGameSetting = false;
    }

    public override void InitAttribute_ForNextOrder()
    {
        base.InitAttribute_ForNextOrder();
        isCompleteSelect_OnPlayTime = false;
    }

    public override bool TryDownCountPerCardType(cTrumpCardInfo cardInfo)
    {
        if (cardCountPerType_GameSetting[cardInfo.cardType] > 1)
        {
            cardCountPerType_GameSetting[cardInfo.cardType]--;

            // �÷��̾ TryDownCountPerCardType�� ������ �� ������ �Ϸ�ƴ����� Ȯ���ϰ� ��ư�� Ȱ��ȭ��
            selectCompleteButton.CheckCompleteSelect_OnChooseCardsToReveal(cardCountPerType_GameSetting);

            //Debug.Log($"{gameObject.name}���� {cardInfo.cardName}ī�� ����");
            //Debug.Log($"{gameObject.name}�� {cardInfo.cardType.ToString()} ���� ī�� �� :" +
            //    $" {cardCountPerType_GameSetting[cardInfo.cardType]}");

            return true;
        }
        else
        {

            //Debug.Log($"{gameObject.name}�� {cardInfo.cardType.ToString()}�� ���� ī�� �� :" +
            //    $" {cardCountPerType_GameSetting[cardInfo.cardType]}");
            //Debug.Log("ī�� ������ ���� �� ����");

            return false;
        }
    }


    public void Set_isCompleteSelect_OnGameSetting(bool value)
    {
        isCompleteSelect_OnGameSetting = value;
    }
    public virtual void Set_isCompleteSelect_OnPlayTime(bool value)
    {
        isCompleteSelect_OnPlayTime = value;
    }
    


    public override void AttackOtherPlayers(List<CardGamePlayerBase> playerList)
    {
        CardGamePlayManager.Instance.NextProgress(); // 201�� ����

        // �� ī�� ���� Ȱ��ȭ
        CardGamePlayManager.Instance.cardGameView.cardScreenButtonSet.PlaySequence_FadeIn();

        // ī���ư ��� Ȱ��ȭ
        CardGamePlayManager.Instance.cardButtonMemoryPool.SetAllButtonInteractable(true);

        // TargetDisplay���� ����� ������ �� �ֵ��� ����
        CardGamePlayManager.Instance.cardGameView.targetDisplay.LiftRestrictionToAllSelections();
        //GameAssistantPopUp_OnlyOneLives.Instance.LiftRestrictionToAllSelections();

        // ��ư Ŭ���� �ݹ��� ����
        isAttack = true;
        selectCompleteButton.SetButtonCallback(1);
        // ��븦 �����ϰ� ī�� ������ �Ϸ��ϸ� 202�� �����ؾ���
    }

    

    public override void DeffenceFromOtherPlayers(CardGamePlayerBase AttackerScript)
    {
        CardGamePlayManager.Instance.NextProgress(); // 301�� ����

        // �� ī�� ���� Ȱ��ȭ
        CardGamePlayManager.Instance.cardGameView.cardScreenButtonSet.PlaySequence_FadeIn();

        // ī���ư ��� Ȱ��ȭ
        CardGamePlayManager.Instance.cardButtonMemoryPool.SetAllButtonInteractable(true);

        // ��ư Ŭ���� �ݹ��� ����
        isAttack = false;
        selectCompleteButton.SetButtonCallback(1);
        // ��븦 �����ϰ� ī�� ������ �Ϸ��ϸ� 302�� �����ؾ���
    }

    
}
