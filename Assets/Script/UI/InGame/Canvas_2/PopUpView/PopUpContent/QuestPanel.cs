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
    /// Setpanel����
    /// </summary>
    public void InitPanel()
    {
        Setpanel(questInfo.name);

        // Ȯ���� �ʿ��ϸ� ��ü�� Ȱ��ȭ ��Ű�� �׷��� ������ ��Ȱ��ȭ
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
