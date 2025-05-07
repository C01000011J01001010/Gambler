using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Deactivatable_ButtonBase : ButtonBase
{
    /// <summary>
    /// 버튼클릭 비활성화
    /// </summary>
    public virtual void TryDeactivate_Button()
    {
        if (!isInteractable) return;

        SetButtonInteractable(false);

        SetDisabledColorAlpha_1();
    }

    /// <summary>
    /// 버튼클릭 활성화
    /// </summary>
    public virtual void TryActivate_Button()
    {
        if (isInteractable) return;

        SetButtonInteractable(true);
    }
}
