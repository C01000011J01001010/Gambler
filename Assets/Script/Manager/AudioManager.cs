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
    public List<IAudioDefault> TotalAudio { get; private set; } // 동일한 인터페이스를 사용중인 클래스를 묶을 예정



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
    /// 오디오 키에 저장된 값만을 제거
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
}
