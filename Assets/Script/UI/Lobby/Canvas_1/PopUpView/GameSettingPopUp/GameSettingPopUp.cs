using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class GameSettingPopUp : PopUpBase_FullScreen<GameSettingPopUp>
{
    static readonly string[] GameSettingTitle =
    {
        "����� ����",
        //"���� ����",
        "������ �ʱ�ȭ"
    };

    // ������ �ʱ�ȭ�� ������ �ǳ�
    // ������ �ʱ�ȭ�� �����˾�â ���
    public GameObject[] GameSettingPanel; 

    private PopUPView_Lobby popUPView { get { return GameManager.connector_Lobby.popUpView_Script; } }

    private void OnEnable()
    {
        RefreshPopUp();
        ScrollToTop();

        // ���۽� ����� �����Ǹ� �ѱ�
        {
            int index = 0;
            for (int CloseIndex = 0; CloseIndex < GameSettingPanel.Length; CloseIndex++)
            {
                // ������ ���� ���ΰ�
                if (index == CloseIndex)
                    continue;

                // �� �ܿ� ��� ���� �ݱ�
                else if (GameSettingPanel[CloseIndex].activeInHierarchy)
                    GameSettingPanel[CloseIndex].SetActive(false);
            }
            GameSettingPanel[index].SetActive(true);
        }
    }

    public override void RefreshPopUp()
    {
        RefreshPopUp(GameSettingTitle.Length,
            () =>
            {
                for(int i = 0; i < GameSettingTitle.Length; i++)
                {
                    int index = i;
                    GameSettingSelectionButton selectButton = ActiveObjList[index].GetComponent<GameSettingSelectionButton>();

                    // ���ڿ� �ʱ�ȭ
                    selectButton.Setpanel(GameSettingTitle[i]);

                    if(index == GameSettingTitle.Length - 1)
                    {
                        // ���� selection���� ���ܵ�
                        selectButton.SetButtonCallback(
                            () =>
                            {
                                // ������ �ʱ�ȭ�� Ȯ�� �˾��� ����
                                popUPView.YesOrNoPopUpOpen();
                                popUPView.yesOrNoPopUp.UpdateMainDescription("����� ��� �����Ͱ� �����˴ϴ�.\n\n������ �ʱ�ȭ �Ͻðڽ��ϱ�?");
                                popUPView.yesOrNoPopUp.SetYesText("��");
                                popUPView.yesOrNoPopUp.SetYesButtonCallBack(
                                    () =>
                                    {
                                        PlayerPrefs.DeleteAll();
                                        Debug.Log("��� ����� ������ ����");
                                    });

                            });
                        
                        

                    }
                    else
                    {
                        // Ŭ���� ���� ȯ�漳�� â
                        selectButton.SetCallback(selectButton,
                            () =>
                            {
                                for (int CloseIndex = 0; CloseIndex < GameSettingPanel.Length; CloseIndex++)
                                {
                                    // ������ ���� ���ΰ�
                                    if (index == CloseIndex)
                                        continue;

                                    // �� �ܿ� ��� ���� �ݱ�
                                    else if (GameSettingPanel[CloseIndex].activeInHierarchy)
                                        GameSettingPanel[CloseIndex].SetActive(false);
                                }

                                GameSettingPanel[index].SetActive(true);

                            });
                    }
                }
            });
    }
}
