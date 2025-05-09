using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPopUp : ItemPopUpBase<InventoryPopUp, InventoryItemOptionButton, InventoryItemDescriptionPanel>
{
    protected override Dictionary<eItemType, cPlayerItem> PopUpItemDict => ItemManager.Instance.PlayerItemDict;
    public cPlayerItem currentClickItem { get; private set; } // 현재 클릭한 아이템
    //[SerializeField] private InventoryItemDescriptionPanel _itemDescriptionPanel;
    //public InventoryItemDescriptionPanel descriptionPanel => _itemDescriptionPanel;

    

    //Dictionary<int, sItem> playerItems => ItemManager.Instance.ItemHashSet;


    ///// <summary>
    ///// 값을 복사하기위한 대상
    ///// </summary>
    //private HashSet<int> itemSlotIndexHash { get; set; }

    ///// <summary>
    ///// popup은 기본적으로 비활성화 상태이니 popupView에서 초기화
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
    ///// 클릭가이드 활성화를 itemOption.InitPanel()의 ClickCheck(eIcon.Inventory)에서 수행함
    ///// </summary>
    //public override void RefreshPopUp()
    //{
    //    RefreshPopUp(itemSlotIndexHash.Count,
    //        () =>
    //        {
    //            // 초기화되지 않은 아이템칸을 확인하기 위한 해시셋
    //            HashSet<int> clearSlotHash = new HashSet<int>(itemSlotIndexHash);

    //            // 아이템칸 초기화
    //            foreach (sItem item in playerItems)
    //            {
    //                // 아이템정보로 초기화될 객체(아이템칸)
    //                InventoryItemOptionButton itemOption = ActiveObjList[item.id].GetComponent<InventoryItemOptionButton>();
    //                clearSlotHash.Remove(item.id); //초기화된 아이템칸의 인덱스는 해시셋에서 제거

    //                // 아이템 종합정보를 호출
    //                cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(item.type);

    //                // 활성화된 각 객체에 정보를 초기화
    //                if (itemOption != null)
    //                {
    //                    itemOption.SetData(item, itemInfo);
    //                    itemOption.InitPanel();

    //                    // 아이템 클릭시 판넬 설정
    //                    itemOption.SetCallback(
    //                        itemOption,
    //                        () =>
    //                        {
    //                            // 아이템에 대한 설명을 화면에 표시
    //                            descriptionPanel.SetPanel(itemOption, itemInfo);

    //                            // 아이템을 확인했으니 속성 변경후 리프레시
    //                            itemInfo.isNeedCheck = false; 
    //                            RefreshPopUp();

    //                        });
    //                }
    //                else
    //                {
    //                    Debug.LogAssertion($"{itemOption.gameObject.name}은 itemScript == null");
    //                }
    //            }

    //            // 비어있는 아이템칸 초기화
    //            foreach(int ClearSlot in clearSlotHash)
    //            {
    //                InventoryItemOptionButton itemOption = ActiveObjList[ClearSlot].GetComponent<InventoryItemOptionButton>();
    //                itemOption.ClearItemImage();
    //                itemOption.ClearClickGuide();
    //                itemOption.ClearButtonCallback(); // 아무런 상호작용이 불가능하도록 만듬
    //            }

    //        });
    //}

}
