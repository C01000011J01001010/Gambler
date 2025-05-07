using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;

// �޸�Ǯ�� �̱������� ���ǵǾ������� �ش� ��ü�� �θ�ü�� �����ϱ⿡ �� �̵��� �ı���
public class CardButtonMemoryPool : MemoryPool_Stack<CardButtonMemoryPool>
{
    // ������ ����
    public PlayerMe playerMe;

    // ��ũ��Ʈ ����
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

    // ���ÿ� �ݴ�� �־ ������ 1������ �������� ����
    protected override void CreateNewObject(int cardNumber)
    {
        GameObject obj = Instantiate(prefab);
        if (obj != null)
        {
            obj.name = $"{cardNumber+1}�� ��ư";
            // �޸�Ǯ�� ����� �޸�Ǯ �ȿ��� ������
            // �޸�Ǯ�� donDestroy�̸� ��ü�� donDestroy
            obj.transform.SetParent(transform, false);
            obj.transform.SetSiblingIndex(0);

            CardSelectButton Buttonscript = obj.GetComponent<CardSelectButton>();
            if(Buttonscript != null)
            {
                // �̹��� ����
                Buttonscript.SetCardButtonImage(cardNumber);
            }

            obj.SetActive(false);

            if (memoryPool != null)
            {
                memoryPool.Push(obj);
            }
            else
            {
                Debug.LogError("�޸�Ǯ(Queue) �ʱ�ȭ �ȵ���");
            }
        }
        else
        {
            Debug.LogError("�������� ����");
        }
    }

    public void InitCardButton(Transform cardsParent)
    {
        Debug.Log("InitCardButton ����");

        // ��� ��ư�� ����ֱ�(0�� �ڽĺ��� ������ ���� �ݴ� ������ �������)
        for (int i = cardSelectButtonList.Count - 1; i >= 0; i--)
        {
            ReturnObject(cardSelectButtonList[i].gameObject);
        }
        cardSelectButtonList.Clear(); // ��ư�� �����ϴ� ����Ʈ�� �ʱ�ȭ

        int cardCount = cardsParent.childCount;

        
        for (int i = 0; i < cardCount; i++)
        {
            Transform card = cardsParent.GetChild(i);

            // �θ� ��Ŀ�� �߽����� ��ġ ����
            float offset = (cardCount % 2 == 0) ? (i - cardCount / 2 + 0.5f) : (i - cardCount / 2);
            Vector2 AnchoredPos = Vector2.zero;

            AnchoredPos.x += offset * buttonRectTrans.rect.size.x * 1.1f;
            AnchoredPos.y += -buttonRectTrans.rect.size.y;

            // ī���ư�� ������ ��ġ�� Ȱ��ȭ
            // CardSelectButton�� Enable�� ���� ��ư�� �׻� ON color��
            GameObject obj = GetObject();
            obj.GetComponent<RectTransform>().anchoredPosition = AnchoredPos;

            Debug.Log($"{i + 1}�� ī���ư ����");

            // ��ư�� ī�� ����
            CardSelectButton Buttonscript = obj.GetComponent<CardSelectButton>();
            Buttonscript.MappingButtonWithCard(card.gameObject);

            // ��ư�� �ݹ��� ����
            if (CardGamePlayManager.Instance.currentProgress == eOOLProgress.num102_BeforeRotateDiceAndDistribution)
            {
                Debug.Log("ī�� ���ù�ư�� OnStartTime �ݹ��� �����");
                Buttonscript.SetButtonCallback(Buttonscript.SelectThisCard_OnGameSetting);
            }
            else
            {
                Debug.Log("ī�� ���ù�ư�� OnPlayTime �ݹ��� �����");
                Buttonscript.SetButtonCallback(Buttonscript.SelectThisCard_OnPlayTime);
            }

            // ��ư�� ����Ʈ�� �߰�
            cardSelectButtonList.Add(Buttonscript);
        }

        // ī�� ��ư�� ���� ���� Ȱ����� ����
        // ��� �÷��̾����� ī���� �й谡 �Ϸ�� �� Ȱ��
        SetAllButtonInteractable(false);

    }

    /// <summary>
    /// ī�� �й谡 �Ϸ�ǰ� ���� ������ �����ϸ� Ŭ�����̵� Ȱ��ȭ�ϵ��� ��
    /// </summary>
    /// <param name="isOn"></param>
    public void SetAllButtonInteractable(bool isOn)
    {
        foreach (CardSelectButton cardButton in cardSelectButtonList)
        {
            cardButton.SetButtonInteractable(isOn);
            cardButton.clickGuide.SetActive(isOn);
        }
    }


    /// <summary>
    /// ������ �Ϸ�Ǹ� �� ������ <br/>
    /// �׷��� ������ ���� ������ ī�常 ����
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
            cardButton.clickGuide.SetActive(false);
        }
    }

    public void ActiveClickGuide()
    {
        foreach (CardSelectButton cardButton in cardSelectButtonList)
        {
            if(cardButton.isOn)
                cardButton.clickGuide.SetActive(true);
        }
    }
}
