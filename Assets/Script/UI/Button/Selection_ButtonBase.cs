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
    public Image image
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
        protected set { _image = value; }
    }

    [SerializeField] protected Text buttonText;
    

    public UnityAction callback { get; private set; }

    /// <summary>
    /// 상속받는 클래스당 1개만 존재하는 공용변수
    /// </summary>
    private static T_Class currentSelectedObj;

    protected virtual void OnDisable()
    {
        UnselectThisButton();
    }

    public static void ClearCurrentSelectedObj()
    {
        currentSelectedObj = null;
    }

    public virtual void Setpanel_Text(string value)
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
