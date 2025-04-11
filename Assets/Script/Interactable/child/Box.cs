using UnityEngine;
using PublicSet;
using System.Collections.Generic;
public class Box : InteractableObject
{
    eTextScriptFile current;
    public List<eItemType> itemList;

    private void Start()
    {
        current = eTextScriptFile.None;
    }

    public override eTextScriptFile GetInteractableEnum()
    {
        return current;
    }

#nullable enable
    public void FillUpBox(List<eItemType> list)
    {
        if(list != null && list.Count >0)
        {
            current = eTextScriptFile.Box_Full;

            // 박스에 아이템이 없는 경우
            if (itemList == null || itemList.Count == 0)
                itemList = list;

            // 박스에 이미 아이템이 있는 경우
            else itemList.AddRange(list);
        }
        else
        {
            Debug.LogError("박스에 넣을 내용물이 없습니다.");
        }
    }

    public void EmptyOutBox()
    {
        current = eTextScriptFile.Box_Empty;

        // 박스에 들어있는 아이템을 플레이어가 획득
        foreach (var item in itemList)
        {
            ItemManager.Instance.PlayerGetItem(item);
        }
        itemList.Clear();
    }
}
