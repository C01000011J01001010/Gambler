using PublicSet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    // 현재 플레이어가 소유하고있는 아이템
    // HashSet(집합) : 중복되는 데이터는 무시함
    // 참조타입을 자료형 파라미터에 넣을경우 참조(주소)를 비교하여 실 데이터가 중복될 수 있음
    public static HashSet<sItem> ItemHashSet { get; private set; }

    public static void HashSetAllClear()
    {
        ItemHashSet.Clear();
    }

    protected override void Awake()
    {
        base.Awake();
        ItemHashSet = new HashSet<sItem>();
    }

    public int GetNewLastId()
    {
        int LastitemID = GetLastItemId();
        Debug.Log($"GetNewLastId에서 반환되는 id : {LastitemID + 1}");
        return LastitemID + 1;
    }
    public int GetLastItemId()
    {
        if (ItemHashSet.Count == 0)
        {
            Debug.Log("저장된 데이터가 없음");
            return -1;
        }

        int maxItemNumber = int.MinValue;

        foreach (sItem item in ItemHashSet)
        {
            if (item.id > maxItemNumber)
            {
                maxItemNumber = item.id;
            }
        }
        return maxItemNumber;
    }

    // 새로운 아이템 정보를 저장하기 위한 함수
    public void PlayerGetItem(eItemType itemType)
    {
        int itemId = GetNewLastId();

        // 입력된 아이템 코드에 맞는 임시데이터를 생성
        sItem newItem = new sItem(itemId, itemType);

        // 지금 입력된 아이템이 존재하는지 여부를 판단
        if (ItemHashSet.Contains(newItem))
        {
            // 이미 존재하면 추가하지 않음
            Debug.LogWarning($"Item {itemId} 는 이미 존재하는 아이템 항목.");
            return;
        }
        // 존재하지 않는다면 아이템 목록에 추가
        ItemHashSet.Add(newItem);
        Debug.Log($"Item {itemId} 획득 성공.");

        // 플레이어가 획득한 아이템을 확인하도록 유도
        cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(itemType);
        itemInfo.isNeedCheck = true;
    }

    public void PlayerLoseItem(sItem item)
    {
        Debug.Log($"item {item.ToString()} 삭제 시도");

        // 지금 입력된 아이템이 존재하는지 여부를 판단
        if (ItemHashSet.Contains(item) == false)
        {
            // 존재하지 않는 버그 아이템이라면 실행을 취소
            Debug.LogWarning($"Item {item.id}은 존재하지 않는 데이터");
            return;
        }

        // 존재하는 아이템이면 목록에서 제거
        ItemHashSet.Remove(item);

        // 리스트화
        List<sItem> itemList = ItemHashSet.ToList();

        // 삭제되는 아이템의 인덱스 위치에서 뒷쪽 인덱스를 앞으로 땡김
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].id > item.id)
            {
                // 값타입이니 내용을 변경후 구조체를 할당
                sItem temp = itemList[i];
                temp.id--;
                itemList[i] = temp;
            }
        }

        // 다시 HashSet으로 복원
        ItemHashSet = new HashSet<sItem>(itemList);

        Debug.Log($"Item {item.ToString()}  제거 성공함");
    }
}
