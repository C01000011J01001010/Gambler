using DG.Tweening;
using PublicSet;
using UnityEngine;

public class CardScreen : MonoBehaviour
{
    [SerializeField] private SelectCompleteButton _selectCompleteButton;
    [SerializeField] private CardButtonMemoryPool _cardButtonMemoryPool;

    bool isAfterAwake;
    bool isCardScreenInCenter;
    Vector2 OutOfScreenAnchoredPos;
    Vector2 CenterAnchoredPos;
    Vector2 rectSize;

    public SelectCompleteButton selectCompleteButton => _selectCompleteButton;
    public CardButtonMemoryPool cardButtonMemoryPool => _cardButtonMemoryPool;

    // ĳ��
    //[SerializeField]
    private RectTransform _canvas;
    private RectTransform _rectTransform;
    private CardScreenOpenButton _cardScreenOpenButton;

    public RectTransform canvas
    {
        get
        {
            CheckCanvas();
            return _canvas;
        }
    }
    public RectTransform rectTransform
    {
        get
        {
            CheckRectTransform();
            return _rectTransform;
        }
    }
    public CardScreenOpenButton cardScreenOpenButton
    {
        get
        {
            CheckCardScreenOpenButton();
            return _cardScreenOpenButton;
        }
    }

    private void CheckCanvas()
    {
        if (_canvas == null)
            _canvas = GameManager.connector_InGame.Canvas0.GetComponent<RectTransform>();
    }
    private void CheckRectTransform()
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();
    }
    private void CheckCardScreenOpenButton()
    {
        if (_cardScreenOpenButton == null)
            _cardScreenOpenButton = GameManager.connector_InGame.Canvas0.CardGameView.cardScreenButtonSet.openButton;
    }

    private void Awake()
    {
        isAfterAwake = true;
    }

    private void Start()
    {
        CheckCanvas();
        CheckRectTransform();
        CheckCardScreenOpenButton();
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
        if (isAfterAwake && rectSize != rectTransform.rect.size)
            SetPos();
    }

    public void OpenButtonCheckClickGuide()
    {
        if(selectCompleteButton.clickGuide.activeSelf) 
        {
            cardScreenOpenButton.SetClickGuide(true);
            return;
        }
        else
        {
            foreach(CardSelectButton cardSelectButton in cardButtonMemoryPool.cardSelectButtonList)
            {
                if(cardSelectButton.clickGuide.activeSelf)
                {
                    cardScreenOpenButton.SetClickGuide(true);
                    return;
                }
            }
        }

        // Ȯ���� ��ư�� ������ ���¹�ư�� Ŭ�����̵带 ����
        cardScreenOpenButton.SetClickGuide(false);
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
            //sequence.AppendCallback(
            //    () => GameManager.connector_InGame.textWindowView_Script.StartTextWindow(eSystemGuide.HowToCardSelect)
            //    );

            cardScreenOpenButton.gameObject.SetActive(false);
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

            cardScreenOpenButton.gameObject.SetActive(true);
            return true;
        }

        return false;
    }
}
