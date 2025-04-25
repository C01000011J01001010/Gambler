using UnityEngine;
using UnityEngine.UI;

public abstract class AudioVolumeContollerBase : MonoBehaviour
{
    [SerializeField] protected SliderBase _volumeSlider;
    [SerializeField] protected ToggleBase _muteToggle;

    public SliderBase volumeSlider { get { return _volumeSlider; } }
    public ToggleBase muteToggle { get { return _muteToggle; } }

    public AudioManager audioManager
    {
        get { return AudioManager.Instance; }
    }

    public virtual void InitController()
    {
        volumeSlider.SetSliderCallback(OnVolumeChanged);
        muteToggle.SetToggleCallback(OnMuteToggle);
        LoadSavedData();
    }

    public abstract void LoadSavedData();

    public abstract void OnVolumeChanged(float volume);

    public abstract void OnMuteToggle(bool isMuted);
}
