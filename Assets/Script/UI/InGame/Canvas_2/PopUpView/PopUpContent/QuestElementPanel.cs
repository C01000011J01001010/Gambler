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
    /// Setpanel����
    /// </summary>
    public void InitPanel()
    {
        Setpanel(questInfo.name);

        if (questInfo.isComplete == false)
        {
            TextOfStatus.color = Color.blue;
            TextOfStatus.text = "������";
        }
        else if (questInfo.isComplete && questInfo.hasReceivedReward == false)
        {
            TextOfStatus.color = Color.red;
            TextOfStatus.text = "����\n�ޱ�";
        }
        else if(questInfo.isComplete && questInfo.hasReceivedReward)
        {
            TextOfStatus.color = Color.black; 
            TextOfStatus.text = "�Ϸ�";
        }
    }
}
