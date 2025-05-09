using UnityEngine;

public abstract class PopUpBase_Window<T_Class> : PopUpBase<T_Class>
    where T_Class : PopUpBase_Window<T_Class>
{
    [SerializeField] protected DragTarget dragTarget_Top;
    [SerializeField] protected DragTarget dragTarget_Bottom;

    private RectTransform rectTransform {  get; set; }
    private Vector2 AnchoredStartPos { get; set; }

    protected override void Awake()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        dragTarget_Top.SetMoveObjAttribute(rectTransform, canvasGroup);
        dragTarget_Bottom.SetMoveObjAttribute(rectTransform, canvasGroup);

        AnchoredStartPos = rectTransform.anchoredPosition;
    }

    protected virtual void OnEnable()
    {
        // 창모드는 마지막 위치가 중심이 아닐 수 있음
        rectTransform.anchoredPosition = AnchoredStartPos;
    }
}
