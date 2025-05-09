using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class QuestPopUp : PopUpBase_FullScreen<QuestPopUp>
{
    [SerializeField] protected QuestDescriptionPanel descriptionPanel;
    
    Dictionary<eQuestType, cPlayerQuest> playerQuestDict
    {
        get { return QuestManager.Instance.PlayerQuestDict; }
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
        Debug.Log($"playerQuestHash.Count == {playerQuestDict.Count}");
        RefreshPopUp(playerQuestDict.Count,
            () =>
            {
                foreach (eQuestType questType in playerQuestDict.Keys)
                {
                    cPlayerQuest quest = playerQuestDict[questType];

                    // ������������ �ʱ�ȭ�� ��ü
                    QuestOptionButton questPanel = ActiveObjList[quest.id].GetComponent<QuestOptionButton>(); ;

                    // ������ ���������� ȣ��
                    cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(questType);

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
