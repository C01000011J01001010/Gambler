using PublicSet;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static PublicSet.iNeedCheck;

public class ItemOptionButton : PopUpOptionButtonBase<ItemOptionButton, sItem, cItemInfo>
{
    [SerializeField] private Image itemImage;

    private PopUpView_InGame popUpView
    {
        get
        {
            return GameManager.connector_InGame.popUpView_Script;
        }
    }

    public UnityAction useCheckCallback;



    public override void InitPanel()
    {
        InitItemImage();

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
        ItemManager.Instance.PlayerLoseItem(defaultData);
    }

    public void ItemSoldByPlayer()
    {
        ItemManager.Instance.PlayerLoseItem(defaultData);

        cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(defaultData.type);
        PlayManager.Instance.AddPlayerMoney(itemInfo.value_Sale);
    }

}
