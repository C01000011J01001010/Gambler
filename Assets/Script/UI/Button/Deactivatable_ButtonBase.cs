using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Deactivatable_ButtonBase : ButtonBase
{
    /// <summary>
    /// ��ưŬ�� ��Ȱ��ȭ
    /// </summary>
    public virtual void TryDeactivate_Button()
    {
        if (!isInteractable) return;

        SetButtonInteractable(false);

        SetDisabledColorAlpha_1();
    }

    /// <summary>
    /// ��ưŬ�� Ȱ��ȭ
    /// </summary>
    public virtual void TryActivate_Button()
    {
        if (isInteractable) return;

        SetButtonInteractable(true);
    }
}
