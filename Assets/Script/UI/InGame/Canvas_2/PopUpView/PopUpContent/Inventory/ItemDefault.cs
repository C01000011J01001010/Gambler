using PublicSet;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemDefault : Selection_ButtonBase<ItemDefault>
{
    [SerializeField] private Image itemImage;
    sItem item;
    cItemInfo itemInfo;

    public sItem Item { get { return item; } }
    public cItemInfo ItemInfo { get { return itemInfo; } }
    private PopUpView_InGame popUpView
    {
        get
        {
            return GameManager.connector_InGame.popUpView_Script;
        }
    }

    public UnityAction useCheckCallback;


    public void InitItemData(sItem item, cItemInfo itemInfo)
    {
        this.item = new sItem(item);
        InitItemInfo(itemInfo);
        InitItemImage();

        if (itemInfo.isAvailable)
        {
            useCheckCallback =
                () =>
                {
                    // ������ ����Ұ��� üũ
                    popUpView.YesOrNoPopUpOpen();

                    // �˾�â�� ���� �� ������ ��� ���� ���
                    popUpView.yesOrNoPopUp.SetYesText("��");
                    popUpView.yesOrNoPopUp.UpdateMainDescription("�ش� �������� ����Ͻðڽ��ϱ�?");


                    // yes������ ������ ������ ���� �ݹ��� ó���ϵ��� ����
                    popUpView.yesOrNoPopUp.SetYesButtonCallBack(
                        () =>
                        {
                            // ������ �ݹ� ����
                            itemInfo.itemCallback();

                            // �Ҹ��� ��� ������ ���� �Ǵ� ���� ����
                            if (itemInfo.isConsumable)
                                UsedByPlayer();

                            // �κ��丮���� ������ĭ�� Ŭ�� �����ϵ���
                            UnselectThisButton();

                            popUpView.inventoryPopUp.descriptionPanel.ClearPanel();
                            popUpView.inventoryPopUp.RefreshPopUp();

                        }
                        );
                };
        }
        else useCheckCallback = null;
            
    }

    public void InitItemInfo(cItemInfo itemInfo)
    {
        // ������ ������ �ʱ�ȭ
        this.itemInfo = itemInfo;
    }
    public void InitItemImage()
    {
        // ������ �̹��� ��ü
        bool result = false;
        Sprite sprite = ItemImageResource.Instance.TryGetImage(itemInfo.type, out result);
        if (result)
        {
            itemImage.sprite = sprite;
        }
    }
    public void ClearItemImage()
    {
        itemImage.sprite = null;
    }

    public void UsedByPlayer()
    {
        ItemManager.Instance.PlayerLoseItem(item);
    }

    public void SoldByPlayer()
    {
        ItemManager.Instance.PlayerLoseItem(item);

        cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.type);
        PlayManager.Instance.AddPlayerMoney(itemInfo.value_Sale);
    }

}
