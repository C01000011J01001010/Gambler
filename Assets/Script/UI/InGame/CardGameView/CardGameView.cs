using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CardGameView : MonoBehaviour
{
    // 에디터 연결
    public PlayerInterface_CardGame playerInterface;
    public CardGamePlayManager cardGamePlayManager;
    public CardScreenBackGround cardScreenBackGround;
    public DiceButton diceButton;
    public SelectCompleteButton selectCompleteButton;
    public CasinoView casinoView;
    public GameObject cardScreenButtonSet;
    public CardScreenButton cardScreenOpenButton;
    public GameObject SubScreen_Card;
    public GameObject StartButtonSet;
    public Text startButtonText;
    public DeckOfCards deckOfCards;
    public TargetDisplay targetImageDisplay;

    // 스크립트 편집
    private Vector3 StartButtonScaleOrigin;
    private readonly string StartMessage = "게임 시작하기";
    private readonly string continueMessage = "게임 이어하기";

    public void InitAttribute()
    {
        selectCompleteButton.InitAttribute();
        diceButton.TryActivate_Button();

        InitContinueButtonText();
    }

    private void Awake()
    {
        

        if (cardGamePlayManager == null)
            Debug.LogAssertion($"cardGamePlayManager == null ");

        StartButtonScaleOrigin = StartButtonSet.transform.localScale;
    }

    private void OnEnable()
    {
        cardGamePlayManager.EnterCardGame();
    }

    private void OnDisable()
    {
        casinoView.onlyOneLivesButton.SetPlayerCantPlayThis();
    }


    public void StartGame()
    {
        Sequence sequence = DOTween.Sequence();

        // 인터페이스 초기화
        playerInterface.returnInterface();

        // start버튼 삭제
        GetSequnce_StartButtonFadeOut(sequence);
        
        // 모든 카드를 덱의 자식객체로 전환
        sequence.AppendCallback(() => deckOfCards.ReturnAllOfCards());

        // 카드 셔플 하고 덱을 뷰의 가운데로 이동
        sequence.AppendCallback(() => deckOfCards.CardShuffle());
        sequence.AppendInterval(2f); // 카드가 중력으로 바닥에 떨어질때까지 기다림

        // 인터페이스 활성화
        playerInterface.GetSequnce_InterfaceOn(sequence);

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
        StartButtonSet.transform.localScale = Vector3.zero;
        StartButtonSet.gameObject.SetActive(false);
    }
    
    public void GetSequnce_StartButtonFadeOut(Sequence sequence)
    {
        // dotween 애니메이션 시간
        float delay = 0.5f;

        // 게임 시작 누를 시 필요없는 인터페이스 비활성화
        // 화면에서 버튼의 크기를 완전히 줄인다음 실제 크기로 복귀와 동시에 비활성화
        sequence.Append(StartButtonSet.transform.DOScale(Vector3.zero, delay));
        sequence.AppendCallback(() => StartButtonSet.SetActive(false));
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

        sequence.AppendCallback(() => StartButtonSet.SetActive(true));
        sequence.Append(StartButtonSet.transform.DOScale(StartButtonScaleOrigin, delay));
        
    }

    // 백버튼 콜백
    public void ReturnCasino()
    {
        CallbackBase.CasinoViewOpen();
    }

}
