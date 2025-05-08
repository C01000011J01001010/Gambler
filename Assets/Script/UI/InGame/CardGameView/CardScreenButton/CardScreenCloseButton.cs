using DG.Tweening;
using UnityEngine;

public class CardScreenCloseButton : CardScreenButtonBase
{

    private void Start()
    {
        CheckCardScreen();
        SetButtonCallback(PlaySequnce_SubScreenClose);
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
