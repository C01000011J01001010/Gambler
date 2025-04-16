using PublicSet;
using UnityEngine;
using UnityEngine.UI;

public class CaseQuest : MonoBehaviour
{
    public GameObject rewardCoin;
    public GameObject rewardItem;

    public Text coinNum;
    public Image itemImage;
    public Text itemName;
    


    public void SetPanel(cQuestInfo questInfo)
    {
        // 코인 보상이 있을경우 활성화
        if (questInfo.rewardCoin == 0) rewardCoin.SetActive(false);
        else
        {
            rewardCoin.SetActive(true);
            coinNum.text = $"x{questInfo.rewardCoin.ToString()}";
        }
        
        // 아이템 보상이 있을경우 활성화
        if(questInfo.rewardItemType == eItemType.None) rewardItem.SetActive(false);
        else
        {
            rewardItem.SetActive(true);
            cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(questInfo.rewardItemType);

            // 아이템 이미지 설정
            Sprite itemSprite = ItemImageResource.Instance.TryGetImage(itemInfo.type, 0, out bool result);
            if (result)
            {
                itemImage.sprite = itemSprite;
            }

            // 아이템 이름 설정
            itemName.text = $"{itemInfo.name}";
        }

    }
}
