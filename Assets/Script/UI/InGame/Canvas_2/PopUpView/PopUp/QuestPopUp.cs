using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class QuestPopUp : PopUpBase_FullScreen<QuestPopUp>
{
    [SerializeField] protected QuestDescriptionPanel descriptionPanel;
    
    HashSet<sQuest> playerQuestHash
    {
        get { return QuestManager.questHashSet; }
    }
    

    private void OnEnable()
    {
        RefreshPopUp();
        ScrollToTop();
        descriptionPanel.ClearPanel();
    }
    protected override void Awake()
    {
        base.Awake();
        InitializePool(10);
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
                    QuestPanel questPanel = ActiveObjList[quest.id].GetComponent<QuestPanel>(); ;

                    // 아이템 종합정보를 호출
                    cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);

                    // 활성화된 각 객체에 정보를 초기화
                    if (questInfo != null)
                    {
                        questPanel.SetQuestdata(quest, questInfo);
                        questPanel.InitPanel();

                        // 퀘스트 항목을 클릭시 호출
                        questPanel.SetCallback(questPanel,
                            () =>
                            {
                                // 퀘스트에 대한 설명을 화면에 표시
                                descriptionPanel.SetPanel(questInfo);

                                // 퀘스트를 확인했으니 변수 재설정
                                // 퀘스트 완료 후 확인 유도에는 영향없음
                                if (questInfo.isComplete == false)
                                {
                                    questInfo.isNeedCheck = false; // foreach문이니 메소드로 수정함
                                    RefreshPopUp();
                                    
                                }

                                // 보상이 없는 경우 확인시 바로 처리
                                else if(questInfo.isComplete && questInfo.hasReceivedReward)
                                {
                                    questInfo.isNeedCheck = false;
                                    RefreshPopUp();
                                }
                            }
                            );
                    }
                    else
                    {
                        Debug.LogAssertion($"{questPanel.gameObject.name}은 QuestElementPanel == null");
                    }
                }
            });

        
    }
}
