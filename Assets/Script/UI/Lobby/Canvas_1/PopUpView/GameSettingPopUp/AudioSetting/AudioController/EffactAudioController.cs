using UnityEngine;

public class EffactAudioController : AudioContollerBase
{
    public EffactAudio effactAudio { get { return audioManager.effactAudio; } }

    public override void LoadSavedData()
    {
        float volume = effactAudio.LoadAudioVolume();
        volumeSlider.slider.value = volume;

        bool isMute = effactAudio.LoadAudioMute();
        muteToggle.toggle.isOn = isMute;
    }

    public override void OnVolumeChanged(float value)
    {
        // 0~1 �̳��� ������ �����ϸ�
        value /= volumeSlider.slider.maxValue;

        effactAudio.UpdateAudioVolume(value);
        Debug.Log("Master Volume Value == " + value.ToString());
    }

    // �ݹ� �Լ�
    public override void OnMuteToggle(bool isMuted)
    {
        // ��Ʈ�� false�� ��ȣ�ۿ� ����
        volumeSlider.slider.interactable = !isMuted;

        effactAudio.UpdateAudioMute(isMuted);
        Debug.Log("Master Volume Muted == " + isMuted.ToString());
    }
}
