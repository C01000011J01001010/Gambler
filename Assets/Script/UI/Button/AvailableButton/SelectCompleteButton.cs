using DG.Tweening;
using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCompleteButton : Deactivatable_ButtonBase
{
    // ������ ����
    //public CardGamePlayManager cardGamePlayManager;
    public PlayerMe playerMe;
    public Text textComp;
    [SerializeField] private CardButtonMemoryPool _cardButtonMemoryPool;
    [SerializeField] private GameObject _clickGuide;

    // ��ũ��Ʈ ����
    public CardButtonMemoryPool cardButtonMemoryPool => _cardButtonMemoryPool;
    public GameObject clickGuide => _clickGuide;


    public readonly string onFirstButtonText = "������ ī��\n�����ϱ�";
    public readonly string onAttackOrDeffenceButtonText = "������ ī��\n�����ϱ�";



    public void InitAttribute()
    {
        SetButtonCallback(CompleteCardSelect_ChooseCardsToReveal);
        TryDeactivate_Button();
        ChangeText(onFirstButtonText);
    }

    private void Start()
    {
        if(CardGamePlayManager.Instance == null)
        {
            Debug.LogAssertion("ī����ӸŴ��� ���� �ȵ���");
        }
        if(cardButtonMemoryPool == null)
        {
            Debug.LogAssertion("ī���ư���� ���� �ȵ���");
        }
        if(playerMe == null)
        {
            Debug.LogAssertion("�÷��̾� ���� �ȵ���");
        }
        if(textComp == null)
        {
            Debug.LogAssertion("�ڽİ�ü�� text�� ���� �ȵ���");
        }
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
        
        cardButtonMemoryPool.CheckClickGuide(true);
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
        CardGamePlayManager.Instance.cardGameView.cardScreenOpenButton.TryDeactivate_Button();
        

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
        CardGamePlayManager.Instance.cardGameView.cardScreenOpenButton.TryDeactivate_Button();

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
                    //if (CardGamePlayManager.Instance.isDistributionCompleted && playerMe.isCompleteSelect_OnGameSetting)
                    if (playerMe.isCompleteSelect_OnGameSetting)
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
        clickGuide.SetActive(isOn);
        cardButtonMemoryPool.CheckClickGuide(isOn);
    }
}
