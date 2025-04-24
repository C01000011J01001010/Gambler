using UnityEngine;

public abstract class GameAudio  : MonoBehaviour
{
    public string volumeValueKey { get; protected set; }
    public string volumeMuteKey { get; protected set; }

    private AudioSource _audioSource;
    private float _defaultVolume;
    public AudioSource audioSource
    {
        get
        {
            if(_audioSource == null) _audioSource = GetComponent<AudioSource>();
            return _audioSource;
        }
    }
    protected float defaultVolume
    {
        get { return _defaultVolume; }
        set
        {
            // 상한값을 1로 제한 (최대 볼륨은 1)
            _defaultVolume = Mathf.Clamp(value, 0f, 1f);
        }
    }



    public float UpdateVolumeValue(float value)
    {
        audioSource.volume = value;
        PlayerSaveManager.Instance.SaveData(volumeValueKey, value);
        Debug.Log("Master Volume Value == " + value.ToString());

        return value;
    }

    public virtual bool UpdateVolumeMute(bool isMute)
    {
        audioSource.mute = isMute;
        PlayerPrefs.SetInt(volumeMuteKey, isMute ? 1 : 0);
        Debug.Log("Master Volume Muted == " + isMute.ToString());

        return isMute;
    }

    public virtual float LoadVolumeValue()
    {
        float value = PlayerSaveManager.Instance.LoadData(volumeValueKey, defaultVolume);
        audioSource.volume = value;

        return value;
    }

    public virtual bool LoadVolumeMute()
    {
        bool isMute = PlayerSaveManager.Instance.LoadData(volumeMuteKey, 0) == 1 ? true : false;
        audioSource.mute = isMute;

        return isMute;
    }

    public virtual void LoadTotalData()
    {
        LoadVolumeValue();
        LoadVolumeMute();
    }
}
