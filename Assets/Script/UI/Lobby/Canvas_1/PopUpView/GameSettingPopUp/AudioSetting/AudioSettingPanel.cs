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
        // 키에 저장된 모든 값을 제거
        AudioManager.Instance.ClearTotalAudioSettingValue();

        // 저장된 값이 없으니 기본값으로 초기화됨
        RefreshPanel();
    }
}
