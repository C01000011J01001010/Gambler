using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using PublicSet;

public class CardGameView : MonoBehaviour
{
    // 에디터 연결
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


    // 스크립트 편집
    private Vector3 StartButtonScaleOrigin;
    private readonly string StartMessage = "게임 시작하기";
    private readonly string continueMessage = "게임 이어하기";


    // 캐싱
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

        // 인터페이스 초기화
        diceSet.returnInterface();

        // start버튼 삭제
        GetSequnce_StartButtonFadeOut(sequence);
        
        // 모든 카드를 덱의 자식객체로 전환
        sequence.AppendCallback(() => deckOfCards.ReturnAllOfCards());

        // 카드 셔플 하고 덱을 뷰의 가운데로 이동
        sequence.AppendCallback(() => deckOfCards.CardShuffle());
        sequence.AppendInterval(2f); // 카드가 중력으로 바닥에 떨어질때까지 기다림

        // 인터페이스 활성화
        diceSet.GetSequnce_InterfaceOn(sequence);

        sequence.AppendCallback(
            () =>
            {
                // progress 전환
                CardGamePlayManager.Instance.NextProgress();

                // 세팅 초기화
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
        // dotween 애니메이션 시간
        float delay = 0.5f;

        // 게임 시작 누를 시 필요없는 인터페이스 비활성화
        // 화면에서 버튼의 크기를 완전히 줄인다음 실제 크기로 복귀와 동시에 비활성화
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
        // dotween 애니메이션 시간
        float delay = 0.5f;

        sequence.AppendCallback(() => startButtonSet.SetActive(true));
        sequence.Append(startButtonSet.transform.DOScale(StartButtonScaleOrigin, delay));
        
    }

    // 백버튼 콜백
    public void ReturnCasino()
    {
        CallbackBase.CasinoViewOpen();
    }

}
