using PublicSet;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemDescriptionPanel : DescriptionPanelBase<InventoryItemOptionButton, cPlayerItem, cItemInfo>
{
    [SerializeField] private ItemUseButton useButton;
    [SerializeField] private ItemRegisterButton RegisterButton;


    protected override void ButtonSetActive(bool active)
    {
        useButton.gameObject.SetActive(active);
        //RegisterButton.gameObject.SetActive(active);
    }

    public override void SetPanel(InventoryItemOptionButton item, cItemInfo itemInfo)
    {
        base.SetPanel(item, itemInfo);

        // 아이템이 사용가능한 경우 
        if (itemInfo.isAvailable)
        {
            // 버튼셋 활성화
            ButtonSetActive(true);

            // 사용하기 누를시 확인창 열림
            if (item.useCheckCallback != null)
                useButton.SetButtonCallback(item.useCheckCallback);

            // 퀵슬롯 등록하기
            //RegisterButton.SetButtonCallback();
        }

        // 기타 잡템의 경우 
        else if (itemInfo.isAvailable == false)
        {
            // 버튼셋 비활성화
            ButtonSetActive(false);
        }
    }
}
