using PublicSet;
using UnityEngine;
using UnityEngine.UI;
using static PublicSet.INeedCheck;

public class QuestOptionButton : PopUpOptionButtonBase<QuestOptionButton, cPlayerQuest,  cQuestInfo>
{
    [SerializeField] private Text TextOfStatus;
    
    /// <summary>
    /// Setpanel포함
    /// </summary>
    public override void InitPanel()
    {
        Setpanel_Text(info.name);

        // 확인이 필요하면 객체를 활성화 시키고 그렇지 않으면 비활성화
        ClickCheck(eIcon.Quest);

        // 기타 정보를 처리
        InitQuestStatus();
    }

    private void InitQuestStatus()
    {
        if (info.isComplete == false)
        {
            TextOfStatus.color = Color.blue;
            TextOfStatus.text = "진행중";
        }
        else if (info.isComplete && info.hasReceivedReward == false)
        {
            TextOfStatus.color = Color.red;
            TextOfStatus.text = "보상\n받기";
        }
        else if (info.isComplete && info.hasReceivedReward)
        {
            TextOfStatus.color = Color.black;
            TextOfStatus.text = "완료";
        }
    }
}
