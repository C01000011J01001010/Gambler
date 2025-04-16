using UnityEngine;
using UnityEngine.UI;
using PublicSet;
using System;
using Unity.VisualScripting;

public class QuestDescriptionPanel : MonoBehaviour
{
    [SerializeField] private Text questName;
    [SerializeField] private Text questDescription;
    [SerializeField] private Text questReward;
    [SerializeField] private RewardButton rewardButton;

    bool NeedLineFeed;
    bool coinDone;
    bool itemDone;


    public void SetPanel(cQuestInfo questInfo, Action TextSpacing = null)
    {
        // �ǳ��� �̸� ����
        questName.text = questInfo.name;

        // ������Ʈ���� �� ���徿 �������� ���� ���̿� ���๮�� ����
        questDescription.text = string.Empty;

        questDescription.text += $"{questInfo.descriptionList[0]}";
        for (int i = 1; i<questInfo.descriptionList.Count;i++)
        {
            if (TextSpacing != null) TextSpacing();

            questDescription.text += $"\n{questInfo.descriptionList[i]}";
        }


        // ����Ʈ ���� ����
        NeedLineFeed = false;
        coinDone = false;
        itemDone = false;
        questReward.text = string.Empty;

        if (questInfo.rewardCoin != 0)
        {
            questReward.text += $"���� {questInfo.rewardCoin.ToString()}��";
            coinDone = true;
            NeedLineFeed = true;
        }

        if(questInfo.rewardItemType != eItemType.None)
        {
            if(NeedLineFeed)
            {
                questReward.text += "\n"; // �� ���� �̻��϶��� ���๮�� ����
            }
            cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(questInfo.rewardItemType);
            questReward.text += $"������ {itemInfo.name}";
            itemDone = true;
        }

        // ������ 1���� �ִ� ���
        if (coinDone || itemDone)
        {
            rewardButton.BindQuestToButton(questInfo);
        }
        else // ������ ���� ���
        {
            questReward.text = "���𰡸� ���������� ��������?";
        }

        // ����Ʈ�� �Ϸ��ϰ� ������ �ȹ��� ���
        if (questInfo.isComplete && questInfo.hasReceivedReward == false)
        {
            rewardButton.TryActivate_Button();
        }
        else
        {
            rewardButton.TryDeactivate_Button();
        }
        
        
    }
    public void ClearPanel()
    {
        questName.text = "����Ʈ ����";
        questDescription.text = "������ �׸� Ŭ�� �� ������ Ȯ���Ͻ� �� �ֽ��ϴ�.";
        questReward.text = "���� ����";
        rewardButton.TryDeactivate_Button();
    }
}
