using UnityEngine;

public class MasterVolumeController : AudioVolumeContollerBase
{
    /// <summary>
    /// ������Ű�� ������ ��� ��Ʈ�ѷ�
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
        // 0~1 �̳��� ������ �����ϸ�
        value /= volumeSlider.slider.maxValue;

        AudioListener.volume = value;
        PlayerPrefs.SetFloat(audioManager.masterVolumeValueKey, value);
        Debug.Log("Master Volume Value == " + value.ToString());
    }

    // �ݹ� �Լ�
    public override void OnMuteToggle(bool isMuted)
    {
        // ��Ʈ�� false�� ��ȣ�ۿ� ����
        volumeSlider.slider.interactable = !isMuted;

        // ��� ��Ʈ�ѷ� ����ȭ
        AudioManager.Instance.SavePreviousStateAndSyncCurrent_Mute(totalController, isMuted);

        AudioListener.pause = isMuted;
        PlayerPrefs.SetInt(audioManager.masterVolumeMuteKey, isMuted ? 1 : 0);
        Debug.Log("Master Volume Muted == " + isMuted.ToString());

        // ���� ���� �ʱ�ȭ
        prevMute = isMuted;
    }

    
}
