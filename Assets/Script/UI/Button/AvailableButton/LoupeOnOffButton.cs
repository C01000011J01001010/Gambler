using UnityEngine;
using UnityEngine.UI;

public class LoupeOnOffButton : ImageChange_ButtonBase
{
    public GameObject Loupe;


    private void Start()
    {
        LoupeOff();
    }

    public void LoupeOn()
    {
        if(Loupe == null)
        {
            Debug.LogAssertion("Loupe ���� �ȵ�");
            return;
        }

        // ����� �ؽ�Ʈ�� ����
        GameManager.connector_InGame.Canvas1.TextWindowView.StartTextWindow(PublicSet.eSystemGuide.HowToUseLoupe);

        // ��ư ��ȯ
        Loupe.SetActive(true);
        ChangeOn();
        SetButtonCallback(LoupeOff);
    }

    public void LoupeOff()
    {
        if (Loupe == null)
        {
            Debug.LogAssertion("Loupe ���� �ȵ�");
            return;
        }

        Loupe.SetActive(false);
        ChangeOff();
        SetButtonCallback(LoupeOn);
    }


}
