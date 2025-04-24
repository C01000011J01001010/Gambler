using UnityEngine;

public class OptionPopUp : PopUpBase_Window<OptionPopUp>
{
    static readonly string[] Options =
    {
        "�κ�� �̵�",
        "ȯ�� ����"
    };

    protected override void OnEnable()
    {
        base.OnEnable();
        RefreshPopUp();
        ScrollToTop();
    }

    public override void RefreshPopUp()
    {
        RefreshPopUp(Options.Length,
            () =>
            {

            });
    }
}
