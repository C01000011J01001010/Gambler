using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : EntryDictManagerBase<QuestManager, cPlayerQuest, eQuestType>
{
    /// <summary>
    /// 완료되었던 퀘스트를 포함한 모든 퀘스트
    /// </summary>
    public Dictionary<eQuestType, cPlayerQuest> PlayerQuestDict
    {
        get => EntryDict;
        private set => EntryDict = value;
    }

    /// <summary>
    /// 반복 가능한 퀘스트를 별도로 관리
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
        // type만 비교하기 때문에 같은 퀘스트는 수주되지 않음
        if (PlayerQuestDict.ContainsKey(questType))
        {
            // 이미 존재하면 추가하지 않음
            Debug.LogWarning($"Quest {questType} 는 수주받은 퀘스트.");
            return false;
        }


        int questId = GetNewLastId();
        cPlayerQuest newQuest = new cPlayerQuest(questId, questType);
        PlayerQuestDict.Add(questType, newQuest);
        Debug.Log($"Quest {questId} 수주 성공.");

        // 플레이어가 수주한 퀘스트를 확인하도록 유도
        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(questType);
        questInfo.isNeedCheck = true;

        return true;
    }
    
    public bool TryPlayerCompleteQuest(eQuestType questType)
    {
        // 수주된 퀘스트인지 확인
        if (PlayerQuestDict.ContainsKey(questType) == false)
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
    /// 반복퀘스트가 있을 경우 날짜가 바뀔때마다 초기화됨
    /// </summary>
    public void InitRefeatableQuest()
    {
        bool isChanged = false;

        // 반복 가능한 퀘스트의 초기화
        foreach (cQuestInfo questInfo in repeatableQuestInfo)
        {
            //보상을 받은 경우에만 초기화
            if (questInfo.hasReceivedReward)
            {
                questInfo.isComplete = false;
                questInfo.hasReceivedReward = false;
                questInfo.isNeedCheck = true;

                isChanged = true;
            }
        }

        // 변경 사항 있으면 즉시 반영
        if (isChanged) GameManager.connector_InGame.popUpViewAsInGame.questPopUp.RefreshPopUp();
    }
}
