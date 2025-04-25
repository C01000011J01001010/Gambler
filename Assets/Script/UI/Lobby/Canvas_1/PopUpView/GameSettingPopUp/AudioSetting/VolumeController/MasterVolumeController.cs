using UnityEngine;

public class MasterVolumeController : AudioVolumeContollerBase
{
    /// <summary>
    /// ������Ű�� ������ ��� ��Ʈ�ѷ�
    /// </summary>
    public AudioVolumeContollerBase[] totalController;


    
    public override void LoadSavedData()
    {
        float volume = audioManager.LoadMasterVolumeValue();
        volumeSlider.slider.value = volume;

        bool isMute = audioManager.LoadMasterVolumeMute();
        muteToggle.toggle.isOn = isMute;
    }

    public override void OnVolumeChanged(float value)
    {
        // 0~1 �̳��� ������ �����ϸ�
        value /= volumeSlider.slider.maxValue;

        audioManager.UpdateMasterVolumeValue(value);

        //AudioListener.volume = value;
        //PlayerPrefs.SetFloat(audioManager.masterVolumeValueKey, value);
        //Debug.Log("Master Volume Value == " + value.ToString());
    }

    // �ݹ� �Լ�
    public override void OnMuteToggle(bool isMuted)
    {
        // ��Ʈ�� false�� ��ȣ�ۿ� ����
        volumeSlider.slider.interactable = !isMuted;

        // ��� ��Ʈ�ѷ� ����ȭ
        audioManager.SavePreviousStateAndSyncCurrent_Mute(totalController, isMuted);

        // ��Ʈ ������Ʈ
        audioManager.UpdateMasterVolumeMute(isMuted);
    }

    
}
