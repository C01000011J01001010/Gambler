using UnityEngine;
using DG.Tweening;
public class DiceSet : MonoBehaviour
{
    //에디터
    //public CardScreenButton cardScreenButton;
    public DiceButton diceButton;
    public GameObject SubScreen_Dice;

    //스크립트
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
        // 현재 위치를 내부에서의 위치로 결정
        InScreenAnchoredPos = rectTransform.anchoredPosition;

        // 박스의 x축만큼 이동하여 화면에서 완전히 사라지도록 좌표를 결정
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
        // 활성화되면 밖에서 대기
        rectTransform.anchoredPosition = OutOfScreenAnchoredPos;

        diceButton.TryDeactivate_Button();

        // content에 활성화할 객체 초기화
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
        // 인터페이스를 숨기고
        GetSequnce_InterfaceOff(sequence);

        // 필요없는 인터페이스 비활성화
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
