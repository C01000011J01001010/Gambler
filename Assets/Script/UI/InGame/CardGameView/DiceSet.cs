using UnityEngine;
using DG.Tweening;
public class DiceSet : MonoBehaviour
{
    //������
    //public CardScreenButton cardScreenButton;
    public DiceButton diceButton;
    public GameObject SubScreen_Dice;

    //��ũ��Ʈ
    private RectTransform _rectTransform;
    private Vector2 InScreenAnchoredPos;
    private Vector2 OutOfScreenAnchoredPos;
    private Vector2 rectSize;
    float delay = 1.0f;

    public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }


    private void Awake()
    {
        SetPos();
    }

    private void Start()
    {
        if (diceButton == null)
        {
            Debug.LogAssertion("diceButton == null");
        }

        if (SubScreen_Dice == null)
        {
            Debug.LogAssertion("SubScreen_Dice == null");
        }
    }

    private void SetPos()
    {
        // ���� ��ġ�� ���ο����� ��ġ�� ����
        InScreenAnchoredPos = rectTransform.anchoredPosition;

        // �ڽ��� x�ุŭ �̵��Ͽ� ȭ�鿡�� ������ ��������� ��ǥ�� ����
        rectSize = rectTransform.rect.size;
        OutOfScreenAnchoredPos = rectTransform.anchoredPosition;
        OutOfScreenAnchoredPos.x = rectSize.x;
    }

    private void OnRectTransformDimensionsChange()
    {
        if (rectSize != rectTransform.rect.size) 
            SetPos();
    }

    private void DiceSetAcive(bool value)
    {
        diceButton.gameObject.SetActive(value);
        SubScreen_Dice.SetActive(value);
    }

    public void Init()
    {
        // Ȱ��ȭ�Ǹ� �ۿ��� ���
        rectTransform.anchoredPosition = OutOfScreenAnchoredPos;

        diceButton.TryDeactivate_Button();

        // content�� Ȱ��ȭ�� ��ü �ʱ�ȭ
        DiceSetAcive(true);
    }

    public void returnInterface()
    {
        diceButton.TryDeactivate_Button();

        Sequence sequence = DOTween.Sequence();
        GetSequnce_InterfaceOff(sequence);
        sequence.AppendCallback(()=>DiceSetAcive(true));

        sequence.SetLoops(1);
        sequence.Play();
    }

    public void GetSequnce_ChangeInterfaceNext(Sequence sequence)
    {
        // �������̽��� �����
        GetSequnce_InterfaceOff(sequence);

        // �ʿ���� �������̽� ��Ȱ��ȭ
        sequence.AppendCallback(() => DiceSetAcive(false));
    }

    public void GetSequnce_InterfaceOn(Sequence sequence)
    {
        sequence.Append(rectTransform.DOAnchorPos(InScreenAnchoredPos, delay));
    }
    public void GetSequnce_InterfaceOff(Sequence sequence)
    {
        sequence.Append(rectTransform.DOAnchorPos(OutOfScreenAnchoredPos, delay));
    }
}
