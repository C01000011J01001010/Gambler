using UnityEngine;

public class AudioSettingPanel : PanelBase_ConstContent
{
    [SerializeField] AudioVolumeContollerBase[] audioControllerArr;

    

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
        AudioManager.Instance.ClearTotalAudioSettingValue();

        // ����� ���� ������ �⺻������ �ʱ�ȭ��
        RefreshPanel();
    }
}
