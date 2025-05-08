using DG.Tweening;
using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCompleteButton : Deactivatable_ButtonBase
{
    // 에디터 연결
    [SerializeField] private PlayerMe playerMe;
    [SerializeField] private Text textComp;
    [SerializeField] private GameObject _clickGuide;

    // 스크립트 편집
    public GameObject clickGuide => _clickGuide;

    public readonly string onFirstButtonText = "선택한 카드\n오픈하기";
    public readonly string onAttackOrDeffenceButtonText = "선택한 카드\n제시하기";


    // 캐싱
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

    // 실행하는 시점에서 컴퓨터는 이미 선택을 완료했어야함
    public void CheckCompleteSelect_OnChooseCardsToReveal(Dictionary<eCardType, int> cardCountPerType)
    {
        int enumLength = Enum.GetValues(typeof(eCardType)).Length;
        for (int i = 0; i < enumLength; i++)
        {
            // 남은 카드의 개수가 1보다 많으면 선택이 완료된 것이 아님
            if (cardCountPerType[(eCardType)i] > 1)
            {
                playerMe.Set_isCompleteSelect_OnGameSetting(false);
                Debug.Log("카드 선택이 완료되지 않음");

                cardButtonMemoryPool.CheckClickGuide(false);
                TryDeactivate_Button();
                return;
            }
        }
        playerMe.Set_isCompleteSelect_OnGameSetting(true);
        // 선택이 완료됐으면 버튼을 활성화 시도
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
        Debug.Log("CompleteCardSelect_OnStartTime 실행");

        // 내카드 보기 비활성화
        cardScreenOpenButton.TryDeactivate_Button();
        

        // 애니메이션 실행
        Sequence sequence = DOTween.Sequence();

        // 서브스크린 닫기
        CardGamePlayManager.Instance.cardGameView.cardScreen.GetSequnce_TryCardScrrenClose(sequence);

        // 내 카드 보기 페이드 아웃
        CardGamePlayManager.Instance.cardGameView.cardScreenButtonSet.GetSequence_FadeOut(sequence);

        // 카드 이동 및 공개
        CardGameAnimationManager.Instance.GetSequnce_ChooseCardsToReveal_Aniamaition(sequence);

        // 애니메이션이 끝난 후 본게임에 진입
        sequence.AppendInterval(2.0f);
        sequence.AppendCallback(CardGamePlayManager.Instance.NextProgress);// 모두 끝난후 다음상태를 진행
        sequence.SetLoops(1);
        sequence.Play();

        // 버튼이 클릭됐으면 똑같은 액션은 할 수 없음
        TryDeactivate_Button();
    }

    public void CompleteCardSelect_OnAttack_Or_OnDeffence()
    {
        Debug.Log($"CompleteCardSelect_OnAttack_Or_OnDeffence 실행");

        // 내카드 보기 비활성화
        cardScreenOpenButton.TryDeactivate_Button();

        // 역할이 끝난 게임어시스턴트의 선택기능 종료
        if(CardGamePlayManager.Instance.Attacker == playerMe)
            CardGamePlayManager.Instance.cardGameView.targetDisplay.PlaceRestrictionToAllSelections();

        // 애니메이션 실행
        Sequence sequence = DOTween.Sequence();

        // 서브스크린 닫기
        CardGamePlayManager.Instance.cardGameView.cardScreen.GetSequnce_TryCardScrrenClose(sequence);

        // 내 카드 보기 페이드 아웃
        CardGamePlayManager.Instance.cardGameView.cardScreenButtonSet.GetSequence_FadeOut(sequence);

        // 카드 제시하기
        playerMe.GetSequnce_PresentCard(sequence,playerMe.isAttack);

        sequence.AppendCallback(CardGamePlayManager.Instance.NextProgress);// 모두 끝난후 다음상태를 진행
        sequence.SetLoops(1);
        sequence.Play();

        // 버튼이 클릭됐으면 똑같은 액션은 할 수 없음
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
                    Debug.LogAssertion("잘못된 접근");
                    return;
                }

        }
    }


    public override void SetButtonInteractable(bool isOn)
    {
        base.SetButtonInteractable(isOn);
        ClickGuideSetActive(isOn);

        // 버튼이 활성화 될때만 적용됨
        if(isOn) cardButtonMemoryPool.CheckClickGuide(true);
        //cardButtonMemoryPool.CheckClickGuide(isOn); // 선택 완료되어 비활성화 하는데 카드 선택 버튼이 활성화되는 문제가 있음
    }

    private void ClickGuideSetActive(bool isOn)
    {
        clickGuide.SetActive(isOn);
        cardScreen.OpenButtonCheckClickGuide();
    }
}
