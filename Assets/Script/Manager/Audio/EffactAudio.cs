using UnityEngine;

public class EffactAudio : GameAudio
{
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip effact;

    protected void Awake()
    {
        volumeValueKey = $"AudioKey_EffactAudio";
        volumeMuteKey = $"{volumeValueKey}_Mute";
        defaultVolume = 1.0f;
    }


    private void Update()
    {
#if UNITY_EDITOR
        // 마우스 클릭을 감지
        if (Input.GetMouseButtonDown(0)) // 0은 좌클릭
        {
            playClick();
        }
#endif
        // 모바일 터치도 감지
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            playClick();
        }
    }

    private void playClick()
    {
        audioSource.clip = click;
        audioSource.Play();
    }

}
