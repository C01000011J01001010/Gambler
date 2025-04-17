using DG.Tweening;
using PublicSet;
using UnityEngine;

public class CardScreenBackGround : MonoBehaviour
{
    public RectTransform canvas;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CardScreenButton cardScreenButton;
    
    bool isCardScreenInCenter;
    Vector2 OutOfScreenAnchoredPos;
    Vector2 CenterAnchoredPos;
    Vector2 rectSize;

    private void Awake()
    {
        SetPos();
    }

    private void SetPos()
    {
        rectSize = rectTransform.rect.size;
        isCardScreenInCenter = false;
        CenterAnchoredPos = Vector2.zero;
        OutOfScreenAnchoredPos = Vector2.zero;

        OutOfScreenAnchoredPos.y -= (canvas.rect.size.y / 2 + rectSize.y);
    }

    private void OnRectTransformDimensionsChange()
    {
        if (rectSize != rectTransform.rect.size)
            SetPos();
    }

    /// <summary>
    /// 버튼 콜백 재설정 포함
    /// </summary>
    /// <param name="sequence"></param>
    /// <returns></returns>
    public bool GetSequnce_TryCardScrrenOpen(Sequence sequence)
    {
        float delay = 1.0f;

        // 스크린을 센터로 불러오는 경우
        if (isCardScreenInCenter == false)
        {
            isCardScreenInCenter = true;
            sequence.Append(rectTransform.DOAnchorPos(CenterAnchoredPos, delay));

            // 화면이 올라오면 ui사용방법을 설명
            sequence.AppendCallback(
                () => GameManager.connector_InGame.textWindowView_Script.StartTextWindow(eSystemGuide.HowToCardSelect)
                );

            cardScreenButton.SetButtonCallback(cardScreenButton.PlaySequnce_SubScreenClose);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 버튼 콜백 재설정 포함
    /// </summary>
    /// <param name="sequence"></param>
    /// <returns></returns>
    public bool GetSequnce_TryCardScrrenClose(Sequence sequence)
    {
        Debug.Log("GetSequnce_CardScrrenClose 실행");
        float delay = 1.0f;

        // 스크린을 밖으로 빼는 경우
        if (isCardScreenInCenter)
        {
            isCardScreenInCenter = false;
            sequence.Append(rectTransform.DOAnchorPos(OutOfScreenAnchoredPos, delay));
            cardScreenButton.SetButtonCallback(cardScreenButton.PlaySequnce_SubScreenOpen);
            return true;
        }

        return false;
    }
}
