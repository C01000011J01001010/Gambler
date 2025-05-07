using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using PublicSet;

public class CardGameView : MonoBehaviour
{
    // ������ ����
    [SerializeField] private DiceSet _diceSet;
    [SerializeField] private GameObject _startButtonSet;
    [SerializeField] private Text _startButtonText;
    [SerializeField] private TargetDisplay _targetDisplay;
    [SerializeField] private CardScreen _cardScreen;
    [SerializeField] private CardScreenButtonSet _cardScreenButtonSet;


    public DiceSet diceSet => _diceSet;
    public GameObject startButtonSet => _startButtonSet;
    public Text startButtonText => _startButtonText;
    public TargetDisplay targetDisplay => _targetDisplay;
    public CardScreen cardScreen => _cardScreen;
    public CardScreenButtonSet cardScreenButtonSet => _cardScreenButtonSet;


    // ��ũ��Ʈ ����
    private Vector3 StartButtonScaleOrigin;
    private readonly string StartMessage = "���� �����ϱ�";
    private readonly string continueMessage = "���� �̾��ϱ�";


    // ĳ��
    private CardGamePlayManager _cardGamePlayManager;
    private CasinoView _casinoView;
    private CardScreenButton _cardScreenOpenButton;
    private DiceButton _diceButton;
    private SelectCompleteButton _selectCompleteButton;
    private DeckOfCards _deckOfCards;

    public CardGamePlayManager cardGamePlayManager
    {
        get
        {
            CheckCardGamePlayManager();
            return _cardGamePlayManager;
        }
    }
    public CasinoView casinoView
    {
        get
        {
            CheckCasinoView();
            return _casinoView;
        }
    }
    public CardScreenButton cardScreenOpenButton
    {
        get
        {
            CheckCardScreenOpenButton();
            return _cardScreenOpenButton;
        }
    }
    public DiceButton diceButton
    {
        get
        {
            CheckDiceButton();
            return _diceButton;
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
    public DeckOfCards deckOfCards
    {
        get
        {
            CheckDeckOfCards();
            return _deckOfCards;
        }
    }

    private void CheckCardGamePlayManager()
    {
        if (_cardGamePlayManager == null)
            _cardGamePlayManager = CardGamePlayManager.Instance;
    }
    private void CheckCasinoView()
    {
        if (_casinoView == null)
            _casinoView = GameManager.connector_InGame.Canvas0.CasinoView;
    }
    private void CheckCardScreenOpenButton()
    {
        if (_cardScreenOpenButton == null)
            _cardScreenOpenButton = cardScreenButtonSet.openButton;
    }
    private void CheckDiceButton()
    {
        if (_diceButton == null)
            _diceButton = diceSet.diceButton;
    }
    private void CheckSelectCompleteButton()
    {
        if (_selectCompleteButton == null)
            _selectCompleteButton = cardScreen.selectCompleteButton;
    }
    private void CheckDeckOfCards()
    {
        if (_deckOfCards == null)
            _deckOfCards = cardGamePlayManager.deckOfCards;
    }

    private void Awake()
    {
        

        if (cardGamePlayManager == null)
            Debug.LogAssertion($"cardGamePlayManager == null ");

        StartButtonScaleOrigin = startButtonSet.transform.localScale;
    }

    private void Start()
    {
        CheckCardGamePlayManager();
        CheckCasinoView();
        CheckCardScreenOpenButton();
        CheckDiceButton();
        CheckSelectCompleteButton();
        CheckDeckOfCards();
    }

    public void InitAttribute()
    {
        selectCompleteButton.InitAttribute();
        diceButton.TryActivate_Button();

        InitContinueButtonText();
    }


    private void OnDisable()
    {
        casinoView.onlyOneLivesButton.SetPlayerCantPlayThis();
        QuestManager.Instance.TryPlayerGetQuest(eQuestType.GoToSleep);
    }


    public void StartGame()
    {
        Sequence sequence = DOTween.Sequence();

        // �������̽� �ʱ�ȭ
        diceSet.returnInterface();

        // start��ư ����
        GetSequnce_StartButtonFadeOut(sequence);
        
        // ��� ī�带 ���� �ڽİ�ü�� ��ȯ
        sequence.AppendCallback(() => deckOfCards.ReturnAllOfCards());

        // ī�� ���� �ϰ� ���� ���� ����� �̵�
        sequence.AppendCallback(() => deckOfCards.CardShuffle());
        sequence.AppendInterval(2f); // ī�尡 �߷����� �ٴڿ� ������������ ��ٸ�

        // �������̽� Ȱ��ȭ
        diceSet.GetSequnce_InterfaceOn(sequence);

        sequence.AppendCallback(
            () =>
            {
                // progress ��ȯ
                CardGamePlayManager.Instance.NextProgress();

                // ���� �ʱ�ȭ
                CardGamePlayManager.Instance.InitCurrentGame();
                diceButton.TryActivate_Button();
                
            });
        
        sequence.SetLoops(1);
        sequence.Play();
    }

    public void InitStartButton()
    {
        startButtonSet.transform.localScale = Vector3.zero;
        startButtonSet.gameObject.SetActive(false);
    }
    
    public void GetSequnce_StartButtonFadeOut(Sequence sequence)
    {
        // dotween �ִϸ��̼� �ð�
        float delay = 0.5f;

        // ���� ���� ���� �� �ʿ���� �������̽� ��Ȱ��ȭ
        // ȭ�鿡�� ��ư�� ũ�⸦ ������ ���δ��� ���� ũ��� ���Ϳ� ���ÿ� ��Ȱ��ȭ
        sequence.Append(startButtonSet.transform.DOScale(Vector3.zero, delay));
        sequence.AppendCallback(() => startButtonSet.SetActive(false));
    }

    public void InitStartButtonText()
    {
        startButtonText.text = StartMessage;
    }
    public void InitContinueButtonText()
    {
        startButtonText.text = continueMessage;
    }

    public void PlaySequnce_StartButtonFadeIn()
    {
        Sequence sequence = DOTween.Sequence();
        GetSequnce_StartButtonFadeIn(sequence);
        sequence.SetLoops(1);
        sequence.Play();
    }
    public void GetSequnce_StartButtonFadeIn(Sequence sequence)
    {
        // dotween �ִϸ��̼� �ð�
        float delay = 0.5f;

        sequence.AppendCallback(() => startButtonSet.SetActive(true));
        sequence.Append(startButtonSet.transform.DOScale(StartButtonScaleOrigin, delay));
        
    }

    // ���ư �ݹ�
    public void ReturnCasino()
    {
        CallbackBase.CasinoViewOpen();
    }

}
