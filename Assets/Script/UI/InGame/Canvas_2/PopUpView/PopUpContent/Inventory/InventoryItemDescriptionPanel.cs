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

        // �������� ��밡���� ��� 
        if (itemInfo.isAvailable)
        {
            // ��ư�� Ȱ��ȭ
            ButtonSetActive(true);

            // ����ϱ� ������ Ȯ��â ����
            if (item.useCheckCallback != null)
                useButton.SetButtonCallback(item.useCheckCallback);

            // ������ ����ϱ�
            //RegisterButton.SetButtonCallback();
        }

        // ��Ÿ ������ ��� 
        else if (itemInfo.isAvailable == false)
        {
            // ��ư�� ��Ȱ��ȭ
            ButtonSetActive(false);
        }
    }
}
