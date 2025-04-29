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

    /// <summary>
    /// Ŭ�����̵� Ȱ��ȭ�� questPanel.InitPanel()�� ClickCheck(eIcon.Quest)���� ������ <br/>
    /// �ٹٲ� �׽�Ʈ
    /// </summary>
    public override void RefreshPopUp()
    {
        Debug.Log($"playerQuestHash.Count == {playerQuestHash.Count}");
        RefreshPopUp(playerQuestHash.Count,
            () =>
            {
                foreach (sQuest quest in playerQuestHash)
                {
                    // ������������ �ʱ�ȭ�� ��ü
                    QuestOptionButton questPanel = ActiveObjList[quest.id].GetComponent<QuestOptionButton>(); ;

                    // ������ ���������� ȣ��
                    cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);

                    // Ȱ��ȭ�� �� ��ü�� ������ �ʱ�ȭ
                    if (questInfo != null)
                    {
                        questPanel.SetData(quest, questInfo);
                        questPanel.InitPanel();

                        // ����Ʈ �׸��� Ŭ���� ȣ��
                        questPanel.SetCallback(questPanel,
                            () =>
                            {
                                // ����Ʈ�� ���� ������ ȭ�鿡 ǥ��
                                descriptionPanel.SetPanel(questInfo);

                                // ����Ʈ�� Ȯ�������� ���� �缳��
                                // ����Ʈ �Ϸ� �� Ȯ�� �������� �������
                                if (questInfo.isComplete == false)
                                {
                                    questInfo.isNeedCheck = false; 
                                    RefreshPopUp();
                                }

                                // ������ ���� ��� Ȯ�ν� �ٷ� ó��
                                else if(questInfo.hasReceivedReward)
                                {
                                    questInfo.isNeedCheck = false;
                                    RefreshPopUp();
                                }
                            }
                            );
                    }
                    else
                    {
                        Debug.LogAssertion($"{questPanel.gameObject.name}�� QuestElementPanel == null");
                    }
                }
            });

        
    }
}
