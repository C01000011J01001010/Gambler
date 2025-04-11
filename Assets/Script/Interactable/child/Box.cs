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

            // �ڽ��� �������� ���� ���
            if (itemList == null || itemList.Count == 0)
                itemList = list;

            // �ڽ��� �̹� �������� �ִ� ���
            else itemList.AddRange(list);
        }
        else
        {
            Debug.LogError("�ڽ��� ���� ���빰�� �����ϴ�.");
        }
    }

    public void EmptyOutBox()
    {
        current = eTextScriptFile.Box_Empty;

        // �ڽ��� ����ִ� �������� �÷��̾ ȹ��
        foreach (var item in itemList)
        {
            ItemManager.Instance.PlayerGetItem(item);
        }
        itemList.Clear();
    }
}
