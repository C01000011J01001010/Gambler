using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToggleBase : MonoBehaviour
{

    private Toggle _toggle;
    public Toggle toggle
    {
        get 
        {
            if (_toggle == null)
            {
                _toggle = GetComponent<Toggle>();
            }
            return _toggle; 
        }
    }

    /// <summary>
    /// SetIsOn ����� �ݹ��� ������ �� ���
    /// </summary>
    protected UnityAction<bool> StoredCallback;

    public void SetInteractable(bool value)
    {
        toggle.interactable = value;

        if(toggle.interactable == false)
        {
            SetIsOn(value);
        }
    }

    /// <summary>
    /// ��ȣ�ۿ���� �ٲ�
    /// </summary>
    /// <param name="value"></param>
    public void SetIsOn(bool value)
    {
        toggle.onValueChanged.RemoveAllListeners();
        toggle.isOn = value;
        SetToggleCallback(StoredCallback);
    }

    public void SetToggleCallback(UnityAction<bool> callback)
    {
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(callback);
        if(StoredCallback != callback) StoredCallback = callback;
    }

}
