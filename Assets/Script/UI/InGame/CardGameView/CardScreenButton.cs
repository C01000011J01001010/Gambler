using DG.Tweening;
using UnityEngine;

public class CardScreenButton : Deactivatable_ButtonBase
{
    [SerializeField] CardScreenBackGround cardScreen;

    private void Start()
    {
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
}
