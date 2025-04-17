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
    /// ��ư �ݹ� �缳�� ����
    /// </summary>
    /// <param name="sequence"></param>
    /// <returns></returns>
    public bool GetSequnce_TryCardScrrenOpen(Sequence sequence)
    {
        float delay = 1.0f;

        // ��ũ���� ���ͷ� �ҷ����� ���
        if (isCardScreenInCenter == false)
        {
            isCardScreenInCenter = true;
            sequence.Append(rectTransform.DOAnchorPos(CenterAnchoredPos, delay));

            // ȭ���� �ö���� ui������� ����
            sequence.AppendCallback(
                () => GameManager.connector_InGame.textWindowView_Script.StartTextWindow(eSystemGuide.HowToCardSelect)
                );

            cardScreenButton.SetButtonCallback(cardScreenButton.PlaySequnce_SubScreenClose);
            return true;
        }

        return false;
    }

    /// <summary>
    /// ��ư �ݹ� �缳�� ����
    /// </summary>
    /// <param name="sequence"></param>
    /// <returns></returns>
    public bool GetSequnce_TryCardScrrenClose(Sequence sequence)
    {
        Debug.Log("GetSequnce_CardScrrenClose ����");
        float delay = 1.0f;

        // ��ũ���� ������ ���� ���
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
