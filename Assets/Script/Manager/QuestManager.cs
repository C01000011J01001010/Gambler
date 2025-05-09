using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : EntryDictManagerBase<QuestManager, cPlayerQuest, eQuestType>
{
    /// <summary>
    /// �Ϸ�Ǿ��� ����Ʈ�� ������ ��� ����Ʈ
    /// </summary>
    public Dictionary<eQuestType, cPlayerQuest> PlayerQuestDict
    {
        get => EntryDict;
        private set => EntryDict = value;
    }

    /// <summary>
    /// �ݺ� ������ ����Ʈ�� ������ ����
    /// </summary>
    public HashSet<cQuestInfo> repeatableQuestInfo {  get; private set; }


    public override void InitAllDict()
    {
        PlayerQuestDict = new Dictionary<eQuestType, cPlayerQuest>();
        repeatableQuestInfo = new HashSet<cQuestInfo>();
    }
    public override void ClearAllDict()
    {
        PlayerQuestDict.Clear();
        repeatableQuestInfo.Clear();
    }

    public bool TryPlayerGetQuest(eQuestType questType)
    {
        // type�� ���ϱ� ������ ���� ����Ʈ�� ���ֵ��� ����
        if (PlayerQuestDict.ContainsKey(questType))
        {
            // �̹� �����ϸ� �߰����� ����
            Debug.LogWarning($"Quest {questType} �� ���ֹ��� ����Ʈ.");
            return false;
        }


        int questId = GetNewLastId();
        cPlayerQuest newQuest = new cPlayerQuest(questId, questType);
        PlayerQuestDict.Add(questType, newQuest);
        Debug.Log($"Quest {questId} ���� ����.");

        // �÷��̾ ������ ����Ʈ�� Ȯ���ϵ��� ����
        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(questType);
        questInfo.isNeedCheck = true;

        return true;
    }
    
    public bool TryPlayerCompleteQuest(eQuestType questType)
    {
        // ���ֵ� ����Ʈ���� Ȯ��
        if (PlayerQuestDict.ContainsKey(questType) == false)
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

    public bool TryAddRefeatableQuest(cQuestInfo questInfo)
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
    public void InitRefeatableQuest()
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
