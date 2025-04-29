using UnityEngine;

public class AudioSettingPanel : PanelBase_ConstContent
{
    [SerializeField] AudioContollerBase[] audioControllerArr;

    

    private void Start()
    {
        RefreshPanel();
    }

    public override void RefreshPanel()
    {
        foreach (var controller in audioControllerArr)
        {
            controller.InitController();
        }
    }

    public void ResetAudioSetting()
    {
        // Ű�� ����� ��� ���� ����
        AudioManager.Instance.ClearTotalAudioSetting();

        // ����� ���� ������ �⺻������ �ʱ�ȭ��
        RefreshPanel();
    }
}
