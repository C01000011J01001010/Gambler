using UnityEngine;

public class EffactVolumeController : AudioVolumeContollerBase
{
    public EffactAudio effactAudio { get { return audioManager.effactAudio; } }

    public override void LoadSavedData()
    {
        float volume = effactAudio.LoadVolumeValue();
        volumeSlider.slider.value = volume;

        bool isMute = effactAudio.LoadVolumeMute();
        muteToggle.toggle.isOn = isMute;
    }

    public override void OnVolumeChanged(float value)
    {
        // 0~1 이내의 값으로 스케일링
        value /= volumeSlider.slider.maxValue;

        effactAudio.UpdateVolumeValue(value);
        Debug.Log("Master Volume Value == " + value.ToString());
    }

    // 콜백 함수
    public override void OnMuteToggle(bool isMuted)
    {
        // 뮤트가 false면 상호작용 가능
        volumeSlider.slider.interactable = !isMuted;

        effactAudio.UpdateVolumeMute(isMuted);
        Debug.Log("Master Volume Muted == " + isMuted.ToString());
    }
}
