using DG.Tweening;
using UnityEngine;

public class MethodManager : MonoBehaviour
{
    public static bool IsIndexInRange<T>(T[] array, int index)
    {
        return index >= 0 && index < array.Length;
    }

    // ------------ui----------
    public static Canvas FindParentCanvas(Transform start)
    {
        Transform current = start;

        while (current != null)
        {
            Canvas canvas = current.GetComponent<Canvas>();
            if (canvas != null)
                return canvas;

            current = current.parent;
        }

        Debug.LogWarning("캔버스를 찾지 못함");
        return null; // 못 찾았을 경우
    }

    public static Sequence GetNewSequnce_SingleUse(GameObject targetObj)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetAutoKill(true); // 시퀀스가 끝나면 자동으로 삭제
        sequence.SetLink(targetObj); // 시퀀스가 적용되는 오브젝트가 삭제될 시 같이 소멸
        sequence.SetLoops(1);

        return sequence;
    }

    public static Sequence GetNewSequnce_Recyclable(GameObject useObj)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetAutoKill(false); // 시퀀스가 끝나도 유지됨
        sequence.SetLink(useObj); // 시퀀스가 적용되는 오브젝트가 삭제될 시 같이 소멸
        sequence.SetLoops(1);

        return sequence;
    }

    public static void GetSequence_FadeIn(Sequence sequence, GameObject targetObj, float delay)
    {
        sequence.AppendCallback(() => targetObj.SetActive(true));
        sequence.AppendCallback(() => targetObj.transform.localScale = Vector3.zero);
        sequence.Append(targetObj.transform.DOScale(Vector3.one, delay).SetEase(Ease.OutBack));
    }

    public static void PlaySequence_FadeIn(GameObject targetObj, float delay)
    {
        Sequence sequence = GetNewSequnce_SingleUse(targetObj);
        GetSequence_FadeIn(sequence, targetObj, delay);
        sequence.Play();
    }

    public static void GetSequence_FadeOut(Sequence sequence, GameObject targetObj, float delay)
    {
        sequence.AppendCallback(() => targetObj.transform.localScale = Vector3.one);
        sequence.Append(targetObj.transform.DOScale(Vector3.zero, delay).SetEase(Ease.InBack));
        sequence.AppendCallback(() => targetObj.SetActive(false));
    }
}
