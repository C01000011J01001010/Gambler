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
        // 창모드는 마지막 위치가 중심이 아닐 수 있음
        rectTransform.anchoredPosition = AnchoredStartPos;
    }

    public virtual void UpdateMainDescription(List<string> descriptionList)
    {
        if(descriptionList.Count > 0)
            mainDescription.text = descriptionList[0];

        // 2개 이상의 문자열인 경우 줄바꿈을 적용함
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
