using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class OptionPopUp : PopUpBase_Window<OptionPopUp>
{
    static readonly string[] Options =
    {
        "�κ�� �̵�",
        "ȯ�� ����"
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

                    // ��ư �ؽ�Ʈ ����
                    optionButton.buttonText.text = Options[index];

                    // �� ��ư�� �ʿ��� �ݹ� ����
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
