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

        Debug.LogWarning("ĵ������ ã�� ����");
        return null; // �� ã���� ���
    }
}
