using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CardGameView : MonoBehaviour
{
    // ������ ����
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

    // ��ũ��Ʈ ����
    private Vector3 StartButtonScaleOrigin;
    private readonly string StartMessage = "���� �����ϱ�";
    private readonly string continueMessage = "���� �̾��ϱ�";

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

        // �������̽� �ʱ�ȭ
        playerInterface.returnInterface();

        // start��ư ����
        GetSequnce_StartButtonFadeOut(sequence);
        
        // ��� ī�带 ���� �ڽİ�ü�� ��ȯ
        sequence.AppendCallback(() => deckOfCards.ReturnAllOfCards());

        // ī�� ���� �ϰ� ���� ���� ����� �̵�
        sequence.AppendCallback(() => deckOfCards.CardShuffle());
        sequence.AppendInterval(2f); // ī�尡 �߷����� �ٴڿ� ������������ ��ٸ�

        // �������̽� Ȱ��ȭ
        playerInterface.GetSequnce_InterfaceOn(sequence);

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
        StartButtonSet.transform.localScale = Vector3.zero;
        StartButtonSet.gameObject.SetActive(false);
    }
    
    public void GetSequnce_StartButtonFadeOut(Sequence sequence)
    {
        // dotween �ִϸ��̼� �ð�
        float delay = 0.5f;

        // ���� ���� ���� �� �ʿ���� �������̽� ��Ȱ��ȭ
        // ȭ�鿡�� ��ư�� ũ�⸦ ������ ���δ��� ���� ũ��� ���Ϳ� ���ÿ� ��Ȱ��ȭ
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
        // dotween �ִϸ��̼� �ð�
        float delay = 0.5f;

        sequence.AppendCallback(() => StartButtonSet.SetActive(true));
        sequence.Append(StartButtonSet.transform.DOScale(StartButtonScaleOrigin, delay));
        
    }

    // ���ư �ݹ�
    public void ReturnCasino()
    {
        CallbackBase.CasinoViewOpen();
    }

}
