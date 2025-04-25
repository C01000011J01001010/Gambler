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
            // �̹� ����Ǵ� �����̸� �Լ� ����
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
            // �̹� ����Ǵ� �����̸� �Լ� ����
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
