using PublicSet;

public class NPC_MunDuckBea : InteractableObject
{
    eTextScriptFile currentFile;
    eStage currentStage { get { return GameManager.Instance.currentStage; } }
    eStage LastStage;

    private void Start()
    {
        currentFile = eTextScriptFile.NPC_MunDuckBea_Encounter;
    }
    public override eTextScriptFile GetInteractableEnum()
    {
        // ���� ����� Encounter ���Ϲ�ȣ ��ȯ
        if (currentFile == eTextScriptFile.NPC_MunDuckBea_Encounter)
        {
            // ����Ʈ ������ ���
            sQuest quest = new sQuest(0, eQuestType.LetsLookAroundOutside);
            if (QuestManager.questHashSet.Contains(quest))
            {
                cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);
                if (!questInfo.isComplete) questInfo.endConditionCheck();
            }
                

            currentFile = eTextScriptFile.NPC_MunDuckBea_Acquaintance;
            return eTextScriptFile.NPC_MunDuckBea_Encounter;
        }

        // �׷��� ������ Acquaintance ���Ϲ�ȣ ��ȯ
        else
        {
            return currentFile;
        }
        
    }

    private void FixedUpdate()
    {
        // �������� ����� �ش罺�������� Encounter�� �����ϵ��� ����
        if (LastStage != currentStage)
        {
            LastStage = currentStage;
            currentFile = eTextScriptFile.NPC_MunDuckBea_Encounter;
        }
    }
}
