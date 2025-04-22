using PublicSet;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : Selection_ButtonBase<QuestPanel>
{
    public Text TextOfStatus;
    public GameObject clickGuide;
    
    


    public sQuest quest { get; private set; }
    public cQuestInfo questInfo { get; private set; }

    public void SetQuestdata(sQuest quest, cQuestInfo cQuest)
    {
        this.quest = new sQuest(quest);
        questInfo = cQuest;
    }

    /// <summary>
    /// Setpanel포함
    /// </summary>
    public void InitPanel()
    {
        Setpanel(questInfo.name);

        // 확인이 필요하면 객체를 활성화 시키고 그렇지 않으면 비활성화
        if(questInfo.isNeedCheck) 
        {
            GameManager.connector_InGame.iconView_Script.TryClickGuideOn(eIcon.Quest);
            if (clickGuide.activeInHierarchy == false) 
                clickGuide.SetActive(true);
        }
        else
        {
            GameManager.connector_InGame.iconView_Script.TryClickGuideOff(eIcon.Quest);
            if (clickGuide.activeInHierarchy) 
                clickGuide.SetActive(false);
        }
        

        if (questInfo.isComplete == false)
        {
            TextOfStatus.color = Color.blue;
            TextOfStatus.text = "진행중";
        }
        else if (questInfo.isComplete && questInfo.hasReceivedReward == false)
        {
            TextOfStatus.color = Color.red;
            TextOfStatus.text = "보상\n받기";
        }
        else if(questInfo.isComplete && questInfo.hasReceivedReward)
        {
            TextOfStatus.color = Color.black; 
            TextOfStatus.text = "완료";
        }
    }
}
