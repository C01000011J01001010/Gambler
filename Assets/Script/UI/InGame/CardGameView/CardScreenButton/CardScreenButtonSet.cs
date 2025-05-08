using DG.Tweening;
using UnityEngine;

public class CardScreenButtonSet : MonoBehaviour
{
    [SerializeField] private CardScreenCloseButton _closeButton;
    [SerializeField] private CardScreenOpenButton _openButton;
    public CardScreenCloseButton closeButton => _closeButton;
    public CardScreenOpenButton openButton => _openButton;



    //private RectTransform _rectTrans;
    //public RectTransform rectTrans
    //{
    //    get
    //    {
    //        if (_rectTrans == null) _rectTrans = GetComponent<RectTransform>();
    //        return _rectTrans;
    //    }
    //}

    public float fadeDuration = 0.5f;

    public void Init()
    {
        gameObject.SetActive(false);
    }

    // ���̵� ��
    public void GetSequence_FadeIn(Sequence sequence)
    {
        openButton.TryActivate_Button(); // ��ư Ȱ��ȭ

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

    // ���̵� �ƿ�
    public void GetSequence_FadeOut(Sequence sequence)
    {
        openButton.TryDeactivate_Button(); // ��ư ��Ȱ��ȭ

        MethodManager.GetSequence_FadeOut(sequence, gameObject, fadeDuration);
    }

    public void PlaySequence_FadeOut()
    {
        Sequence sequence = MethodManager.GetNewSequnce_SingleUse(gameObject);
        GetSequence_FadeOut(sequence);
        sequence.Play();
    }
}
