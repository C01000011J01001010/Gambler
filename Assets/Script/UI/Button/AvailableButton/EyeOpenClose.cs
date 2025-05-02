using UnityEngine;

public class EyeOpenClose : ImageChange_ButtonBase
{
    public Sprite eyeOpen;
    public Sprite eyeClose;

    private TargetDisplay targetImageDisplay { get { return CardGamePlayManager.Instance.cardGameView.targetDisplay; } }

    private void Start()
    {
        EyeClose();
    }

    public void EyeOpen()
    {
        targetImageDisplay.gameObject.SetActive(true);
        ChangeOn();
        SetButtonCallback(EyeClose);
    }

    public void EyeClose()
    {
        targetImageDisplay.gameObject.SetActive(false);
        ChangeOff();
        SetButtonCallback(EyeOpen);
    }


    protected override void ChangeOn()
    {
        if (image != null)
        {
            image.sprite = eyeOpen;
        }
        else
        {
            Debug.Log($"{gameObject.name} ��ü�� �̹��� ������Ʈ ����");
        }
    }
    protected override void ChangeOff()
    {
        if (image != null)
        {
            image.sprite = eyeClose;
        }
        else
        {
            Debug.Log($"{gameObject.name} ��ü�� �̹��� ������Ʈ ����");
        }
    }
}
