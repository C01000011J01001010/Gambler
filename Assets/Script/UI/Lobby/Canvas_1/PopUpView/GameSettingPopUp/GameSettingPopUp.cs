using System;
using UnityEngine;

public class GameSettingPopUp : PopUpBase_FullScreen<GameSettingPopUp>
{
    static readonly string[] GameSettingTitle =
    {
        "����� ����",
        //"���� ����",
        "������ �ʱ�ȭ"
    };

    public enum eSettingPanel
    {
        Audio,
        Control,
    }

    // ������ �ʱ�ȭ�� ������ �ǳ�
    // ������ �ʱ�ȭ�� �����˾�â ���
    [SerializeField] private PanelBase_ConstContent[] _gameSettingPanel;
    public PanelBase_ConstContent[] gameSettingPanel { get { return _gameSettingPanel; } }

    public PanelBase_ConstContent currentPanel {  get; private set; }

    private PopUpViewBase popUPView { get { return GameManager.connector.popUpView; } }

    private void OnEnable()
    {
        RefreshPopUp();
        ScrollToTop();

        // ���۽� ����� �����Ǹ� �ѱ�
        PanelOpen((int)eSettingPanel.Audio);
    }

    public override void RefreshPopUp()
    {
        base.RefreshPopUp(GameSettingTitle.Length,
            (Action)(() =>
            {
                
                for (int i = 0; i < GameSettingTitle.Length; i++)
                {
                    int index = i;
                    GameSettingSelectionButton selectButton = ActiveObjList[index].GetComponent<GameSettingSelectionButton>();

                    // ���ڿ� �ʱ�ȭ
                    selectButton.Setpanel_Text(GameSettingTitle[i]);

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
                                        PlayerSaveManager.Instance.PlayerDataReset();
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
                                PanelOpen(index);
                            });
                    }
                }
            }));
    }


    /// <summary>
    /// �ϳ��� �ǳڸ� Ȱ��ȭ �ϰ� ������ ���� ��Ȱ��ȭ, ���� �ǳ��� ����
    /// </summary>
    /// <param name="index"></param>
    public void PanelOpen(int index)
    {
        if (index >= gameSettingPanel.Length)
        {
            Debug.Log("�߸��� �ε��� ����");
            return;
        }

        for (int CloseIndex = 0; CloseIndex < gameSettingPanel.Length; CloseIndex++)
        {
            // ������ ���� ���ΰ�
            if (index == CloseIndex)
                continue;

            // �� �ܿ� ��� ���� �ݱ�
            else gameSettingPanel[CloseIndex].gameObject.SetActive(false);
                
        }

        // ���� ������ �ǳ��� �����ϰ� �� �ǳ��� Ȱ��ȭ
        currentPanel = gameSettingPanel[index];
        currentPanel.gameObject.SetActive(true);
    }
}
