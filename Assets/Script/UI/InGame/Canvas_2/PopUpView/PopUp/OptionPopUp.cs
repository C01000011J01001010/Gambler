using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class OptionPopUp : PopUpBase_Window<OptionPopUp>
{
    static readonly string[] Options =
    {
        "로비로 이동",
        "환경 설정"
    };

    protected override void OnEnable()
    {
        base.OnEnable();
        RefreshPopUp();
        ScrollToTop();
    }

    public override void RefreshPopUp()
    {
        RefreshPopUp(Options.Length,
            () =>
            {
                for (int index = 0; index < Options.Length; index++)
                {
                    OptionButton optionButton = ActiveObjList[index].GetComponent<OptionButton>();

                    // 버튼 텍스트 지정
                    optionButton.buttonText.text = Options[index];

                    // 각 버튼에 필요한 콜백 삽입
                    switch (index)
                    {
                        case 0:
                            {
                                optionButton.SetButtonCallback(
                                    () => 
                                    {
                                        GameManager.Instance.SceneUnloadView(() => SceneManager.LoadScene("Lobby"));
                                    });
                            }
                            break;

                        case 1:
                            {
                                optionButton.SetButtonCallback(
                                    () =>
                                    {
                                        GameManager.connector_InGame.popUpViewAsInGame.GameSettingPopUpOpen();
                                    });
                            }
                            break;
                    }
                }

            });

    }

}
