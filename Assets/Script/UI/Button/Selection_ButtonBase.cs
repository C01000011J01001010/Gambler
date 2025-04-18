using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

/// <summary>
/// 같은 종류의 버튼은 오직 한순간에 한번만 선택됨
/// </summary>
/// <typeparam name="T_Class">상속받는 클래스</typeparam>
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
    public UnityAction callback { get; private set; }

    public void Setpanel(string value)
    {
        buttonText.text = value;
    }

    /// <summary>
    /// SetButtonCallback대신 해당 함수를 사용해야함
    /// </summary>
    /// <param name="currentObj">각 Obj의 스크립트 컴포넌트객체</param>
    /// <param name="fuc">항목 클릭시 description에 실행할 내용</param>
    public void SetCallback(T_Class currentObj, UnityAction fuc)
    {
        SetButtonCallback(() => TrySelectThisButton(currentObj));
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
