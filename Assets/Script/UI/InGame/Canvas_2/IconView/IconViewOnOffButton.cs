using UnityEngine;
using UnityEngine.UI;

public class IconViewOnOffButton : IconButtonBase
{
    private IconView _iconview;
    public IconView iconView
    {
        get
        {
            if (_iconview == null) _iconview = GameManager.connector_InGame.iconView_Script;
            return _iconview;
        }
    }
    public Sprite leftArrow;
    public Sprite rightArrow;
    public Image image;


    private void Start()
    {
        SetButtonCallback(iconView.IconViewOpen);
    }

    public void SetImage_ToOpen()
    {
        image.sprite = leftArrow;
    }

    public void SetImage_ToClose()
    {
        image.sprite = rightArrow;
    }

}
