using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;

// 메모리풀이 싱글톤으로 정의되어있지만 해당 객체는 부모객체가 존재하기에 씬 이동시 파괴됨
public class CardButtonMemoryPool : MemoryPool_Stack<CardButtonMemoryPool>
{
    // 에디터 연결
    public PlayerMe playerMe;

    // 스크립트 편집
    protected RectTransform buttonRectTrans {  get; private set; }
    public List<CardSelectButton> cardSelectButtonList {  get; private set; }

    protected override void Awake()
    {
        base.Awake();
        buttonRectTrans = prefab.GetComponent<RectTransform>();
        cardSelectButtonList = new List<CardSelectButton>();
        InitializePool(6);
    }

    public override void InitializePool(int size)
    {
        if (memoryPool == null) memoryPool = new Stack<GameObject>();
        else memoryPool.Clear();

        for (int i = size-1 ; i>=0 ; i--)
            CreateNewObject(i);
    }

    // 스택에 반대로 넣어서 꺼낼때 1번부터 나오도록 만듬
    protected override void CreateNewObject(int cardNumber)
    {
        GameObject obj = Instantiate(prefab);
        if (obj != null)
        {
            obj.name = $"{cardNumber+1}번 버튼";
            // 메모리풀의 대상을 메모리풀 안에서 관리함
            // 메모리풀이 donDestroy이면 객체도 donDestroy
            obj.transform.SetParent(transform, false);
            obj.transform.SetSiblingIndex(0);

            CardSelectButton Buttonscript = obj.GetComponent<CardSelectButton>();
            if(Buttonscript != null)
            {
                // 이미지 설정
                Buttonscript.SetCardButtonImage(cardNumber);
            }

            obj.SetActive(false);

            if (memoryPool != null)
            {
                memoryPool.Push(obj);
            }
            else
            {
                Debug.LogError("메모리풀(Queue) 초기화 안됐음");
            }
        }
        else
        {
            Debug.LogError("프리팹이 없음");
        }
    }

    public void InitCardButton(Transform cardsParent)
    {
        Debug.Log("InitCardButton 실행");

        // 모든 버튼을 집어넣기(0번 자식부터 꺼내기 위해 반대 순서로 집어넣음)
        for (int i = cardSelectButtonList.Count - 1; i >= 0; i--)
        {
            ReturnObject(cardSelectButtonList[i].gameObject);
        }
        cardSelectButtonList.Clear(); // 버튼을 참조하던 리스트도 초기화

        int cardCount = cardsParent.childCount;

        
        for (int i = 0; i < cardCount; i++)
        {
            Transform card = cardsParent.GetChild(i);

            // 부모 앵커를 중심으로 위치 설정
            float offset = (cardCount % 2 == 0) ? (i - cardCount / 2 + 0.5f) : (i - cardCount / 2);
            Vector2 AnchoredPos = Vector2.zero;

            AnchoredPos.x += offset * buttonRectTrans.rect.size.x * 1.1f;
            AnchoredPos.y += -buttonRectTrans.rect.size.y;

            // 카드버튼을 정해진 위치에 활성화
            // CardSelectButton의 Enable에 의해 버튼은 항상 ON color임
            GameObject obj = GetObject();
            obj.GetComponent<RectTransform>().anchoredPosition = AnchoredPos;

            Debug.Log($"{i + 1}번 카드버튼 생성");

            // 버튼과 카드 연결
            CardSelectButton Buttonscript = obj.GetComponent<CardSelectButton>();
            Buttonscript.MappingButtonWithCard(card.gameObject);

            // 버튼의 콜백을 설정
            if (CardGamePlayManager.Instance.currentProgress == eOOLProgress.num102_BeforeRotateDiceAndDistribution)
            {
                Debug.Log("카드 선택버튼에 OnStartTime 콜백이 연결됨");
                Buttonscript.SetButtonCallback(Buttonscript.SelectThisCard_OnGameSetting);
            }
            else
            {
                Debug.Log("카드 선택버튼에 OnPlayTime 콜백이 연결됨");
                Buttonscript.SetButtonCallback(Buttonscript.SelectThisCard_OnPlayTime);
            }

            // 버튼을 리스트에 추가
            cardSelectButtonList.Add(Buttonscript);
        }

        // 실제로 사용할때까지 잠금
        SetAllButtonInteractable(false);
    }

    /// <summary>
    /// 카드 분배가 완료되고 게임 세팅을 시작하면 클릭가이드 활성화하도록 함
    /// </summary>
    /// <param name="isOn"></param>
    public void SetAllButtonInteractable(bool isOn)
    {
        foreach (CardSelectButton cardButton in cardSelectButtonList)
        {
            cardButton.SetButtonInteractable(isOn);
        }
    }


    /// <summary>
    /// 선택이 완료되면 다 꺼지고 <br/>
    /// 그렇지 않으면 선택 가능한 카드만 켜짐
    /// </summary>
    /// <param name="SelectDone"></param>
    public void CheckClickGuide(bool SelectDone)
    {
        if (SelectDone) ClearClickGuide();
        else ActiveClickGuide();
    }

    public void ClearClickGuide()
    {
        foreach(CardSelectButton cardButton in cardSelectButtonList)
        {
            cardButton.ClickGuideSetActive(false);
        }
    }

    public void ActiveClickGuide()
    {
        foreach (CardSelectButton cardButton in cardSelectButtonList)
        {
            if (cardButton.isOn)
                cardButton.ClickGuideSetActive(true);
        }
    }
}
