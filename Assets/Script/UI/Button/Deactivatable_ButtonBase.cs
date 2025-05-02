using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Deactivatable_ButtonBase : ButtonBase
{

    public bool isInteractable => button.interactable;

    /// <summary>
    /// ��ưŬ�� ��Ȱ��ȭ
    /// </summary>
    public virtual void TryDeactivate_Button()
    {
        if (!isInteractable) return;

        button.interactable = false;

        if (button.colors.disabledColor.a > 0.99f) return;

        else
        {
            // ��ȣ�ۿ��� ���� ���� �� �⺻���� ����Ǵ� ������ ����
            ColorBlock colorBlock = button.colors;
            Color color = colorBlock.disabledColor;

            color.a = 1.0f;

            colorBlock.disabledColor = color;
            button.colors = colorBlock;
        }
    }

    /// <summary>
    /// ��ưŬ�� Ȱ��ȭ
    /// </summary>
    public virtual void TryActivate_Button()
    {
        if (isInteractable) return;

        button.interactable = true;
    }
}
