using UnityEngine;

public abstract class IconButtonBase : ButtonBase
{
    private PopUpView_InGame _popUpView;
    public PopUpView_InGame popUpView
    {
        get
        {
            if (_popUpView == null)
            {
                _popUpView = GameManager.connector_InGame.popUpView_Script;
            }
            return _popUpView;
        }
    }
}
