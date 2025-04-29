using System.Collections.Generic;
using UnityEngine;


public interface IAudioDefault
{
    string volumeValueKey { get; }
    string volumeMuteKey { get; }

    public float UpdateAudioVolume(float value);
    public bool UpdateAudioMute(bool isMute);
    public float LoadAudioVolume();
    public bool LoadAudioMute();
    public void LoadTotalSetting();
    public void ClearAudioSetting();
}

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private MasterAudio _masterAudio;
    [SerializeField] private BackGroundAudio _backGroundAudio;
    [SerializeField] private EffactAudio _effactAudio;


    public MasterAudio masterAudio{ get { return _masterAudio; } }
    public BackGroundAudio backGroundAudio { get { return _backGroundAudio; } }
    public EffactAudio effactAudio { get { return _effactAudio; } }
    public List<IAudioDefault> TotalAudio { get; private set; } // ������ �������̽��� ������� Ŭ������ ���� ����



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


    protected override void Awake()
    {
        base.Awake();
        InitAudioSet();
        previousMuteKey = "previousMuteKey";
    }

    private void InitAudioSet()
    {
        TotalAudio = new List<IAudioDefault>(3);
        TotalAudio.Add(masterAudio);
        TotalAudio.Add(backGroundAudio);
        TotalAudio.Add(effactAudio);
    }

    private void Start()
    {
        for (int i = 0; i < TotalAudio.Count; i++)
        {
            TotalAudio[i].LoadTotalSetting();
        }

        backGroundAudio.DefaultPlay();
    }

    /// <summary>
    /// ����� Ű�� ����� ������ ����
    /// </summary>
    public void ClearTotalAudioSetting()
    {
        for (int i = 0; i < TotalAudio.Count; i++)
        {
            TotalAudio[i].ClearAudioSetting();
        }
    }


    public void SavePreviousStateAndSyncCurrent_Mute(AudioContollerBase[] ControllerArray, bool currentMute)
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
}
