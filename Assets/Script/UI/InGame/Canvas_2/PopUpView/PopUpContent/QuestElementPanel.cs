using PublicSet;
using UnityEngine;
using UnityEngine.UI;

public class QuestElementPanel : Selection_ButtonBase<QuestElementPanel>
{
    public sQuest quest;
    public cQuestInfo questInfo {  get; private set; }
    public Text TextOfStatus;


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
