using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class QuestPopUp : PopUpBase<QuestPopUp>
{
    [SerializeField] private QuestDescriptionPanel descriptionPanel;

    HashSet<sQuest> playerQuestHash
    {
        get { return QuestManager.questHashSet; }
    }

    protected override void Awake()
    {
        base.Awake();
        InitializePool(10);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        descriptionPanel.ClearPanel();
    }

    public override void RefreshPopUp()
    {
        Debug.Log($"playerQuestHash.Count == {playerQuestHash.Count}");
        RefreshPopUp(playerQuestHash.Count,
            () =>
            {
                foreach (sQuest quest in playerQuestHash)
                {
                    // 아이템정보로 초기화될 객체
                    QuestElementPanel questPanel = ActiveObjList[quest.id].GetComponent<QuestElementPanel>(); ;

                    // 아이템 종합정보를 호출
                    cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);

                    // 활성화된 각 객체에 정보를 초기화
                    if (questInfo != null)
                    {
                        questPanel.SetQuestdata(quest, questInfo);
                        questPanel.InitPanel();

                        // 퀘스트 항목을 클릭시 호출
                        questPanel.SetCallback(
                            () =>
                            {
                                descriptionPanel.SetPanel(questInfo);
                            });
                    }
                    else
                    {
                        Debug.LogAssertion($"{questPanel.gameObject.name}은 QuestElementPanel == null");
                    }
                }
            });
    }
}
