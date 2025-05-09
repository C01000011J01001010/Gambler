using UnityEngine;

public abstract class AudioBase  : MonoBehaviour, IAudioDefault
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

    // 캐싱
    private AudioSource _audioSource;
    public AudioSource audioSource
    {
        get
        {
            CheckAudioSource();
            return _audioSource;
        }
    }
    private void CheckAudioSource()
    {
        if (_audioSource == null) 
            _audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        CheckAudioSource();
    }

    public virtual float UpdateAudioVolume(float value)
    {
        audioSource.volume = value;
        PlayerSaveManager.Instance.SaveData(volumeValueKey, value);
        Debug.Log("Master Volume Value == " + value.ToString());

        return value;
    }

    public virtual bool UpdateAudioMute(bool isMute)
    {
        audioSource.mute = isMute;
        PlayerPrefs.SetInt(volumeMuteKey, isMute ? 1 : 0);
        Debug.Log("Master Volume Muted == " + isMute.ToString());

        return isMute;
    }

    public virtual float LoadAudioVolume()
    {
        float value = PlayerSaveManager.Instance.LoadData(volumeValueKey, defaultVolume);
        audioSource.volume = value;

        return value;
    }

    public virtual bool LoadAudioMute()
    {
        bool isMute = PlayerSaveManager.Instance.LoadData(volumeMuteKey, 0) == 1 ? true : false;
        audioSource.mute = isMute;

        return isMute;
    }

    // 이 아래는 공통사항

    public virtual void LoadTotalSetting()
    {
        LoadAudioVolume();
        LoadAudioMute();
    }

    public virtual void ClearAudioSetting()
    {
        PlayerPrefs.DeleteKey(volumeValueKey);
        PlayerPrefs.DeleteKey(volumeMuteKey);
    }
}
