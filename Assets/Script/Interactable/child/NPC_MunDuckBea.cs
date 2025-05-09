using PublicSet;

public class NPC_MunDuckBea : DynamicInteractableBase
{
    eStage currentStage { get { return GameManager.Instance.currentStage; } }
    eStage LastStage;

    private void Start()
    {
        defaultFile = eTextScriptFile.NPC_MunDuckBea_Encounter;
        LastStage = currentStage;
    }
    public override eTextScriptFile GetInteractableEnum()
    {
        InitCurrentFile();

        // ���� ����� Encounter ���Ϲ�ȣ ��ȯ
        if (currentFile == eTextScriptFile.NPC_MunDuckBea_Encounter)
        {
            // ����Ʈ ������ ���
            //cPlayerQuest quest = new cPlayerQuest(0, eQuestType.LetsLookAroundOutside);
            eQuestType questType = eQuestType.LetsLookAroundOutside;
            if (QuestManager.Instance.PlayerQuestDict.ContainsKey(questType))
            {
                cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(questType);
                if (!questInfo.isComplete) questInfo.checkEndCondition();
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

    private void InitCurrentFile()
    {
        // �������� ����� �ش罺�������� Encounter�� �����ϵ��� ����
        if (LastStage != currentStage)
        {
            LastStage = currentStage;
            currentFile = eTextScriptFile.NPC_MunDuckBea_Encounter;
        }
    }
}
