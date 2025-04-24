using UnityEngine;

public class BackGroundVolumeController : AudioVolumeContollerBase
{
    public BackGroundAudio backGroundAudio { get { return audioManager.backGroundAudio; } }

    public override void LoadSavedData()
    {
        float volume = backGroundAudio.LoadVolumeValue();
        volumeSlider.slider.value = volume;

        bool isMute = backGroundAudio.LoadVolumeMute();
        muteToggle.toggle.isOn = isMute;
    }

    public override void OnVolumeChanged(float value)
    {
        // 0~1 �̳��� ������ �����ϸ�
        value /= volumeSlider.slider.maxValue;

        backGroundAudio.UpdateVolumeValue(value);
        Debug.Log("Master Volume Value == " + value.ToString());
    }

    // �ݹ� �Լ�
    public override void OnMuteToggle(bool isMuted)
    {
        // ��Ʈ�� false�� ��ȣ�ۿ� ����
        volumeSlider.slider.interactable = !isMuted;

        backGroundAudio.UpdateVolumeMute(isMuted);
        Debug.Log("Master Volume Muted == " + isMuted.ToString());
    }
}
