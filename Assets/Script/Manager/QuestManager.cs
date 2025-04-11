using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    /// <summary>
    /// 완료되었던 퀘스트를 포함한 모든 퀘스트
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

    
    public void PlayerGetQuest(eQuestType questType)
    {
        int questId = GetNewLastId();

        sQuest newQuest = new sQuest(questId, questType);

        // type만 비교하기 때문에 같은 퀘스트는 수주되지 않음
        if (questHashSet.Contains(newQuest))
        {
            // 이미 존재하면 추가하지 않음
            Debug.LogWarning($"Quest {questId} 는 수주받은 퀘스트.");
            return;
        }

        questHashSet.Add(newQuest);
        Debug.Log($"Quest {questId} 수주 성공.");

        GameManager.connector_InGame.popUpView_Script.questListPopUp.RefreshPopUp();
    }


    public void PlayerCompleteQuest(eQuestType questType)
    {
        // 수주된 퀘스트인지 확인
        sQuest quest = new sQuest(0,questType);
        if(questHashSet.Contains(quest) == false)
        {
            Debug.LogWarning("수주하지 않은 퀘스트");
            return;
        }
        

        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(questType);
        questInfo.isComplete = true;

        // 보상이 따로 없는 경우는 바로 완료상태가 되도록 설정
        if (questInfo.rewardCoin == 0 && questInfo.rewardItemType == eItemType.None)
            questInfo.hasReceivedReward = true;

        EventManager.Instance.SetEventMessage($"퀘스트\n{questInfo.name}\n성공!");
        EventManager.Instance.PlaySequnce_EventAnimation();

        GameManager.connector_InGame.popUpView_Script.questListPopUp.RefreshPopUp();
    }

}
