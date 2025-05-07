using PublicSet;
using System.Linq.Expressions;
using UnityEngine;
using static PublicSet.INeedCheck;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T_Class">PopUpOptionButtonBase�� ��ӹ��� �ڽ�Ŭ����</typeparam>
/// <typeparam name="sDefaultData">���� ��ư�� ���� id�� type�� ������</typeparam>
/// <typeparam name="cInfo">type�� ���� csv���� ������ �� �ִ� ����</typeparam>
public abstract class PopUpOptionButtonBase<T_Class, sDefaultData ,cInfo> : Selection_ButtonBase<T_Class> 
    where T_Class : PopUpOptionButtonBase<T_Class, sDefaultData, cInfo>
    where sDefaultData : struct
    where cInfo : class, INeedCheck
{
    [SerializeField] protected GameObject clickGuide;

    protected sDefaultData defaultData;
    protected cInfo info;

    public IconView iconView => GameManager.connector_InGame.Canvas1.IconView;


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
        clickGuide.SetActive(info.isNeedCheck);
        if (info.isNeedCheck) iconView.TryClickGuideOn(icon);
        else iconView.TryClickGuideOff(icon);
    }

    /// <summary>
    /// ������ �䰡 ���� ���
    /// </summary>
    protected void ClickCheck()
    {
        // Ȯ���� �ʿ��ϸ� ��ü�� Ȱ��ȭ ��Ű�� �׷��� ������ ��Ȱ��ȭ
        clickGuide.SetActive(info.isNeedCheck);
    }
}
