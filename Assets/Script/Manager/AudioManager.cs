using Unity.VisualScripting;
using UnityEngine;

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


    protected override void Awake()
    {
        base.Awake();
        masterVolumeValueKey = "AudioKey_Master";
        masterVolumeMuteKey = $"{masterVolumeValueKey}_Mute";
        previousMuteKey = "previousMuteKey";
    }

    private void Start()
    {
        backGroundAudio.LoadTotalData();
        effactAudio.LoadTotalData();

        backGroundAudio.DefaultPlay();
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
                ControllerArray[i].muteToggle.toggle.isOn = currentMute;
            }
            else
            {
                // 이전 상태 복귀
                ControllerArray[i].muteToggle.toggle.isOn = previousMuteStatusArr[i];
            }

        }
    }


}
