using DG.Tweening;
using UnityEngine;

public class CardScreenButtonSet : MonoBehaviour
{
    
    

    [SerializeField] private CardScreenButton _closeButton;
    [SerializeField] private CardScreenButton _openButton;
    private RectTransform _rectTrans;

    public CardScreenButton closeButton => _closeButton;
    public CardScreenButton openButton => _openButton;
    public RectTransform rectTrans
    {
        get
        {
            if (_rectTrans == null) _rectTrans = GetComponent<RectTransform>();
            return _rectTrans;
        }
    }



    public float fadeDuration = 0.5f;

    public void Init()
    {
        gameObject.SetActive(false);
    }

    // 페이드 인
    public void GetSequence_FadeIn(Sequence sequence)
    {
        openButton.TryActivate_Button(); // 버튼 활성화

        MethodManager.GetSequence_FadeIn(sequence, gameObject, fadeDuration);
        //sequence.AppendCallback(() => gameObject.SetActive(true));
        //sequence.AppendCallback(() => transform.localScale = Vector3.zero);
        //sequence.Append(transform.DOScale(Vector3.one, fadeDuration).SetEase(Ease.OutBack));
        
    }

    public void PlaySequence_FadeIn()
    {
        Sequence sequence = MethodManager.GetNewSequnce_SingleUse(gameObject);
        GetSequence_FadeIn(sequence);
        sequence.Play();
    }

    // 페이드 아웃
    public void GetSequence_FadeOut(Sequence sequence)
    {
        openButton.TryDeactivate_Button(); // 버튼 비활성화

        MethodManager.GetSequence_FadeOut(sequence, gameObject, fadeDuration);
        //sequence.AppendCallback(() => transform.localScale = Vector3.one);
        //sequence.Append(transform.DOScale(Vector3.zero, fadeDuration).SetEase(Ease.InBack));
        //sequence.AppendCallback(() => gameObject.SetActive(false));
    }

    public void PlaySequence_FadeOut()
    {
        Sequence sequence = MethodManager.GetNewSequnce_SingleUse(gameObject);
        GetSequence_FadeOut(sequence);
        sequence.Play();
    }
}
