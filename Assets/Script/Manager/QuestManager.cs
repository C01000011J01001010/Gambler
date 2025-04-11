using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    /// <summary>
    /// �Ϸ�Ǿ��� ����Ʈ�� ������ ��� ����Ʈ
    /// </summary>
    static public HashSet<sQuest> questHashSet { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        questHashSet = new HashSet<sQuest>();
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

    
    public void PlayerGetQuest(eQuestType questType)
    {
        int questId = GetNewLastId();

        sQuest newQuest = new sQuest(questId, questType);

        // type�� ���ϱ� ������ ���� ����Ʈ�� ���ֵ��� ����
        if (questHashSet.Contains(newQuest))
        {
            // �̹� �����ϸ� �߰����� ����
            Debug.LogWarning($"Quest {questId} �� ���ֹ��� ����Ʈ.");
            return;
        }

        questHashSet.Add(newQuest);
        Debug.Log($"Quest {questId} ���� ����.");

        GameManager.connector_InGame.popUpView_Script.questListPopUp.RefreshPopUp();
    }


    public void PlayerCompleteQuest(eQuestType questType)
    {
        // ���ֵ� ����Ʈ���� Ȯ��
        sQuest quest = new sQuest(0,questType);
        if(questHashSet.Contains(quest) == false)
        {
            Debug.LogWarning("�������� ���� ����Ʈ");
            return;
        }
        

        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(questType);
        questInfo.isComplete = true;

        // ������ ���� ���� ���� �ٷ� �Ϸ���°� �ǵ��� ����
        if (questInfo.rewardCoin == 0 && questInfo.rewardItemType == eItemType.None)
            questInfo.hasReceivedReward = true;

        EventManager.Instance.SetEventMessage($"����Ʈ\n{questInfo.name}\n����!");
        EventManager.Instance.PlaySequnce_EventAnimation();

        GameManager.connector_InGame.popUpView_Script.questListPopUp.RefreshPopUp();
    }

}
