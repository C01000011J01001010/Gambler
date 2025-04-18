using UnityEngine;
using UnityEngine.EventSystems;

public class DragTarget : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform moveObjRectTransform {  get; private set; }
    public CanvasGroup moveObjCanvasGroup {  get; private set; }

    /// <summary>
    /// ���� ��ü�� �����ϴ� ���� ����� ĵ����
    /// </summary>
    private Canvas _canvas;
    public Canvas canvas
    {
        get 
        {
            if (_canvas == null) _canvas = MethodManager.FindParentCanvas(transform); 
            return _canvas; 
        }
    }


    /// <summary>
    /// IPointerDownHandler, ��ġ�ٿ�� ȣ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("��ġ/Ŭ�� ����!");
        DragManager.Instance.OnTouchEnter(this, eventData, canvas);
    }

    /// <summary>
    /// IBeginDragHandler, ��ġ �� �巡�� ������ �� ȣ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("�巡�� ����!");
        // ���������� ó��
    }

    /// <summary>
    /// IDragHandler, �巡�� �� ��� ȣ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        DragManager.Instance.Drag(eventData);
    }

    /// <summary>
    /// IEndDragHandler, �巡�� ������ �� ȣ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        DragManager.Instance.EndDrag();
    }

    /// <summary>
    /// ���� �巡�� ���� �̵��ϴ� ����� �ٸ� �� ����
    /// </summary>
    /// <param name="value"></param>
    public void SetMoveObjAttribute(RectTransform rectTransform, CanvasGroup canvasGroup)
    {
        if (canvasGroup != null)
        {
            moveObjRectTransform = rectTransform;
            moveObjCanvasGroup = canvasGroup;
        }
        else
        {
            Debug.LogAssertion("ĵ���� �׷��� ����");
        }
            
    }
    
    public RectTransform GetRectTransform() => GetComponent<RectTransform>();
    public CanvasGroup GetCanvasGroup() => GetComponent<CanvasGroup>();
}
