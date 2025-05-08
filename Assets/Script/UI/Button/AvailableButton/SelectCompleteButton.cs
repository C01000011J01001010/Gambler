using DG.Tweening;
using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCompleteButton : Deactivatable_ButtonBase
{
    // ������ ����
    [SerializeField] private PlayerMe playerMe;
    [SerializeField] private Text textComp;
    [SerializeField] private GameObject _clickGuide;

    // ��ũ��Ʈ ����
    public GameObject clickGuide => _clickGuide;

    public readonly string onFirstButtonText = "������ ī��\n�����ϱ�";
    public readonly string onAttackOrDeffenceButtonText = "������ ī��\n�����ϱ�";


    // ĳ��
    private CardScreen _cardScreen;
    private CardButtonMemoryPool _cardButtonMemoryPool;
    private CardScreenOpenButton _cardScreenOpenButton;

    public CardScreen cardScreen
    {
        get
        {
            CheckCardScreen();
            return _cardScreen;
        }
    }
    public CardButtonMemoryPool cardButtonMemoryPool
    {
        get
        {
            CheckCardButtonMemoryPool();
            return _cardButtonMemoryPool;
        }
    }
    public CardScreenOpenButton cardScreenOpenButton
    {
        get
        {
            CheckCardScreenOpenButton();
            return _cardScreenOpenButton;
        }
    }

    public void CheckCardScreen()
    {
        if (_cardScreen == null) 
            _cardScreen = GameManager.connector_InGame.Canvas0.CardGameView.cardScreen;
    }
    public void CheckCardButtonMemoryPool()
    {
        if (_cardButtonMemoryPool == null)
            _cardButtonMemoryPool = cardScreen.cardButtonMemoryPool;
    }
    public void CheckCardScreenOpenButton()
    {
        if (_cardScreenOpenButton == null)
            _cardScreenOpenButton = cardScreen.cardScreenOpenButton;
    }

    private void Start()
    {
        CheckCardScreen();
        CheckCardButtonMemoryPool();
        CheckCardScreenOpenButton();
    }

    public void InitAttribute()
    {
        SetButtonCallback(CompleteCardSelect_ChooseCardsToReveal);
        TryDeactivate_Button();
        ChangeText(onFirstButtonText);
    }

    // �����ϴ� �������� ��ǻ�ʹ� �̹� ������ �Ϸ��߾����
    public void CheckCompleteSelect_OnChooseCardsToReveal(Dictionary<eCardType, int> cardCountPerType)
    {
        int enumLength = Enum.GetValues(typeof(eCardType)).Length;
        for (int i = 0; i < enumLength; i++)
        {
            // ���� ī���� ������ 1���� ������ ������ �Ϸ�� ���� �ƴ�
            if (cardCountPerType[(eCardType)i] > 1)
            {
                playerMe.Set_isCompleteSelect_OnGameSetting(false);
                Debug.Log("ī�� ������ �Ϸ���� ����");

                cardButtonMemoryPool.CheckClickGuide(false);
                TryDeactivate_Button();
                return;
            }
        }
        playerMe.Set_isCompleteSelect_OnGameSetting(true);
        // ������ �Ϸ������ ��ư�� Ȱ��ȭ �õ�
        TryActivate_Button();
    }

    public void ChangeText(string text)
    {
        textComp.text = text;
    }

    public void SetButtonCallback(int Select)
    {
        switch(Select)
        {
            case 0:
                ChangeText(onFirstButtonText);
                SetButtonCallback(CompleteCardSelect_ChooseCardsToReveal);break;
            case 1:
                ChangeText(onAttackOrDeffenceButtonText);
                SetButtonCallback(CompleteCardSelect_OnAttack_Or_OnDeffence); break;
        }
    }

    public void CompleteCardSelect_ChooseCardsToReveal()
    {
        Debug.Log("CompleteCardSelect_OnStartTime ����");

        // ��ī�� ���� ��Ȱ��ȭ
        cardScreenOpenButton.TryDeactivate_Button();
        

        // �ִϸ��̼� ����
        Sequence sequence = DOTween.Sequence();

        // ���꽺ũ�� �ݱ�
        CardGamePlayManager.Instance.cardGameView.cardScreen.GetSequnce_TryCardScrrenClose(sequence);

        // �� ī�� ���� ���̵� �ƿ�
        CardGamePlayManager.Instance.cardGameView.cardScreenButtonSet.GetSequence_FadeOut(sequence);

        // ī�� �̵� �� ����
        CardGameAnimationManager.Instance.GetSequnce_ChooseCardsToReveal_Aniamaition(sequence);

        // �ִϸ��̼��� ���� �� �����ӿ� ����
        sequence.AppendInterval(2.0f);
        sequence.AppendCallback(CardGamePlayManager.Instance.NextProgress);// ��� ������ �������¸� ����
        sequence.SetLoops(1);
        sequence.Play();

        // ��ư�� Ŭ�������� �Ȱ��� �׼��� �� �� ����
        TryDeactivate_Button();
    }

    public void CompleteCardSelect_OnAttack_Or_OnDeffence()
    {
        Debug.Log($"CompleteCardSelect_OnAttack_Or_OnDeffence ����");

        // ��ī�� ���� ��Ȱ��ȭ
        cardScreenOpenButton.TryDeactivate_Button();

        // ������ ���� ���Ӿ�ý���Ʈ�� ���ñ�� ����
        if(CardGamePlayManager.Instance.Attacker == playerMe)
            CardGamePlayManager.Instance.cardGameView.targetDisplay.PlaceRestrictionToAllSelections();

        // �ִϸ��̼� ����
        Sequence sequence = DOTween.Sequence();

        // ���꽺ũ�� �ݱ�
        CardGamePlayManager.Instance.cardGameView.cardScreen.GetSequnce_TryCardScrrenClose(sequence);

        // �� ī�� ���� ���̵� �ƿ�
        CardGamePlayManager.Instance.cardGameView.cardScreenButtonSet.GetSequence_FadeOut(sequence);

        // ī�� �����ϱ�
        playerMe.GetSequnce_PresentCard(sequence,playerMe.isAttack);

        sequence.AppendCallback(CardGamePlayManager.Instance.NextProgress);// ��� ������ �������¸� ����
        sequence.SetLoops(1);
        sequence.Play();

        // ��ư�� Ŭ�������� �Ȱ��� �׼��� �� �� ����
        TryDeactivate_Button();
    }

    public override void TryActivate_Button()
    {
        switch (CardGamePlayManager.Instance.currentProgress)
        {
            case eOOLProgress.num102_BeforeRotateDiceAndDistribution:
            case eOOLProgress.num103_BeforeChooseCardsToReveal:
                {
                    if (CardGamePlayManager.Instance.isDistributionCompleted && playerMe.isCompleteSelect_OnGameSetting)
                    //if (playerMe.isCompleteSelect_OnGameSetting)
                    {
                        SetButtonInteractable(true);
                    }
                    return;
                }
            case eOOLProgress.num201_AttackTurnPlayer:
                {
                    if(playerMe.AttackTarget != null && playerMe.isCompleteSelect_OnPlayTime)
                    {
                        SetButtonInteractable(true);
                    }
                    return;
                }
            case eOOLProgress.num301_DefenseTrun_Player:
                {
                    if (playerMe.isCompleteSelect_OnPlayTime)
                    {
                        SetButtonInteractable(true);
                    }
                    return;
                }
            default:
                {
                    Debug.LogAssertion("�߸��� ����");
                    return;
                }

        }
    }


    public override void SetButtonInteractable(bool isOn)
    {
        base.SetButtonInteractable(isOn);
        ClickGuideSetActive(isOn);

        // ��ư�� Ȱ��ȭ �ɶ��� �����
        if(isOn) cardButtonMemoryPool.CheckClickGuide(true);
        //cardButtonMemoryPool.CheckClickGuide(isOn); // ���� �Ϸ�Ǿ� ��Ȱ��ȭ �ϴµ� ī�� ���� ��ư�� Ȱ��ȭ�Ǵ� ������ ����
    }

    private void ClickGuideSetActive(bool isOn)
    {
        clickGuide.SetActive(isOn);
        cardScreen.OpenButtonCheckClickGuide();
    }
}
