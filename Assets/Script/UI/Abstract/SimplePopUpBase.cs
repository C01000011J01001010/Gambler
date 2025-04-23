using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class SimplePopUpBase : MonoBehaviour
{
    [SerializeField]protected Text mainDescription;
    [SerializeField] protected DragTarget dragTarget_Top;
    [SerializeField] protected DragTarget dragTarget_Bottom;

    private RectTransform rectTransform { get; set; }
    private Vector2 AnchoredStartPos { get; set; }

    protected void Awake()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        dragTarget_Top.SetMoveObjAttribute(rectTransform, canvasGroup);
        dragTarget_Bottom.SetMoveObjAttribute(rectTransform, canvasGroup);

        AnchoredStartPos = rectTransform.anchoredPosition;
    }

    protected virtual void OnEnable()
    {
        // â���� ������ ��ġ�� �߽��� �ƴ� �� ����
        rectTransform.anchoredPosition = AnchoredStartPos;
    }

    public virtual void UpdateMainDescription(List<string> descriptionList)
    {
        if(descriptionList.Count > 0)
            mainDescription.text = descriptionList[0];

        // 2�� �̻��� ���ڿ��� ��� �ٹٲ��� ������
        for (int i = 1; i<descriptionList.Count; i++)
        {
            mainDescription.text += $"\n{descriptionList[i]}";
        }
    }

    public virtual void UpdateMainDescription(string description)
    {
        mainDescription.text = description;
    }
}
