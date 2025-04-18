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
                            itemInfo.itemCallback();

                            // 소모성인 경우 아이템 삭제 또는 개수 차감
                            if (itemInfo.isConsumable)
                                UsedByPlayer();

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

    public void InitItemInfo(cItemInfo itemInfo)
    {
        // 아이템 정보를 초기화
        this.itemInfo = itemInfo;
    }
    public void InitItemImage()
    {
        // 아이템 이미지 교체
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
