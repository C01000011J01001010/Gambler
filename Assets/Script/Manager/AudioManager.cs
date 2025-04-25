using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private BackGroundAudio _backGroundAudio;
    [SerializeField] private EffactAudio _effactAudio;


    public BackGroundAudio backGroundAudio { get { return _backGroundAudio; } }
    public EffactAudio effactAudio { get { return _effactAudio; } }

    public string masterVolumeValueKey { get; private set; }
    public string masterVolumeMuteKey { get; private set; }
    public string previousMuteKey { get; private set; }

    /// <summary>
    /// 마스터볼륨 뮤트시 다른 컨트롤러의 이전 상태 저장
    /// </summary>
    private bool[] _previousMuteStatusArr;
    public bool[] previousMuteStatusArr
    {
        get
        {
            if (_previousMuteStatusArr == null)
            {
                _previousMuteStatusArr = new bool[2];
                for (int i = 0; i < _previousMuteStatusArr.Length; i++)
                {
                    _previousMuteStatusArr[i] = false; // 기본값은 토글하지 않은 상태
                }
            }
            return _previousMuteStatusArr;
        }
    }

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


    protected override void Awake()
    {
        base.Awake();
        masterVolumeValueKey = "AudioKey_Master";
        masterVolumeMuteKey = $"{masterVolumeValueKey}_Mute";
        previousMuteKey = "previousMuteKey";

        defaultVolume = 0.7f;
    }

    private void Start()
    {
        LoadTotalData();
        backGroundAudio.LoadTotalData();
        effactAudio.LoadTotalData();

        backGroundAudio.DefaultPlay();
    }

    public float UpdateMasterVolumeValue(float value)
    {
        AudioListener.volume = value;
        PlayerSaveManager.Instance.SaveData(masterVolumeValueKey, value);
        Debug.Log("Master Volume Value == " + value.ToString());

        return value;
    }

    public virtual bool UpdateMasterVolumeMute(bool isMute)
    {
        AudioListener.pause = isMute;
        PlayerSaveManager.Instance.SaveData(masterVolumeMuteKey, isMute ? 1 : 0);
        Debug.Log("Master Volume Muted == " + isMute.ToString());

        return isMute;
    }

    public virtual float LoadMasterVolumeValue()
    {
        float value = PlayerSaveManager.Instance.LoadData(masterVolumeValueKey, defaultVolume);
        AudioListener.volume = value;

        return value;
    }

    public virtual bool LoadMasterVolumeMute()
    {
        bool isMute = PlayerSaveManager.Instance.LoadData(masterVolumeMuteKey, 0) == 1 ? true : false;
        AudioListener.pause = isMute;

        return isMute;
    }

    public virtual void LoadTotalData()
    {
        LoadMasterVolumeValue();
        LoadMasterVolumeMute();
    }

    public void SavePreviousStateAndSyncCurrent_Mute(AudioVolumeContollerBase[] ControllerArray, bool currentMute)
    {
        // 모든 컨트롤러 동기화
        for (int i = 0; i < ControllerArray.Length; i++)
        {
            if (currentMute)
            {
                // 이전 상태 저장
                previousMuteStatusArr[i] = ControllerArray[i].muteToggle.toggle.isOn;

                // 덮어 씌우기
                // 보이기에만 바뀌기 위해 지정 함수를 사용
                ControllerArray[i].muteToggle.toggle.isOn =  currentMute;

                // 마스터뮤트가 토글되면 다른 뮤트는 토글이 불가능하도록 만듬
                ControllerArray[i].muteToggle.toggle.interactable = !currentMute;
                ControllerArray[i].volumeSlider.slider.interactable = !currentMute;

            }
            else
            {
                // 토글 가능하도록 만듬
                ControllerArray[i].muteToggle.toggle.interactable = !currentMute;
                ControllerArray[i].volumeSlider.slider.interactable = !currentMute;

                // 이전 상태 복귀
                ControllerArray[i].muteToggle.toggle.isOn = previousMuteStatusArr[i];
            }
        }
    }

    /// <summary>
    /// 오디오 키에 저장된 값만을 제거
    /// </summary>
    public void ClearTotalAudioSettingValue()
    {
        {
            PlayerPrefs.DeleteKey(masterVolumeValueKey);
            PlayerPrefs.DeleteKey(masterVolumeMuteKey);
            backGroundAudio.ClearAudioSettingValue();
            effactAudio.ClearAudioSettingValue();
        }
    }
}
