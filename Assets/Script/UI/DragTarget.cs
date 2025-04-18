using UnityEngine;
using UnityEngine.EventSystems;

public class DragTarget : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform moveObjRectTransform {  get; private set; }
    public CanvasGroup moveObjCanvasGroup {  get; private set; }

    /// <summary>
    /// 현재 객체를 포함하는 가장 가까운 캔버스
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
    /// IPointerDownHandler, 터치다운시 호출
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("터치/클릭 시작!");
        DragManager.Instance.OnTouchEnter(this, eventData, canvas);
    }

    /// <summary>
    /// IBeginDragHandler, 터치 후 드래그 시작할 때 호출
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("드래그 시작!");
        // 선택적으로 처리
    }

    /// <summary>
    /// IDragHandler, 드래그 중 계속 호출
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        DragManager.Instance.Drag(eventData);
    }

    /// <summary>
    /// IEndDragHandler, 드래그 끝났을 때 호출
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        DragManager.Instance.EndDrag();
    }

    /// <summary>
    /// 실제 드래그 대상과 이동하는 대상이 다를 수 있음
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
            Debug.LogAssertion("캔버스 그룹이 없음");
        }
            
    }
    
    public RectTransform GetRectTransform() => GetComponent<RectTransform>();
    public CanvasGroup GetCanvasGroup() => GetComponent<CanvasGroup>();
}
