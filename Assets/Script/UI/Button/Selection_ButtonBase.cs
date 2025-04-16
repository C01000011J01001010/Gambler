using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// ���� ������ ��ư�� ���� �Ѽ����� �ѹ��� ���õ�
/// </summary>
/// <typeparam name="T_Class">��ӹ޴� Ŭ����</typeparam>
public abstract class Selection_ButtonBase<T_Class> : ButtonBase where T_Class : Selection_ButtonBase<T_Class>
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
    public Action callback { get; private set; }

    public void Setpanel(string value)
    {
        buttonText.text = value;
    }
    public void SetCallback(Action fuc)
    {
        callback = fuc;
    }

    public virtual void TrySelectThisButton(T_Class currentObj)
    {
        if (currentSelectedObj == null)
        {
            currentSelectedObj = currentObj;
            image.color = Color.gray;
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
        image.color = Color.white;
        currentSelectedObj = null;
    }
}
