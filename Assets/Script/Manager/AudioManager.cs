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
    /// �����ͺ��� ��Ʈ�� �ٸ� ��Ʈ�ѷ��� ���� ���� ����
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
                    _previousMuteStatusArr[i] = false; // �⺻���� ������� ���� ����
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
            // ���Ѱ��� 1�� ���� (�ִ� ������ 1)
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
        // ��� ��Ʈ�ѷ� ����ȭ
        for (int i = 0; i < ControllerArray.Length; i++)
        {
            if (currentMute)
            {
                // ���� ���� ����
                previousMuteStatusArr[i] = ControllerArray[i].muteToggle.toggle.isOn;

                // ���� �����
                // ���̱⿡�� �ٲ�� ���� ���� �Լ��� ���
                ControllerArray[i].muteToggle.toggle.isOn =  currentMute;

                // �����͹�Ʈ�� ��۵Ǹ� �ٸ� ��Ʈ�� ����� �Ұ����ϵ��� ����
                ControllerArray[i].muteToggle.toggle.interactable = !currentMute;
                ControllerArray[i].volumeSlider.slider.interactable = !currentMute;

            }
            else
            {
                // ��� �����ϵ��� ����
                ControllerArray[i].muteToggle.toggle.interactable = !currentMute;
                ControllerArray[i].volumeSlider.slider.interactable = !currentMute;

                // ���� ���� ����
                ControllerArray[i].muteToggle.toggle.isOn = previousMuteStatusArr[i];
            }
        }
    }

    /// <summary>
    /// ����� Ű�� ����� ������ ����
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
