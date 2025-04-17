using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopUp : PopUpBase<InventoryPopUp>
{
    public sItem currentClickItem; // 현재 클릭한 아이템
    [SerializeField] private ItemDescriptionPanel itemDescriptionPanel;

    HashSet<sItem> playerItems
    {
        get { return ItemManager.ItemHashSet; }
    }

    private void OnEnable()
    {
        RefreshPopUp();
        ScrollToTop();
    }

    public override void RefreshPopUp()
    {
        Debug.Log($"Player_items.Count == {playerItems.Count}");
        RefreshPopUp(playerItems.Count,
            () =>
            {
                foreach (sItem item in playerItems)
                {
                    // 아이템정보로 초기화될 객체
                    ItemDefault itemDefault = ActiveObjList[item.id].GetComponent<ItemDefault>();

                    // 아이템 종합정보를 호출
                    cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.type);

                    // 활성화된 각 객체에 정보를 초기화
                    if (itemDefault != null)
                    {
                        itemDefault.InitItemData(item, itemInfo);
                    }
                    else
                    {
                        Debug.LogAssertion($"{itemDefault.gameObject.name}은 itemScript == null");
                    }

                    // 아이템 클릭시 판넬 설정
                    itemDefault.SetCallback(() => itemDescriptionPanel.SetPanel(itemDefault, itemInfo));

                }
            });

    }
}
