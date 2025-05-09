using PublicSet;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryItemOptionButton : PopUpOptionButtonBase<InventoryItemOptionButton, cPlayerItem, cItemInfo>
{
    [SerializeField] private Image itemImage;

    private PopUpView_InGame popUpView
    {
        get
        {
            return GameManager.connector_InGame.popUpViewAsInGame;
        }
    }

    public UnityAction useCheckCallback;



    public override void InitPanel()
    {
        InitItemImage();

        ClickCheck(eIcon.Inventory);

        InitUseCheckCallback();
    }

    public void InitPanel(object PopUp)
    {
        InitItemImage();

        //if(ShopPopUp != typeof(PopUp))
            ClickCheck(eIcon.Inventory);

        InitUseCheckCallback();
    }

    private void InitUseCheckCallback()
    {
        if (info.isAvailable)
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
                            info.itemCallback();

                            // �Ҹ��� ��� ������ ���� �Ǵ� ���� ����
                            if (info.isConsumable)
                                ItemUsedByPlayer();

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

    public void InitItemImage()
    {
        // ������ �̹��� ��ü
        bool result = false;
        Sprite sprite = ItemImageResource.Instance.TryGetImage(base.info.type, out result);
        if (result)
        {
            itemImage.sprite = sprite;
        }
    }

    public void ClearItemImage()
    {
        itemImage.sprite = null;
    }

    public void ClearClickGuide()
    {
        if(clickGuide.activeInHierarchy)
            clickGuide.gameObject.SetActive(false);
    }

    public void ItemUsedByPlayer()
    {
        ItemManager.Instance.PlayerLoseItem(entryData);
    }

    public void ItemSoldByPlayer()
    {
        ItemManager.Instance.PlayerLoseItem(entryData);

        cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(entryData.type);
        PlayManager.Instance.AddPlayerMoney(itemInfo.value_Sale);
    }

}
