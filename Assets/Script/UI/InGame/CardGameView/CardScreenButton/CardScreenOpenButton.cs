using DG.Tweening;
using UnityEngine;

public class CardScreenOpenButton : CardScreenButtonBase
{
    
    [SerializeField] private GameObject _clickGuide;
    public GameObject clickGuide => _clickGuide;


    private void Start()
    {
        CheckCardScreen();
        SetButtonCallback(PlaySequnce_SubScreenOpen);
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
        clickGuide.SetActive(isOn);
    }

}
