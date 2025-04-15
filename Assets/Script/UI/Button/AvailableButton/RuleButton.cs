using UnityEngine;

public class RuleButton : Selection_ButtonBase<RuleButton>
{
    
    /// <summary>
    /// 한번 선택된 버튼이 있으면 언셀렉 전까지 다른 버튼이 선택될 수 없음
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
