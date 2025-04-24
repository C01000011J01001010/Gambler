using UnityEngine;

public class MasterVolumeController : AudioVolumeContollerBase
{
    /// <summary>
    /// 마스터키를 제외한 모든 컨트롤러
    /// </summary>
    public AudioVolumeContollerBase[] totalController;


    private bool prevMute;
    
    public override void LoadSavedData()
    {
        float volume = PlayerSaveManager.Instance.LoadData(audioManager.masterVolumeValueKey, 1f);
        volumeSlider.slider.value = volume;

        bool isMute = PlayerSaveManager.Instance.LoadData(audioManager.masterVolumeMuteKey, 0) == 1 ? true : false;
        muteToggle.toggle.isOn = isMute;
    }

    public override void OnVolumeChanged(float value)
    {
        // 0~1 이내의 값으로 스케일링
        value /= volumeSlider.slider.maxValue;

        AudioListener.volume = value;
        PlayerPrefs.SetFloat(audioManager.masterVolumeValueKey, value);
        Debug.Log("Master Volume Value == " + value.ToString());
    }

    // 콜백 함수
    public override void OnMuteToggle(bool isMuted)
    {
        // 뮤트가 false면 상호작용 가능
        volumeSlider.slider.interactable = !isMuted;

        // 모든 컨트롤러 동기화
        AudioManager.Instance.SavePreviousStateAndSyncCurrent_Mute(totalController, isMuted);

        AudioListener.pause = isMuted;
        PlayerPrefs.SetInt(audioManager.masterVolumeMuteKey, isMuted ? 1 : 0);
        Debug.Log("Master Volume Muted == " + isMuted.ToString());

        // 이전 상태 초기화
        prevMute = isMuted;
    }

    
}
