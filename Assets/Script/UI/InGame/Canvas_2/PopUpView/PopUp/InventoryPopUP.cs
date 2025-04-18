using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopUp : PopUpBase_FullScreen<InventoryPopUp>
{
    
    [SerializeField] private ItemDescriptionPanel _itemDescriptionPanel;
    public ItemDescriptionPanel descriptionPanel { get { return _itemDescriptionPanel; } }

    public sItem currentClickItem { get; private set; } // ���� Ŭ���� ������

    HashSet<sItem> playerItems
    {
        get { return ItemManager.ItemHashSet; }
    }

    private int ItemSlotSize;

    /// <summary>
    /// ���� �����ϱ����� ���
    /// </summary>
    private HashSet<int> itemSlotIndexHash;

    /// <summary>
    /// popup�� �⺻������ ��Ȱ��ȭ �����̴� popupView���� �ʱ�ȭ
    /// </summary>
    public void InitAttribute()
    {
        if (ItemSlotSize == 0)
        {
            ItemSlotSize = contentGrid.constraintCount * 10;
        }
        if (itemSlotIndexHash == null)
        {
            itemSlotIndexHash = new HashSet<int>();
            for (int i = 0; i < ItemSlotSize; i++)
            {
                itemSlotIndexHash.Add(i);
            }
        }
    }

    private void OnEnable()
    {
        RefreshPopUp();
        ScrollToTop();
        descriptionPanel.ClearPanel();
    }

    public override void RefreshPopUp()
    {
        RefreshPopUp(ItemSlotSize,
            () =>
            {
                // �ʱ�ȭ���� ���� ������ĭ�� Ȯ���ϱ� ���� �ؽü�
                HashSet<int> clearSlotHash = new HashSet<int>(itemSlotIndexHash);

                // ������ĭ �ʱ�ȭ
                foreach (sItem item in playerItems)
                {
                    // ������������ �ʱ�ȭ�� ��ü(������ĭ)
                    ItemDefault itemDefault = ActiveObjList[item.id].GetComponent<ItemDefault>();
                    clearSlotHash.Remove(item.id); //�ʱ�ȭ�� ������ĭ�� �ε����� �ؽü¿��� ����

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
                    itemDefault.SetCallback(itemDefault , () => descriptionPanel.SetPanel(itemDefault, itemInfo));
                }

                // ����ִ� ������ĭ �ʱ�ȭ
                foreach(int ClearSlot in clearSlotHash)
                {
                    ItemDefault itemDefault = ActiveObjList[ClearSlot].GetComponent<ItemDefault>();
                    itemDefault.ClearItemImage();
                    itemDefault.ClearButtonCallback(); // �ƹ��� ��ȣ�ۿ��� �Ұ����ϵ��� ����
                }
                
            });
    }

    
}
