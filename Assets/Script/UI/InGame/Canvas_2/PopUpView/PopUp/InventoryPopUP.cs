using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPopUp : ItemPopUpBase<InventoryPopUp, InventoryItemOptionButton, InventoryItemDescriptionPanel>
{
    protected override Dictionary<eItemType, cPlayerItem> PopUpItemDict => ItemManager.Instance.PlayerItemDict;
    public cPlayerItem currentClickItem { get; private set; } // ���� Ŭ���� ������
    //[SerializeField] private InventoryItemDescriptionPanel _itemDescriptionPanel;
    //public InventoryItemDescriptionPanel descriptionPanel => _itemDescriptionPanel;

    

    //Dictionary<int, sItem> playerItems => ItemManager.Instance.ItemHashSet;


    ///// <summary>
    ///// ���� �����ϱ����� ���
    ///// </summary>
    //private HashSet<int> itemSlotIndexHash { get; set; }

    ///// <summary>
    ///// popup�� �⺻������ ��Ȱ��ȭ �����̴� popupView���� �ʱ�ȭ
    ///// </summary>
    //public void InitAttribute()
    //{
    //    int ItemSlotSize = contentGrid.constraintCount * 10;

    //    if (itemSlotIndexHash == null)
    //    {
    //        itemSlotIndexHash = new HashSet<int>();
    //        for (int i = 0; i < ItemSlotSize; i++)
    //        {
    //            itemSlotIndexHash.Add(i);
    //        }
    //    }
    //}

    //private void OnEnable()
    //{
    //    RefreshPopUp();
    //    ScrollToTop();
    //    descriptionPanel.ClearPanel();
    //}

    ///// <summary>
    ///// Ŭ�����̵� Ȱ��ȭ�� itemOption.InitPanel()�� ClickCheck(eIcon.Inventory)���� ������
    ///// </summary>
    //public override void RefreshPopUp()
    //{
    //    RefreshPopUp(itemSlotIndexHash.Count,
    //        () =>
    //        {
    //            // �ʱ�ȭ���� ���� ������ĭ�� Ȯ���ϱ� ���� �ؽü�
    //            HashSet<int> clearSlotHash = new HashSet<int>(itemSlotIndexHash);

    //            // ������ĭ �ʱ�ȭ
    //            foreach (sItem item in playerItems)
    //            {
    //                // ������������ �ʱ�ȭ�� ��ü(������ĭ)
    //                InventoryItemOptionButton itemOption = ActiveObjList[item.id].GetComponent<InventoryItemOptionButton>();
    //                clearSlotHash.Remove(item.id); //�ʱ�ȭ�� ������ĭ�� �ε����� �ؽü¿��� ����

    //                // ������ ���������� ȣ��
    //                cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.type);

    //                // Ȱ��ȭ�� �� ��ü�� ������ �ʱ�ȭ
    //                if (itemOption != null)
    //                {
    //                    itemOption.SetData(item, itemInfo);
    //                    itemOption.InitPanel();

    //                    // ������ Ŭ���� �ǳ� ����
    //                    itemOption.SetCallback(
    //                        itemOption,
    //                        () =>
    //                        {
    //                            // �����ۿ� ���� ������ ȭ�鿡 ǥ��
    //                            descriptionPanel.SetPanel(itemOption, itemInfo);

    //                            // �������� Ȯ�������� �Ӽ� ������ ��������
    //                            itemInfo.isNeedCheck = false; 
    //                            RefreshPopUp();

    //                        });
    //                }
    //                else
    //                {
    //                    Debug.LogAssertion($"{itemOption.gameObject.name}�� itemScript == null");
    //                }
    //            }

    //            // ����ִ� ������ĭ �ʱ�ȭ
    //            foreach(int ClearSlot in clearSlotHash)
    //            {
    //                InventoryItemOptionButton itemOption = ActiveObjList[ClearSlot].GetComponent<InventoryItemOptionButton>();
    //                itemOption.ClearItemImage();
    //                itemOption.ClearClickGuide();
    //                itemOption.ClearButtonCallback(); // �ƹ��� ��ȣ�ۿ��� �Ұ����ϵ��� ����
    //            }

    //        });
    //}

}
