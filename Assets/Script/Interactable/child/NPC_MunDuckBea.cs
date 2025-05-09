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

        // 최초 조우시 Encounter 파일번호 반환
        if (currentFile == eTextScriptFile.NPC_MunDuckBea_Encounter)
        {
            // 퀘스트 수주한 경우
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

        // 그렇지 않으면 Acquaintance 파일번호 반환
        else
        {
            return currentFile;
        }
        
    }

    private void InitCurrentFile()
    {
        // 스테이지 변경시 해당스테이지의 Encounter를 시작하도록 조정
        if (LastStage != currentStage)
        {
            LastStage = currentStage;
            currentFile = eTextScriptFile.NPC_MunDuckBea_Encounter;
        }
    }
}
