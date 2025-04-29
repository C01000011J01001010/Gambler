using UnityEngine;

public class MasterAudio : MonoBehaviour, IAudioDefault // AudioSource를 사용하지 않으니 따로 만듬 
{
    public string volumeValueKey { get; protected set; }
    public string volumeMuteKey { get; protected set; }

    private float _defaultVolume;
    protected float defaultVolume
    {
        get { return _defaultVolume; }
        set
        {
            // 상한값을 1로 제한 (최대 볼륨은 1)
            _defaultVolume = Mathf.Clamp(value, 0f, 1f);
        }
    }

    private void Awake()
    {
        volumeValueKey = "AudioKey_Master";
        volumeMuteKey = $"{volumeValueKey}_Mute";
        defaultVolume = 0.7f;
    }

    public float UpdateAudioVolume(float value)
    {
        AudioListener.volume = value;
        PlayerSaveManager.Instance.SaveData(volumeValueKey, value);
        Debug.Log("Master Volume Value == " + value.ToString());

        return value;
    }

    public bool UpdateAudioMute(bool isMute)
    {
        AudioListener.pause = isMute;
        PlayerSaveManager.Instance.SaveData(volumeMuteKey, isMute ? 1 : 0);
        Debug.Log("Master Volume Muted == " + isMute.ToString());

        return isMute;
    }

    public float LoadAudioVolume()
    {
        float value = PlayerSaveManager.Instance.LoadData(volumeValueKey, defaultVolume);
        AudioListener.volume = value;

        return value;
    }

    public bool LoadAudioMute()
    {
        bool isMute = PlayerSaveManager.Instance.LoadData(volumeMuteKey, 0) == 1 ? true : false;
        AudioListener.pause = isMute;

        return isMute;
    }

    public void LoadTotalSetting()
    {
        LoadAudioVolume();
        LoadAudioMute();
    }

    public void ClearAudioSetting()
    {
        PlayerPrefs.DeleteKey(volumeValueKey);
        PlayerPrefs.DeleteKey(volumeMuteKey);
    }
}
