

using PublicSet;

public class SaveButton : SaveAndContinue_ButtonBase
{
    public override void Callback()
    {
        string savedDate = PlayerSaveManager.Instance.LoadSavedDate(saveKey);
        if (savedDate != string.Empty) // �̹� ����� �����Ͱ� �ִ� ���
        {
            popUpView.YesOrNoPopUpOpen();
            yesOrNoPopUp.UpdateMainDescription("�̹� ����� �����Ͱ� �ֽ��ϴ�.\n�����͸� ���Ӱ� ����Ͻðڽ��ϱ�?");
            yesOrNoPopUp.SetYesText("��");
            yesOrNoPopUp.SetYesButtonCallBack(
                () =>
                {
                    SaveDataProcess();
                });
        }
        else //����� �����Ͱ� ���� ���
        {
            SaveDataProcess();
        }
        (popUpView as PopUpView_InGame).saveDataPopUp.RefreshPopUp();
    }

    private void SaveDataProcess()
    {
        // ����Ʈ ������ ���
        sQuest quest = new sQuest(0, eQuestType.LearnHowToSave);
        if (QuestManager.questHashSet.Contains(quest))
        {
            cQuestInfo quesInfo = CsvManager.Instance.GetQuestInfo(quest.type);
            if (quesInfo.isComplete == false) quesInfo.callback_endConditionCheck();
        }

        GameManager.Instance.SetPlayerSaveKey(saveKey);
        PlayerSaveManager.Instance.SaveTotalData();

        popUpView.CheckPopUpOpen();
        checkPopUp.UpdateMainDescription("�÷��̾� ������ ����Ǿ����ϴ�.");
        GameManager.connector_InGame.popUpView_Script.saveDataPopUp.RefreshPopUp();
    }
}
