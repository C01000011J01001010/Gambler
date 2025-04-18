using UnityEngine;
using UnityEngine.EventSystems;

public class DragManager : Singleton<DragManager>
{
    private Canvas canvas;

    private RectTransform currentRect;
    
    private CanvasGroup canvasGroup;


    public void OnTouchEnter(DragTarget target, PointerEventData eventData, Canvas TargetCanvas)
    {
        if(target.moveObjRectTransform == null || target.moveObjCanvasGroup == null)
        {
            Debug.Log("�巡�� ����� �����̴� ������� ��");
            currentRect = target.GetRectTransform();
            canvasGroup = target.GetCanvasGroup();
        }
        else // �����̴� ����� ������ ��츸 �ش� ������Ʈ�� ���
        {
            Debug.Log("�巡�� ����� �����̴� ����� �ٸ�");
            currentRect = target.moveObjRectTransform;
            canvasGroup = target.moveObjCanvasGroup;
        }

        
        canvas = TargetCanvas;

        canvasGroup.alpha = 0.6f; // �ش� ��ü��
        canvasGroup.blocksRaycasts = false;
    }

    public void Drag(PointerEventData eventData)
    {
        if (currentRect == null) return;

        // eventData.delta == �̵��� �ȼ���, canvas.scaleFactor == ĵ���� Ȯ�����
        currentRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void EndDrag()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        currentRect = null;
        canvasGroup = null;
    }
}

