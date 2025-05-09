using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemPopUpBase<T_Class,T_OptionButton, T_DescriptionPanel> 
    : PopUpBase_FullScreen<T_Class>
    where T_Class : ItemPopUpBase<T_Class, T_OptionButton, T_DescriptionPanel>
    where T_OptionButton : PopUpOptionButtonBase<T_OptionButton, cPlayerItem, cItemInfo>
    where T_DescriptionPanel : DescriptionPanelBase<T_OptionButton, cPlayerItem, cItemInfo>
{

    [SerializeField] private T_DescriptionPanel _itemDescriptionPanel;
    /// <summary>
    /// ���� �����ϱ����� ���
    /// </summary>
    private HashSet<int> _itemSlotIndexHash;

    public T_DescriptionPanel descriptionPanel => _itemDescriptionPanel;
    public HashSet<int> itemSlotIndexHash => _itemSlotIndexHash;

    /// <summary>
    /// �߻�������Ƽ, ��ӹ޴� �� �˾����� ����
    /// </summary>
    protected abstract Dictionary<eItemType, cPlayerItem> PopUpItemDict { get; }




    /// <summary>
    /// popup�� �⺻������ ��Ȱ��ȭ �����̴� popupView���� �ʱ�ȭ
    /// </summary>
    public void InitAttribute()
    {
        int ItemSlotSize = contentGrid.constraintCount * 10;

        if (_itemSlotIndexHash == null)
        {
            _itemSlotIndexHash = new HashSet<int>();
            for (int i = 0; i < ItemSlotSize; i++)
            {
                _itemSlotIndexHash.Add(i);
            }
        }
    }

    private void OnEnable()
    {
        RefreshPopUp();
        ScrollToTop();
        descriptionPanel.ClearPanel();
    }

    

    /// <summary>
    /// Ŭ�����̵� Ȱ��ȭ�� itemOption.InitPanel()�� ClickCheck(eIcon.Inventory)���� ������
    /// </summary>
    public override void RefreshPopUp()
    {
        RefreshPopUp(itemSlotIndexHash.Count,
            () =>
            {
                // �ʱ�ȭ���� ���� ������ĭ�� Ȯ���ϱ� ���� �ؽü�
                HashSet<int> clearSlotHash = new HashSet<int>(itemSlotIndexHash);

                // ������ĭ �ʱ�ȭ
                foreach (eItemType itemType in PopUpItemDict.Keys)
                {
                    cPlayerItem item = PopUpItemDict[itemType];

                    // ������������ �ʱ�ȭ�� ��ü(������ĭ)
                    T_OptionButton itemOption = ActiveObjList[item.id].GetComponent<T_OptionButton>();
                    clearSlotHash.Remove(item.id); //�ʱ�ȭ�� ������ĭ�� �ε����� �ؽü¿��� ����

                    // ������ ���������� ȣ��
                    cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(itemType);

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

                ClearEmptyItemSlot(clearSlotHash);

            });
    }

    protected void ClearEmptyItemSlot(HashSet<int> clearSlotHash)
    {
        // ����ִ� ������ĭ �ʱ�ȭ
        foreach (int ClearSlot in clearSlotHash)
        {
            InventoryItemOptionButton itemOption = ActiveObjList[ClearSlot].GetComponent<InventoryItemOptionButton>();
            itemOption.ClearItemImage();
            itemOption.ClearClickGuide();
            itemOption.ClearButtonCallback(); // �ƹ��� ��ȣ�ۿ��� �Ұ����ϵ��� ����
        }
    }


}

