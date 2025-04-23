using PublicSet;
using System.Linq.Expressions;
using UnityEngine;
using static PublicSet.iNeedCheck;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T_Class">PopUpOptionButtonBase를 상속받을 자식클래스</typeparam>
/// <typeparam name="sDefaultData">선택 버튼이 갖는 id과 type의 데이터</typeparam>
/// <typeparam name="cInfo">type에 따라 csv에서 가져올 수 있는 정보</typeparam>
public abstract class PopUpOptionButtonBase<T_Class, sDefaultData ,cInfo> : Selection_ButtonBase<T_Class> 
    where T_Class : PopUpOptionButtonBase<T_Class, sDefaultData, cInfo>
    where sDefaultData : struct
    where cInfo : class, iNeedCheck
{
    [SerializeField] protected GameObject clickGuide;

    protected sDefaultData defaultData;
    protected cInfo info;


    public virtual void SetData(sDefaultData defualtData, cInfo info)
    {
        this.defaultData = defualtData;
        this.info = info;
    }

    public abstract void InitPanel();

    /// <summary>
    /// 아이콘뷰도 함께 처리해야 하는 경우
    /// </summary>
    /// <param name="icon"></param>
    protected void ClickCheck(eIcon icon)
    {
        // 확인이 필요하면 객체를 활성화 시키고 그렇지 않으면 비활성화
        if (info.isNeedCheck)
        {
            GameManager.connector_InGame.iconView_Script.TryClickGuideOn(icon);
            if (clickGuide.activeInHierarchy == false)
                clickGuide.SetActive(true);
        }
        else
        {
            GameManager.connector_InGame.iconView_Script.TryClickGuideOff(icon);
            if (clickGuide.activeInHierarchy)
                clickGuide.SetActive(false);
        }
    }

    /// <summary>
    /// 아이콘 뷰가 없는 경우
    /// </summary>
    protected void ClickCheck()
    {
        // 확인이 필요하면 객체를 활성화 시키고 그렇지 않으면 비활성화
        if (info.isNeedCheck)
        {
            if (clickGuide.activeInHierarchy == false)
                clickGuide.SetActive(true);
        }
        else
        {
            if (clickGuide.activeInHierarchy)
                clickGuide.SetActive(false);
        }
    }
}
