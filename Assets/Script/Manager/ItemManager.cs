using PublicSet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    // ���� �÷��̾ �����ϰ��ִ� ������
    // HashSet(����) : �ߺ��Ǵ� �����ʹ� ������
    // ����Ÿ���� �ڷ��� �Ķ���Ϳ� ������� ����(�ּ�)�� ���Ͽ� �� �����Ͱ� �ߺ��� �� ����
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
        Debug.Log($"GetNewLastId���� ��ȯ�Ǵ� id : {LastitemID + 1}");
        return LastitemID + 1;
    }
    public int GetLastItemId()
    {
        if (ItemHashSet.Count == 0)
        {
            Debug.Log("����� �����Ͱ� ����");
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

    // ���ο� ������ ������ �����ϱ� ���� �Լ�
    public void PlayerGetItem(eItemType itemType)
    {
        int itemId = GetNewLastId();

        // �Էµ� ������ �ڵ忡 �´� �ӽõ����͸� ����
        sItem newItem = new sItem(itemId, itemType);

        // ���� �Էµ� �������� �����ϴ��� ���θ� �Ǵ�
        if (ItemHashSet.Contains(newItem))
        {
            // �̹� �����ϸ� �߰����� ����
            Debug.LogWarning($"Item {itemId} �� �̹� �����ϴ� ������ �׸�.");
            return;
        }
        // �������� �ʴ´ٸ� ������ ��Ͽ� �߰�
        ItemHashSet.Add(newItem);
        Debug.Log($"Item {itemId} ȹ�� ����.");

        // �÷��̾ ȹ���� �������� Ȯ���ϵ��� ����
        cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(itemType);
        itemInfo.isNeedCheck = true;
    }

    public void PlayerLoseItem(sItem item)
    {
        Debug.Log($"item {item.ToString()} ���� �õ�");

        // ���� �Էµ� �������� �����ϴ��� ���θ� �Ǵ�
        if (ItemHashSet.Contains(item) == false)
        {
            // �������� �ʴ� ���� �������̶�� ������ ���
            Debug.LogWarning($"Item {item.id}�� �������� �ʴ� ������");
            return;
        }

        // �����ϴ� �������̸� ��Ͽ��� ����
        ItemHashSet.Remove(item);

        // ����Ʈȭ
        List<sItem> itemList = ItemHashSet.ToList();

        // �����Ǵ� �������� �ε��� ��ġ���� ���� �ε����� ������ ����
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].id > item.id)
            {
                // ��Ÿ���̴� ������ ������ ����ü�� �Ҵ�
                sItem temp = itemList[i];
                temp.id--;
                itemList[i] = temp;
            }
        }

        // �ٽ� HashSet���� ����
        ItemHashSet = new HashSet<sItem>(itemList);

        Debug.Log($"Item {item.ToString()}  ���� ������");
    }
}
