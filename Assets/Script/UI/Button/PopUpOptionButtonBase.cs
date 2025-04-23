using PublicSet;
using System.Linq.Expressions;
using UnityEngine;
using static PublicSet.iNeedCheck;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T_Class">PopUpOptionButtonBase�� ��ӹ��� �ڽ�Ŭ����</typeparam>
/// <typeparam name="sDefaultData">���� ��ư�� ���� id�� type�� ������</typeparam>
/// <typeparam name="cInfo">type�� ���� csv���� ������ �� �ִ� ����</typeparam>
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
    /// �����ܺ䵵 �Բ� ó���ؾ� �ϴ� ���
    /// </summary>
    /// <param name="icon"></param>
    protected void ClickCheck(eIcon icon)
    {
        // Ȯ���� �ʿ��ϸ� ��ü�� Ȱ��ȭ ��Ű�� �׷��� ������ ��Ȱ��ȭ
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
    /// ������ �䰡 ���� ���
    /// </summary>
    protected void ClickCheck()
    {
        // Ȯ���� �ʿ��ϸ� ��ü�� Ȱ��ȭ ��Ű�� �׷��� ������ ��Ȱ��ȭ
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
