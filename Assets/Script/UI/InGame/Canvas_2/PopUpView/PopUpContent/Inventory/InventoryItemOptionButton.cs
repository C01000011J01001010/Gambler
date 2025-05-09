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
                    // 정말로 사용할건지 체크
                    popUpView.YesOrNoPopUpOpen();

                    // 팝업창이 켜진 후 아이템 사용 여부 출력
                    popUpView.yesOrNoPopUp.SetYesText("네");
                    popUpView.yesOrNoPopUp.UpdateMainDescription("해당 아이템을 사용하시겠습니까?");


                    // yes선택을 누를시 아이템 고유 콜백을 처리하도록 연결
                    popUpView.yesOrNoPopUp.SetYesButtonCallBack(
                        () =>
                        {
                            // 아이템 콜백 실행
                            info.itemCallback();

                            // 소모성인 경우 아이템 삭제 또는 개수 차감
                            if (info.isConsumable)
                                ItemUsedByPlayer();

                            // 인벤토리에서 아이템칸이 클릭 가능하도록
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
        // 아이템 이미지 교체
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
