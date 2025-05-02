using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    /// <summary>
    /// �Ϸ�Ǿ��� ����Ʈ�� ������ ��� ����Ʈ
    /// </summary>
    public static HashSet<sQuest> questHashSet { get; private set; }

    /// <summary>
    /// �ݺ� ������ ����Ʈ�� ����
    /// </summary>
    public static HashSet<cQuestInfo> repeatableQuestInfo {  get; private set; }

    public static void HashSetAllClear()
    {
        questHashSet.Clear();
        repeatableQuestInfo.Clear();
    }

    protected override void Awake()
    {
        base.Awake();
        questHashSet = new HashSet<sQuest>();
        repeatableQuestInfo = new HashSet<cQuestInfo>();
    }

    public int GetNewLastId()
    {
        int LastitemID = GetLastItemId();
        Debug.Log($"GetNewLastId���� ��ȯ�Ǵ� id : {LastitemID + 1}");
        return LastitemID + 1;
    }
    public int GetLastItemId()
    {
        if (questHashSet.Count == 0)
        {
            Debug.Log("����� �����Ͱ� ����");
            return -1;
        }

        int maxItemNumber = int.MinValue;

        foreach (sQuest quest in questHashSet)
        {
            if (quest.id > maxItemNumber)
            {
                maxItemNumber = quest.id;
            }
        }
        return maxItemNumber;
    }

    
    public bool TryPlayerGetQuest(eQuestType questType)
    {
        int questId = GetNewLastId();

        sQuest newQuest = new sQuest(questId, questType);

        // type�� ���ϱ� ������ ���� ����Ʈ�� ���ֵ��� ����
        if (questHashSet.Contains(newQuest))
        {
            // �̹� �����ϸ� �߰����� ����
            Debug.LogWarning($"Quest {questId} �� ���ֹ��� ����Ʈ.");
            return false;
        }

        questHashSet.Add(newQuest);
        Debug.Log($"Quest {questId} ���� ����.");

        // �÷��̾ ������ ����Ʈ�� Ȯ���ϵ��� ����
        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(questType);
        questInfo.isNeedCheck = true;

        return true;
    }
    
    public bool TryPlayerCompleteQuest(eQuestType questType)
    {
        // ���ֵ� ����Ʈ���� Ȯ��
        sQuest quest = new sQuest(0,questType);
        if(questHashSet.Contains(quest) == false)
        {
            Debug.LogWarning("�������� ���� ����Ʈ");
            return false;
        }
        

        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(questType);
        questInfo.isComplete = true;
        

        // ������ ���� ���� ���� �ٷ� �Ϸ���°� �ǵ��� ����
        if (questInfo.rewardCoin == 0 && questInfo.rewardItemType == eItemType.None)
        {
            questInfo.hasReceivedReward = true;
        }

        // �÷��̾ �Ϸ��� ����Ʈ�� Ȯ���ϰ� ������ �޵��� ����
        questInfo.isNeedCheck = true;

        // �ݺ� ������ ����Ʈ�̸� �������� �Ѿ �� �ʱ�ȭ �ϱ� ���� ���� ����
        TryAddRefeatableQuest(questInfo);

        return true;
    }

    public static bool TryAddRefeatableQuest(cQuestInfo questInfo)
    {
        if (questInfo.isRepeatable)
        {
            repeatableQuestInfo.Add(questInfo);
            return true;
        }
        return false;
    }

    /// <summary>
    /// �ݺ�����Ʈ�� ���� ��� ��¥�� �ٲ𶧸��� �ʱ�ȭ��
    /// </summary>
    public static void InitRefeatableQuest()
    {
        bool isChanged = false;

        // �ݺ� ������ ����Ʈ�� �ʱ�ȭ
        foreach (cQuestInfo questInfo in repeatableQuestInfo)
        {
            //������ ���� ��쿡�� �ʱ�ȭ
            if (questInfo.hasReceivedReward)
            {
                questInfo.isComplete = false;
                questInfo.hasReceivedReward = false;
                questInfo.isNeedCheck = true;

                isChanged = true;
            }
        }

        // ���� ���� ������ ��� �ݿ�
        if (isChanged) GameManager.connector_InGame.popUpViewAsInGame.questPopUp.RefreshPopUp();
    }
}
