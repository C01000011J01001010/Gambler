using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopUp : PopUpBase<InventoryPopUp>
{
    public sItem currentClickItem; // ���� Ŭ���� ������
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
                    // ������������ �ʱ�ȭ�� ��ü
                    ItemDefault itemDefault = ActiveObjList[item.id].GetComponent<ItemDefault>();

                    // ������ ���������� ȣ��
                    cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.type);

                    // Ȱ��ȭ�� �� ��ü�� ������ �ʱ�ȭ
                    if (itemDefault != null)
                    {
                        itemDefault.InitItemData(item, itemInfo);
                    }
                    else
                    {
                        Debug.LogAssertion($"{itemDefault.gameObject.name}�� itemScript == null");
                    }

                    // ������ Ŭ���� �ǳ� ����
                    itemDefault.SetCallback(() => itemDescriptionPanel.SetPanel(itemDefault, itemInfo));

                }
            });

    }
}
