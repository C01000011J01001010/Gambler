using UnityEngine;
using UnityEngine.Audio;

public class BackGroundAudio : GameAudio
{
    [SerializeField] private AudioClip defaultMusic;
    [SerializeField] private AudioClip playMusic;

    protected void Awake()
    {
        volumeValueKey = $"AudioKey_BackGroundAudio";
        volumeMuteKey = $"{volumeValueKey}_Mute";
        defaultVolume = 0.8f;
    }


    public void DefaultPlay()
    {
        if (audioSource != null)
        {
            // 이미 재생되는 음악이면 함수 종료
            if (audioSource.clip == defaultMusic) return;

            audioSource.Stop();
            audioSource.loop = true;
            audioSource.clip = defaultMusic;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("audioSource == null");
        }
    }

    public void LetsPlay()
    {

        if (audioSource != null)
        {
            // 이미 재생되는 음악이면 함수 종료
            if (audioSource.clip == playMusic) return;

            audioSource.Stop();
            audioSource.loop = true;
            audioSource.clip = playMusic;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("audioSource == null");
        }
    }


}
