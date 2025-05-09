using PublicSet;
using UnityEngine;
using UnityEngine.UI;
using static PublicSet.INeedCheck;

public class QuestOptionButton : PopUpOptionButtonBase<QuestOptionButton, cPlayerQuest,  cQuestInfo>
{
    [SerializeField] private Text TextOfStatus;
    
    /// <summary>
    /// Setpanel����
    /// </summary>
    public override void InitPanel()
    {
        Setpanel_Text(info.name);

        // Ȯ���� �ʿ��ϸ� ��ü�� Ȱ��ȭ ��Ű�� �׷��� ������ ��Ȱ��ȭ
        ClickCheck(eIcon.Quest);

        // ��Ÿ ������ ó��
        InitQuestStatus();
    }

    private void InitQuestStatus()
    {
        if (info.isComplete == false)
        {
            TextOfStatus.color = Color.blue;
            TextOfStatus.text = "������";
        }
        else if (info.isComplete && info.hasReceivedReward == false)
        {
            TextOfStatus.color = Color.red;
            TextOfStatus.text = "����\n�ޱ�";
        }
        else if (info.isComplete && info.hasReceivedReward)
        {
            TextOfStatus.color = Color.black;
            TextOfStatus.text = "�Ϸ�";
        }
    }
}
