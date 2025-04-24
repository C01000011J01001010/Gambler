using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class GameSettingPopUp : PopUpBase_FullScreen<GameSettingPopUp>
{
    static readonly string[] GameSettingTitle =
    {
        "오디오 설정",
        //"조작 설정",
        "데이터 초기화"
    };

    // 데이터 초기화를 제외한 판넬
    // 데이터 초기화는 선택팝업창 사용
    public GameObject[] GameSettingPanel; 

    private PopUPView_Lobby popUPView { get { return GameManager.connector_Lobby.popUpView_Script; } }

    private void OnEnable()
    {
        RefreshPopUp();
        ScrollToTop();

        // 시작시 오디오 설정탭만 켜기
        {
            int index = 0;
            for (int CloseIndex = 0; CloseIndex < GameSettingPanel.Length; CloseIndex++)
            {
                // 열려는 탭은 놔두고
                if (index == CloseIndex)
                    continue;

                // 그 외에 모든 탭은 닫기
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

                    // 문자열 초기화
                    selectButton.Setpanel(GameSettingTitle[i]);

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
                                        PlayerPrefs.DeleteAll();
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
                                for (int CloseIndex = 0; CloseIndex < GameSettingPanel.Length; CloseIndex++)
                                {
                                    // 열려는 탭은 놔두고
                                    if (index == CloseIndex)
                                        continue;

                                    // 그 외에 모든 탭은 닫기
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
