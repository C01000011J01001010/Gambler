using UnityEngine;

public class MasterAudioController : AudioContollerBase
{
    /// <summary>
    /// 마스터키를 제외한 모든 컨트롤러
    /// </summary>
    public AudioContollerBase[] totalController;


    
    public override void LoadSavedData()
    {
        float volume = audioManager.masterAudio.LoadAudioVolume();
        volumeSlider.slider.value = volume;

        bool isMute = audioManager.masterAudio.LoadAudioMute();
        muteToggle.toggle.isOn = isMute;
    }

    public override void OnVolumeChanged(float value)
    {
        // 0~1 이내의 값으로 스케일링
        value /= volumeSlider.slider.maxValue;

        audioManager.masterAudio.UpdateAudioVolume(value);

        //AudioListener.volume = value;
        //PlayerPrefs.SetFloat(audioManager.masterVolumeValueKey, value);
        //Debug.Log("Master Volume Value == " + value.ToString());
    }

    // 콜백 함수
    public override void OnMuteToggle(bool isMuted)
    {
        // 뮤트가 false면 상호작용 가능
        volumeSlider.slider.interactable = !isMuted;

        // 모든 컨트롤러 동기화
        audioManager.SavePreviousStateAndSyncCurrent_Mute(totalController, isMuted);

        // 뮤트 업데이트
        audioManager.masterAudio.UpdateAudioMute(isMuted);
    }

    
}
