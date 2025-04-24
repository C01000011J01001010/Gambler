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
        // ��� ��Ʈ�ѷ� ����ȭ
        for (int i = 0; i < ControllerArray.Length; i++)
        {
            if (currentMute)
            {
                // ���� ���� ����
                previousMuteStatusArr[i] = ControllerArray[i].muteToggle.toggle.isOn;

                // ���� �����
                ControllerArray[i].muteToggle.toggle.isOn = currentMute;
            }
            else
            {
                // ���� ���� ����
                ControllerArray[i].muteToggle.toggle.isOn = previousMuteStatusArr[i];
            }

        }
    }


}
