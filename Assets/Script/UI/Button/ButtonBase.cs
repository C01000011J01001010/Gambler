using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class ButtonBase : MonoBehaviour
{
    private Button _button;

    private Button button
    {
        get
        {
            if (_button == null) _button = GetComponent<Button>(); 

            if(_button != null) return _button;
            else 
            { 
                Debug.LogAssertion($"{gameObject.name} 객체는 버튼컴포넌트를 갖고있지 않음"); 
                return null; 
            }
        }
    }

    public bool isInteractable => button.interactable;

    /// <summary>
    /// 콜백함수는 오로지 하나만 넣을 수 있으며, 여러 개를 넣을 시 람다함수로 여러 함수를 묶어야함
    /// </summary>
    /// <param name="callback"></param>
    public virtual void SetButtonCallback(UnityAction callback)
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();

            if(callback != null) button.onClick.AddListener(callback);
        }
        else
        {
            Debug.Log("button == null");
        }
    }

    public virtual void ClearButtonCallback()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    public virtual void SetButtonInteractable(bool isOn)
    {
        button.interactable = isOn;
    }

    public virtual void SetDisabledColorAlpha_1()
    {
        if (button.colors.disabledColor.a > 0.99f) return;

        else
        {
            // 상호작용을 하지 않을 시 기본으로 적용되는 반투명 제거
            ColorBlock colorBlock = button.colors;
            Color color = colorBlock.disabledColor;

            color.a = 1.0f;

            colorBlock.disabledColor = color;
            button.colors = colorBlock;
        }
    }
}
