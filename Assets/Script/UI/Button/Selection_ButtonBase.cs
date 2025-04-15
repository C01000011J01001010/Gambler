using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

/// <summary>
/// 같은 종류의 버튼은 오직 한순간에 한번만 선택됨
/// </summary>
/// <typeparam name="T_Class">상속받는 클래스</typeparam>
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
                Debug.Log("이미지 컴포넌트 확보");
            }
            return _image; 
        }
    }

    /// <summary>
    /// 상속받는 클래스당 1개만 존재하는 공용변수
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
