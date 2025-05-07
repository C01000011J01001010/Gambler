using DG.Tweening;
using UnityEngine;

public class CardScreenButton : Deactivatable_ButtonBase
{
    
    [SerializeField] private GameObject _clickGuide;
    public GameObject clickGuide => _clickGuide;


    // ĳ��
    private CardScreen _cardScreen;
    public CardScreen cardScreen
    {
        get
        {
            CheckCardScreen();
            return _cardScreen;
        }
    }
    private void CheckCardScreen()
    {
        if (_cardScreen == null)
            _cardScreen = GameManager.connector_InGame.Canvas0.CardGameView.cardScreen;
    }

    private void Start()
    {
        CheckCardScreen();
    }

    public void PlaySequnce_SubScreenOpen()
    {
        Sequence sequence = DOTween.Sequence();
        bool result = cardScreen.GetSequnce_TryCardScrrenOpen(sequence);

        if (result)
        {

            sequence.SetLoops(1);
            sequence.Play();
        }
    }

    public void PlaySequnce_SubScreenClose()
    {
        Sequence sequence = DOTween.Sequence();
        bool result = cardScreen.GetSequnce_TryCardScrrenClose(sequence);

        if (result)
        {
            sequence.SetLoops(1);
            sequence.Play();
        }
    }

    public void SetClickGuide(bool isOn)
    {
        clickGuide.SetActive(isOn);
    }

    /// <summary>
    /// �÷��̾ ���ʸ� ���������� ��� Ȱ��ȭ��
    /// </summary>
    /// <param name="isOn"></param>
    public override void SetButtonInteractable(bool isOn)
    {
        base.SetButtonInteractable(isOn);

        if(clickGuide != null) // close��ư���� ���� �� �Լ��� ȣ��Ǵ� ��츦 ���
        {
            clickGuide.SetActive(isOn);
        }
        
    }
}
