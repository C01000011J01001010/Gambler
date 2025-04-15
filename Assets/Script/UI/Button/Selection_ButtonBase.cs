using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

/// <summary>
/// ���� ������ ��ư�� ���� �Ѽ����� �ѹ��� ���õ�
/// </summary>
/// <typeparam name="T_Class">��ӹ޴� Ŭ����</typeparam>
public abstract class Selection_ButtonBase<T_Class> : ButtonBase where T_Class : ButtonBase
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

    private void Start()
    {
        SetButtonCallback(TrySelectThisButton);
    }
    private void OnDisable()
    {
        UnselectThisButton();
    }

    public abstract void TrySelectThisButton();
    public abstract void UnselectThisButton();
}
