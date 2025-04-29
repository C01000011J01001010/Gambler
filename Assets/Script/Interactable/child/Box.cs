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
        // �÷��̾ ��ȣ�ۿ� �� �� �ֵ��� �±� ����
        gameObject.tag = "Interactable";

        if (list != null && list.Count >0)
        {
            currentFile = eTextScriptFile.Box_Full;

            // �ڽ��� �������� ���� ���
            if (itemList == null || itemList.Count == 0)
                itemList = list;

            // �ڽ��� �̹� �������� �ִ� ���
            else itemList.AddRange(list);

            Debug.Log("�������� ���ڿ� ä�������ϴ�.");
            for (int i = 0; i < itemList.Count; i++)
                Debug.Log($"����ڽ��� ä���� ������ -> {i}. {itemList[i].ToString()}");
        }
        else
        {
            Debug.LogError("�ڽ��� ���� ���빰�� �����ϴ�.");
        }
    }

    public void EmptyOutBox()
    {
        // �÷��̾ ��ȣ�ۿ� �� �� �ֵ��� �±� ����
        gameObject.tag = "Interactable";

        currentFile = eTextScriptFile.Box_Empty;

        // �ڽ��� ����ִ� �������� �÷��̾ ȹ��
        foreach (var item in itemList)
        {
            ItemManager.Instance.PlayerGetItem(item);
        }
        itemList.Clear();
    }
}
