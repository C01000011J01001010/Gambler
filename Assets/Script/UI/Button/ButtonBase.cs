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
                Debug.LogAssertion($"{gameObject.name} ��ü�� ��ư������Ʈ�� �������� ����"); 
                return null; 
            }
        }
    }

    public bool isInteractable => button.interactable;

    /// <summary>
    /// �ݹ��Լ��� ������ �ϳ��� ���� �� ������, ���� ���� ���� �� �����Լ��� ���� �Լ��� �������
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
            // ��ȣ�ۿ��� ���� ���� �� �⺻���� ����Ǵ� ������ ����
            ColorBlock colorBlock = button.colors;
            Color color = colorBlock.disabledColor;

            color.a = 1.0f;

            colorBlock.disabledColor = color;
            button.colors = colorBlock;
        }
    }
}
