using System;
using UnityEngine;

public class GameSettingPopUp : PopUpBase_FullScreen<GameSettingPopUp>
{
    static readonly string[] GameSettingTitle =
    {
        "오디오 설정",
        //"조작 설정",
        "데이터 초기화"
    };

    public enum eSettingPanel
    {
        Audio,
        Control,
    }

    // 데이터 초기화를 제외한 판넬
    // 데이터 초기화는 선택팝업창 사용
    [SerializeField] private PanelBase_ConstContent[] _gameSettingPanel;
    public PanelBase_ConstContent[] gameSettingPanel { get { return _gameSettingPanel; } }

    public PanelBase_ConstContent currentPanel {  get; private set; }

    private PopUpViewBase popUPView { get { return GameManager.connector.popUpView; } }

    private void OnEnable()
    {
        RefreshPopUp();
        ScrollToTop();

        // 시작시 오디오 설정탭만 켜기
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

                    // 문자열 초기화
                    selectButton.Setpanel_Text(GameSettingTitle[i]);

                    if(index == GameSettingTitle.Length - 1)
                    {
                        // 단일 selection에서 제외됨
                        selectButton.SetButtonCallback(
                            () =>
                            {
                                // 데이터 초기화는 확인 팝업만 열림
                                popUPView.YesOrNoPopUpOpen();
                                popUPView.yesOrNoPopUp.UpdateMainDescription("저장된 모든 데이터가 삭제됩니다.\n\n정말로 초기화 하시겠습니까?");
                                popUPView.yesOrNoPopUp.SetYesText("예");
                                popUPView.yesOrNoPopUp.SetYesButtonCallBack(
                                    () =>
                                    {
                                        PlayerSaveManager.Instance.PlayerDataReset();
                                        Debug.Log("모든 저장된 데이터 삭제");
                                    });

                            });
                        
                        

                    }
                    else
                    {
                        // 클릭시 열릴 환경설정 창
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
    /// 하나의 판넬만 활성화 하고 나머지 전부 비활성화, 현재 판넬을 저장
    /// </summary>
    /// <param name="index"></param>
    public void PanelOpen(int index)
    {
        if (index >= gameSettingPanel.Length)
        {
            Debug.Log("잘못된 인덱스 접근");
            return;
        }

        for (int CloseIndex = 0; CloseIndex < gameSettingPanel.Length; CloseIndex++)
        {
            // 열려는 탭은 놔두고
            if (index == CloseIndex)
                continue;

            // 그 외에 모든 탭은 닫기
            else gameSettingPanel[CloseIndex].gameObject.SetActive(false);
                
        }

        // 현재 오픈한 판넬을 저장하고 그 판넬을 활성화
        currentPanel = gameSettingPanel[index];
        currentPanel.gameObject.SetActive(true);
    }
}
