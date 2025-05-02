using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Deactivatable_ButtonBase : ButtonBase
{

    public bool isInteractable => button.interactable;

    /// <summary>
    /// 버튼클릭 비활성화
    /// </summary>
    public virtual void TryDeactivate_Button()
    {
        if (!isInteractable) return;

        button.interactable = false;

        if (button.colors.disabledColor.a > 0.99f) return;

        else
        {
            // 상호작용을 하지 않을 시 기본으로 적용되는 반투명 제거
            ColorBlock colorBlock = button.colors;
            Color color = colorBlock.disabledColor;

            color.a = 1.0f;

            colorBlock.disabledColor = color;
            button.colors = colorBlock;
        }
    }

    /// <summary>
    /// 버튼클릭 활성화
    /// </summary>
    public virtual void TryActivate_Button()
    {
        if (isInteractable) return;

        button.interactable = true;
    }
}
