using PublicSet;
using System.Collections.Generic;
using UnityEngine;

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
                    ItemOptionButton itemOption = ActiveObjList[item.id].GetComponent<ItemOptionButton>();
                    clearSlotHash.Remove(item.id); //�ʱ�ȭ�� ������ĭ�� �ε����� �ؽü¿��� ����

                    // ������ ���������� ȣ��
                    cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.type);

                    // Ȱ��ȭ�� �� ��ü�� ������ �ʱ�ȭ
                    if (itemOption != null)
                    {
                        itemOption.SetData(item, itemInfo);
                        itemOption.InitPanel();

                        // ������ Ŭ���� �ǳ� ����
                        itemOption.SetCallback(
                            itemOption,
                            () =>
                            {
                                // �����ۿ� ���� ������ ȭ�鿡 ǥ��
                                descriptionPanel.SetPanel(itemOption, itemInfo);

                                // �������� Ȯ�������� �Ӽ� ������ ��������
                                itemInfo.isNeedCheck = false; 
                                RefreshPopUp();

                            });
                    }
                    else
                    {
                        Debug.LogAssertion($"{itemOption.gameObject.name}�� itemScript == null");
                    }
                }

                // ����ִ� ������ĭ �ʱ�ȭ
                foreach(int ClearSlot in clearSlotHash)
                {
                    ItemOptionButton itemOption = ActiveObjList[ClearSlot].GetComponent<ItemOptionButton>();
                    itemOption.ClearItemImage();
                    itemOption.ClearClickGuide();
                    itemOption.ClearButtonCallback(); // �ƹ��� ��ȣ�ۿ��� �Ұ����ϵ��� ����
                }
                
            });
    }

    
}
