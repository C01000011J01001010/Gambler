using UnityEngine;

public class RuleButton : Selection_ButtonBase<RuleButton>
{
    
    /// <summary>
    /// �ѹ� ���õ� ��ư�� ������ �𼿷� ������ �ٸ� ��ư�� ���õ� �� ����
    /// </summary>
    public override void TrySelectThisButton()
    {
        if (currentSelectedObj == null)
        {
            currentSelectedObj = this;
            image.color = Color.gray;
        }
        else
        {
            currentSelectedObj.UnselectThisButton();
            TrySelectThisButton();
        }
    }

    public override void UnselectThisButton()
    {
        image.color = Color.white;
        currentSelectedObj = null;
    }
}
