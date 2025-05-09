using PublicSet;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : EntryDictManagerBase<ItemManager, cPlayerItem, eItemType>
{
    // ���� �÷��̾ �����ϰ��ִ� ������

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

    // ���ο� ������ ������ �����ϱ� ���� �Լ�
    public void PlayerGetItem(eItemType itemType)
    {
        // ���� �Էµ� �������� �����ϴ��� ���θ� �Ǵ�
        if (PlayerItemDict.ContainsKey(itemType))
        {
            // �̹� �����ϸ� ������ �ø��� ����
            PlayerItemDict[itemType].quantity++;
            return;
        }

        // �������� �ʴ´ٸ� ������ ��Ͽ� �߰�
        int itemId = GetNewLastId();
        cPlayerItem newItem = new cPlayerItem(itemId, itemType, 1);
        PlayerItemDict.Add(itemType, newItem);
        Debug.Log($"Item {itemId} ȹ�� ����.");

        // �÷��̾ ȹ���� �������� Ȯ���ϵ��� ����
        cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(itemType);
        itemInfo.isNeedCheck = true;
    }

    public void PlayerLoseItem(cPlayerItem item)
    {
        Debug.Log($"item {item.ToString()} ���� �õ�");

        // ���� �Էµ� �������� �����ϴ��� ���θ� �Ǵ�
        if (PlayerItemDict.ContainsKey(item.type) == false)
        {
            // �������� �ʴ� ���� �������̶�� ������ ���
            Debug.LogWarning($"Item {item.id}�� �������� �ʴ� ������");
            return;
        }

        // �����ϴ� �������̸� ������ 1�� ����
        PlayerItemDict[item.type].quantity--; 

        // �������� 0���� �Ǹ� �������� ������ ��ųʸ����� ����
        if (PlayerItemDict[item.type].quantity == 0)
        {
            PlayerItemDict.Remove(item.type);
        }
        
        Debug.Log($"Item {item.ToString()}  ���� ������");
    }
}
