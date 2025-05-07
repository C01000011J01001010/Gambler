using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardSelectButton : ImageChange_ButtonBase
{
    // 에디터 연결
    public Sprite[] sprites;

    [SerializeField] GameObject _clickGuide;


    // 스크립트 편집
    public bool isOn {  get; private set; }
    public CardButtonMemoryPool parent {  get; private set; }
    public GameObject ButtonToCard {  get; private set; }
    public TrumpCardDefault trumpCardScript { get; private set; }

    public GameObject clickGuide => _clickGuide;

    // 메모리풀에서 꺼내질때마다 실행
    private void OnEnable()
    {
        ChangeOn();
    }

    public void SetCardButtonImage(int orderInHnad)
    {
        //Debug.Log($"{orderInHnad}번 카드의 버튼 생성예정");
        if(image != null)
        {
            if(orderInHnad < sprites.Length)
            {
                image.sprite = sprites[orderInHnad];
            }
            else
            {
                Debug.LogAssertion("sprite 확인 필요");
            }
        }
        else
        {
            Debug.LogAssertion("image == null");
        }
    }

    public void MappingButtonWithCard(GameObject card)
    {
        TrumpCardDefault trumpCardDefault = card.GetComponent<TrumpCardDefault>();

        if(trumpCardDefault != null)
        {
            ButtonToCard = card;
            trumpCardScript = trumpCardDefault;
            //UnselectThisCard();
        }
        else
        {
            Debug.LogAssertion($"{card.name}은 card가 아님");
        }
        
    }


    private void CheckProperties()
    {
        if (ButtonToCard == null)
        {
            Debug.LogAssertion($"{ButtonToCard.gameObject.name}의 trumpCardScript == null");
            return;
        }
        if (trumpCardScript == null)
        {
            Debug.LogAssertion("ButtonToCard == null");
            return;
        }
        if (parent == null)
        {
            parent = transform.parent.GetComponent<CardButtonMemoryPool>();
        }
    }


    public void SelectThisCard_OnGameSetting()
    {
        CheckProperties();

        if (parent != null)
        {
            if (trumpCardScript.TrySelectThisCard_OnGameSetting(parent.playerMe))
            {
                // 버튼 전환
                ChangeOff();
                SetButtonCallback(UnselectThisCard_OnGameSetting);
            }
            else
            {
                GameManager.connector_InGame.Canvas1.TextWindowView.StartTextWindow(eSystemGuide.violationOfGameSettingRules);
                return;
            }
        }
        else
        {
            Debug.LogAssertion($"{transform.parent.gameObject.name}의 CardButtonSet == null");
        }
    }
    public void UnselectThisCard_OnGameSetting()
    {
        CheckProperties();

        if (parent != null)
        {
            trumpCardScript.UnselectThisCard_OnStartTime(parent.playerMe);

            // 버튼 전환
            ChangeOn();
            SetButtonCallback(SelectThisCard_OnGameSetting);
        }
        else
        {
            Debug.LogAssertion($"{transform.parent.gameObject.name}의 CardButtonSet == null");
        }

    }

    public void SelectThisCard_OnPlayTime()
    {
        CheckProperties();

        if (trumpCardScript.TrySelectThisCard_OnPlayTime(parent.playerMe))
        {
            // 버튼 전환
            ChangeOff();
            SetButtonCallback(UnselectThisCard_OnPlayTime);
            CardGamePlayManager.Instance.cardGameView.selectCompleteButton.TryActivate_Button();

            // 선택가능한 카드가 하나뿐이나 선택이 완료되면 모든 ClickGuide를 종료
            parent.CheckClickGuide(true);
        }
        else
        {
            GameManager.connector_InGame.Canvas1.TextWindowView.StartTextWindow(eSystemGuide.makeSureOfYourChoice);
            return;
        }
        
    }

    public void UnselectThisCard_OnPlayTime()
    {
        CheckProperties();

        trumpCardScript.UnselectThisCard_OnPlayTime(parent.playerMe);
        // 버튼 전환
        ChangeOn();
        SetButtonCallback(SelectThisCard_OnPlayTime);
        CardGamePlayManager.Instance.cardGameView.selectCompleteButton.TryDeactivate_Button();

        // 선택이 취소되면 모든 ClickGuide를 활성화
        parent.CheckClickGuide(false);
    }

    protected override void ChangeOn()
    {
        base.ChangeOn();
        clickGuide.SetActive(true);
        isOn = true;
    }

    protected override void ChangeOff()
    {
        base.ChangeOff();
        clickGuide.SetActive(false);
        isOn = false;
    }
}
