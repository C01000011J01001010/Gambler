using UnityEngine;
using DG.Tweening;
public class PlayerInterface_CardGame : MonoBehaviour
{
    //������
    public RectTransform rectTransform;
    public CardScreenButton cardScreenButton;
    public DiceButton diceButton;
    public GameObject SubScreen_Dice;

    //��ũ��Ʈ
    private Vector2 InScreenAnchoredPos;
    private Vector2 OutOfScreenAnchoredPos;
    private Vector2 rectSize;
    float delay = 1.0f;
    

    private void Awake()
    {
        SetPos();
    }

    private void Start()
    {
        if(cardScreenButton == null)
        {
            Debug.LogAssertion("cardScreenButton == null");
        }

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

    public void InitInterface()
    {
        // Ȱ��ȭ�Ǹ� �ۿ��� ���
        rectTransform.anchoredPosition = OutOfScreenAnchoredPos;

        cardScreenButton.TryDeactivate_Button();
        diceButton.TryDeactivate_Button();

        // content�� Ȱ��ȭ�� ��ü �ʱ�ȭ
        DiceSetAcive(true);
        cardScreenButton.gameObject.SetActive(false);
    }

    public void returnInterface()
    {
        diceButton.TryDeactivate_Button();

        Sequence sequence = DOTween.Sequence();
        GetSequnce_InterfaceOff(sequence);
        sequence.AppendCallback(()=>DiceSetAcive(true));
        sequence.AppendCallback(()=>cardScreenButton.gameObject.SetActive(false));

        sequence.SetLoops(1);
        sequence.Play();
    }

    public void GetSequnce_ChangeInterfaceNext(Sequence sequence)
    {
        // �������̽��� �����
        GetSequnce_InterfaceOff(sequence);

        // ���빰�� ������ ����
        sequence.AppendCallback(() => DiceSetAcive(false));
        sequence.AppendCallback(() => cardScreenButton.gameObject.SetActive(true));
        sequence.AppendCallback(() => cardScreenButton.TryActivate_Button());

        // �ٽ� ����
        GetSequnce_InterfaceOn(sequence);
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
