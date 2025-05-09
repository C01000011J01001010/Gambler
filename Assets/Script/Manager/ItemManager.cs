using PublicSet;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : EntryDictManagerBase<ItemManager, cPlayerItem, eItemType>
{
    // 현재 플레이어가 소유하고있는 아이템

    public Dictionary<eItemType, cPlayerItem> PlayerItemDict
    {
        get => EntryDict;
        private set => EntryDict = value;
    }

    public override void InitAllDict()
    {
        PlayerItemDict = new Dictionary<eItemType, cPlayerItem>();
    }
    public override void ClearAllDict()
    {
        PlayerItemDict.Clear();
    }

    // 새로운 아이템 정보를 저장하기 위한 함수
    public void PlayerGetItem(eItemType itemType)
    {
        // 지금 입력된 아이템이 존재하는지 여부를 판단
        if (PlayerItemDict.ContainsKey(itemType))
        {
            // 이미 존재하면 개수를 늘리고 종료
            PlayerItemDict[itemType].quantity++;
            return;
        }

        // 존재하지 않는다면 아이템 목록에 추가
        int itemId = GetNewLastId();
        cPlayerItem newItem = new cPlayerItem(itemId, itemType, 1);
        PlayerItemDict.Add(itemType, newItem);
        Debug.Log($"Item {itemId} 획득 성공.");

        // 플레이어가 획득한 아이템을 확인하도록 유도
        cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(itemType);
        itemInfo.isNeedCheck = true;
    }

    public void PlayerLoseItem(cPlayerItem item)
    {
        Debug.Log($"item {item.ToString()} 삭제 시도");

        // 지금 입력된 아이템이 존재하는지 여부를 판단
        if (PlayerItemDict.ContainsKey(item.type) == false)
        {
            // 존재하지 않는 버그 아이템이라면 실행을 취소
            Debug.LogWarning($"Item {item.id}은 존재하지 않는 데이터");
            return;
        }

        // 존재하는 아이템이면 개수를 1개 줄임
        PlayerItemDict[item.type].quantity--; 

        // 아이템이 0개가 되면 존재하지 않으니 딕셔너리에서 제거
        if (PlayerItemDict[item.type].quantity == 0)
        {
            PlayerItemDict.Remove(item.type);
        }
        
        Debug.Log($"Item {item.ToString()}  제거 성공함");
    }
}
