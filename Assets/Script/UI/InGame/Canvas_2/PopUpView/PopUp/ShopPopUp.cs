using PublicSet;
using System.Collections.Generic;

public class ShopPopUp : ItemPopUpBase<InventoryPopUp, InventoryItemOptionButton, InventoryItemDescriptionPanel>
{
    protected override Dictionary<eItemType, cPlayerItem> PopUpItemDict => ShopManager.Instance.ItemHashSet;
}
