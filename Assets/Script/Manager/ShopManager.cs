using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : EntryDictManagerBase<ShopManager, cPlayerItem, eItemType>
{
    public Dictionary<eItemType, cPlayerItem> ItemHashSet
    {
        get => EntryDict;
        private set => EntryDict = value;
    }

    public override void InitAllDict()
    {
        ItemHashSet = new Dictionary<eItemType, cPlayerItem>();
    }
    public override void ClearAllDict()
    {
        ItemHashSet.Clear();
    }

    public void RestockTheShop(eItemType item)
    {

    }
}
