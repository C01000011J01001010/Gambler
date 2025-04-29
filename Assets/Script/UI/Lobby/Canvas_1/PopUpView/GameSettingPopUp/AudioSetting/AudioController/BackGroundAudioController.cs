using UnityEngine;

public class BackGroundAudioController : AudioContollerBase
{
    public BackGroundAudio backGroundAudio { get { return audioManager.backGroundAudio; } }

    public override void LoadSavedData()
    {
        float volume = backGroundAudio.LoadAudioVolume();
        volumeSlider.slider.value = volume;

        bool isMute = backGroundAudio.LoadAudioMute();
        muteToggle.toggle.isOn = isMute;
    }

    public override void OnVolumeChanged(float value)
    {
        // 0~1 이내의 값으로 스케일링
        value /= volumeSlider.slider.maxValue;

        backGroundAudio.UpdateAudioVolume(value);
        Debug.Log("Master Volume Value == " + value.ToString());
    }

    // 콜백 함수
    public override void OnMuteToggle(bool isMuted)
    {
        // 뮤트가 false면 상호작용 가능
        volumeSlider.slider.interactable = !isMuted;

        backGroundAudio.UpdateAudioMute(isMuted);
        Debug.Log("Master Volume Muted == " + isMuted.ToString());
    }
}
