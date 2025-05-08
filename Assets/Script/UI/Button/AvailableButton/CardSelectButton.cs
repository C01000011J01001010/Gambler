using PublicSet;
using UnityEngine;

public class CardSelectButton : ImageChange_ButtonBase
{
    // ������ ����
    public Sprite[] sprites;

    [SerializeField] GameObject _clickGuide;


    // ��ũ��Ʈ ����
    public GameObject clickGuide => _clickGuide;

    public bool isOn {  get; private set; }
    public GameObject ButtonToCard {  get; private set; }
    public TrumpCardDefault trumpCardScript { get; private set; }

    



    // ĳ��
    private CardScreen _cardScreen;
    private CardButtonMemoryPool _cardButtonMemoryPool;
    private SelectCompleteButton _selectCompleteButton;

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
    public SelectCompleteButton selectCompleteButton
    {
        get
        {
            CheckSelectCompleteButton();
            return _selectCompleteButton;
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
    public void CheckSelectCompleteButton()
    {
        if (_selectCompleteButton == null)
            _selectCompleteButton = cardScreen.selectCompleteButton;
    }

    private void Start()
    {
        CheckCardScreen();
        CheckCardButtonMemoryPool();
        CheckSelectCompleteButton();
        SetDisabledColorAlpha_1();
    }

    // �޸�Ǯ���� ������������ ����
    private void OnEnable()
    {
        ChangeOn();
    }

    public void SetCardButtonImage(int orderInHnad)
    {
        //Debug.Log($"{orderInHnad}�� ī���� ��ư ��������");
        if(image != null)
        {
            if(orderInHnad < sprites.Length)
            {
                image.sprite = sprites[orderInHnad];
            }
            else
            {
                Debug.LogAssertion("sprite Ȯ�� �ʿ�");
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
            Debug.LogAssertion($"{card.name}�� card�� �ƴ�");
        }
        
    }


    private void CheckProperties()
    {
        if (ButtonToCard == null)
        {
            Debug.LogAssertion($"{ButtonToCard.gameObject.name}�� trumpCardScript == null");
            return;
        }
        if (trumpCardScript == null)
        {
            Debug.LogAssertion("ButtonToCard == null");
            return;
        }
    }


    public void SelectThisCard_OnGameSetting()
    {
        CheckProperties();

        if (cardButtonMemoryPool != null)
        {
            if (trumpCardScript.TrySelectThisCard_OnGameSetting(cardButtonMemoryPool.playerMe))
            {
                // ��ư ��ȯ
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
            Debug.LogAssertion($"{transform.parent.gameObject.name}�� CardButtonSet == null");
        }
    }
    public void UnselectThisCard_OnGameSetting()
    {
        CheckProperties();

        if (cardButtonMemoryPool != null)
        {
            trumpCardScript.UnselectThisCard_OnStartTime(cardButtonMemoryPool.playerMe);

            // ��ư ��ȯ
            ChangeOn();
            SetButtonCallback(SelectThisCard_OnGameSetting);
        }
        else
        {
            Debug.LogAssertion($"{transform.parent.gameObject.name}�� CardButtonSet == null");
        }

    }

    public void SelectThisCard_OnPlayTime()
    {
        CheckProperties();

        if (trumpCardScript.TrySelectThisCard_OnPlayTime(cardButtonMemoryPool.playerMe))
        {
            // ��ư ��ȯ
            ChangeOff();
            SetButtonCallback(UnselectThisCard_OnPlayTime);
            selectCompleteButton.TryActivate_Button();
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

        trumpCardScript.UnselectThisCard_OnPlayTime(cardButtonMemoryPool.playerMe);
        // ��ư ��ȯ
        ChangeOn();
        SetButtonCallback(SelectThisCard_OnPlayTime);
        selectCompleteButton.TryDeactivate_Button();

        // ������ ��ҵǸ� ��� ClickGuide�� Ȱ��ȭ
        cardButtonMemoryPool.CheckClickGuide(false);
    }

    public void ClickGuideSetActive(bool isOn)
    {
        clickGuide.SetActive(isOn);
        cardScreen.OpenButtonCheckClickGuide();
    }

    protected override void ChangeOn()
    {
        base.ChangeOn();
        ClickGuideSetActive(true);
        isOn = true;
    }

    protected override void ChangeOff()
    {
        base.ChangeOff();
        ClickGuideSetActive(false);
        isOn = false;
    }

    public override void SetButtonInteractable(bool isOn)
    {
        base.SetButtonInteractable(isOn);
        ClickGuideSetActive(isOn);
    }
}
