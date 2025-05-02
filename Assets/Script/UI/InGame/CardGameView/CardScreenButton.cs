using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardScreenButton : Deactivatable_ButtonBase
{
    [SerializeField] CardScreenBackGround cardScreen;


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
