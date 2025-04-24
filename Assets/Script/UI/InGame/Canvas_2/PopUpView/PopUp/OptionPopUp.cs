using UnityEngine;

public class OptionPopUp : PopUpBase_Window<OptionPopUp>
{
    static readonly string[] Options =
    {
        "로비로 이동",
        "환경 설정"
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
