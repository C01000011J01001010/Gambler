using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

/// <summary>
/// ���� ������ ��ư�� ���� �Ѽ����� �ѹ��� ���õ�
/// </summary>
/// <typeparam name="T_Class">��ӹ޴� Ŭ����</typeparam>
public abstract class Selection_ButtonBase<T_Class> : Deactivatable_ButtonBase where T_Class : Selection_ButtonBase<T_Class>
{
    private Image _image = null;
    protected Image image
    {
        get
        {
            if (_image == null) 
            {
                _image = GetComponent<Image>();
                Debug.Log("�̹��� ������Ʈ Ȯ��");
            }
            return _image; 
        }
    }

    /// <summary>
    /// ��ӹ޴� Ŭ������ 1���� �����ϴ� ���뺯��
    /// </summary>
    protected static T_Class currentSelectedObj;

    private void OnDisable()
    {
        UnselectThisButton();
    }

    [SerializeField] protected Text buttonText;
    public UnityAction callback { get; private set; }

    public void Setpanel(string value)
    {
        buttonText.text = value;
    }

    /// <summary>
    /// SetButtonCallback��� �ش� �Լ��� ����ؾ���
    /// </summary>
    /// <param name="currentObj">�� Obj�� ��ũ��Ʈ ������Ʈ��ü</param>
    /// <param name="fuc">�׸� Ŭ���� description�� ������ ����</param>
    public void SetCallback(T_Class currentObj, UnityAction fuc)
    {
        SetButtonCallback(() => TrySelectThisButton(currentObj));
        callback = fuc;
    }


    /// <summary>
    /// �� �ڽ�Ŭ������ start���� ȣ��Ǿ����
    /// </summary>
    /// <param name="currentObj"></param>
    public virtual void TrySelectThisButton(T_Class currentObj)
    {
        if (currentSelectedObj == null)
        {
            currentSelectedObj = currentObj;
            TryDeactivate_Button();
            callback();
        }
        else
        {
            currentSelectedObj.UnselectThisButton();
            TrySelectThisButton(currentObj);
        }
    }

    public virtual void UnselectThisButton()
    {
        if(currentSelectedObj != null)
        {
            currentSelectedObj.TryActivate_Button();
            currentSelectedObj = null;
        }
    }
}
