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
        // ���� ������ ������� Ȱ��ȭ
        if (questInfo.rewardCoin == 0) rewardCoin.SetActive(false);
        else
        {
            rewardCoin.SetActive(true);
            coinNum.text = $"x{questInfo.rewardCoin.ToString()}";
        }
        
        // ������ ������ ������� Ȱ��ȭ
        if(questInfo.rewardItemType == eItemType.None) rewardItem.SetActive(false);
        else
        {
            rewardItem.SetActive(true);
            cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(questInfo.rewardItemType);

            // ������ �̹��� ����
            Sprite itemSprite = ItemImageResource.Instance.TryGetImage(itemInfo.type, 0, out bool result);
            if (result)
            {
                itemImage.sprite = itemSprite;
            }

            // ������ �̸� ����
            itemName.text = $"{itemInfo.name}";
        }

    }
}
