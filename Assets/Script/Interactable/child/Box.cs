using UnityEngine;
using PublicSet;
using System.Collections.Generic;
public class Box : DynamicInteractableBase
{
    public List<eItemType> itemList;

    private void Start()
    {
        defaultFile = eTextScriptFile.Box_Empty;
    }

    public override eTextScriptFile GetInteractableEnum()
    {
        return currentFile;
    }

#nullable enable
    public void FillUpBox(List<eItemType> list)
    {
        // 플레이어가 상호작용 할 수 있도록 태그 변경
        gameObject.tag = "Interactable";

        if (list != null && list.Count >0)
        {
            currentFile = eTextScriptFile.Box_Full;

            // 박스에 아이템이 없는 경우
            if (itemList == null || itemList.Count == 0)
                itemList = list;

            // 박스에 이미 아이템이 있는 경우
            else itemList.AddRange(list);

            Debug.Log("아이템이 상자에 채워졌습니다.");
            for (int i = 0; i < itemList.Count; i++)
                Debug.Log($"현재박스에 채워진 아이템 -> {i}. {itemList[i].ToString()}");
        }
        else
        {
            Debug.LogError("박스에 넣을 내용물이 없습니다.");
        }
    }

    public void EmptyOutBox()
    {
        // 플레이어가 상호작용 할 수 있도록 태그 변경
        gameObject.tag = "Interactable";

        currentFile = eTextScriptFile.Box_Empty;

        // 박스에 들어있는 아이템을 플레이어가 획득
        foreach (var item in itemList)
        {
            ItemManager.Instance.PlayerGetItem(item);
        }
        itemList.Clear();
    }
}
