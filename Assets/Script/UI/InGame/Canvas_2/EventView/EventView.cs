using UnityEngine;
using UnityEngine.UI;

public class EventView : ButtonBase
{
    [SerializeField] private Text eventText;
    [SerializeField] private CanvasGroup _canvasGroup;
    public CanvasGroup canvasGroup { get { return _canvasGroup; } }
    


    private void Start()
    {
        SetButtonCallback(EventEnd);
    }

    public void SetTextContent(string value)
    {
        eventText.text = value;
    }
    public void SetTextColer(Color value)
    {
        eventText.color = value;
    }

    public void EventEnd()
    {
        gameObject.SetActive(false);
    }
}
