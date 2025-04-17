using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 같은 종류의 버튼은 오직 한순간에 한번만 선택됨
/// </summary>
/// <typeparam name="T_Class">상속받는 클래스</typeparam>
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
                Debug.Log("이미지 컴포넌트 확보");
            }
            return _image; 
        }
    }

    /// <summary>
    /// 상속받는 클래스당 1개만 존재하는 공용변수
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

    /// <summary>
    /// SetButtonCallback대신 해당 함수를 사용해야함
    /// </summary>
    /// <param name="fuc"></param>
    public void SetCallback(Action fuc)
    {
        callback = fuc;
    }

    /// <summary>
    /// 각 자식클래스의 start에서 호출되어야함
    /// </summary>
    /// <param name="currentObj"></param>
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
