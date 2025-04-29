using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    /// <summary>
    /// 완료되었던 퀘스트를 포함한 모든 퀘스트
    /// </summary>
    static public HashSet<sQuest> questHashSet { get; private set; }

    /// <summary>
    /// 반복 가능한 퀘스트를 관리
    /// </summary>
    static public HashSet<cQuestInfo> repeatableQuestInfo {  get; private set; }

    protected override void Awake()
    {
        base.Awake();
        questHashSet = new HashSet<sQuest>();
    }

    public int GetNewLastId()
    {
        int LastitemID = GetLastItemId();
        Debug.Log($"GetNewLastId에서 반환되는 id : {LastitemID + 1}");
        return LastitemID + 1;
    }
    public int GetLastItemId()
    {
        if (questHashSet.Count == 0)
        {
            Debug.Log("저장된 데이터가 없음");
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

        // type만 비교하기 때문에 같은 퀘스트는 수주되지 않음
        if (questHashSet.Contains(newQuest))
        {
            // 이미 존재하면 추가하지 않음
            Debug.LogWarning($"Quest {questId} 는 수주받은 퀘스트.");
            return false;
        }

        questHashSet.Add(newQuest);
        Debug.Log($"Quest {questId} 수주 성공.");

        // 플레이어가 수주한 퀘스트를 확인하도록 유도
        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(questType);
        questInfo.isNeedCheck = true;

        return true;
    }


    public bool TryPlayerCompleteQuest(eQuestType questType)
    {
        // 수주된 퀘스트인지 확인
        sQuest quest = new sQuest(0,questType);
        if(questHashSet.Contains(quest) == false)
        {
            Debug.LogWarning("수주하지 않은 퀘스트");
            return false;
        }
        

        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(questType);
        questInfo.isComplete = true;
        

        // 보상이 따로 없는 경우는 바로 완료상태가 되도록 설정
        if (questInfo.rewardCoin == 0 && questInfo.rewardItemType == eItemType.None)
        {
            questInfo.hasReceivedReward = true;
        }

        // 플레이어가 완료한 퀘스트를 확인하고 보상을 받도록 유도
        questInfo.isNeedCheck = true;

        // 반복 가능한 퀘스트이면 다음날로 넘어갈 때 초기화 하기 위해 따로 보관
        if (questInfo.isRepeatable && repeatableQuestInfo.Contains(questInfo) == false)
        {
            repeatableQuestInfo.Add(questInfo);
        }

        return true;
    }

}
