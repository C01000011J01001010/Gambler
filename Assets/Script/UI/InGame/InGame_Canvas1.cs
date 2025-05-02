using UnityEngine;

public class InGame_Canvas1 : MonoBehaviour
{
    [SerializeField] private TextWindowView _TextWindowView;
    [SerializeField] private PlayerMoneyView _PlayerMoneyView;
    [SerializeField] private PauseButton _PauseButton;
    [SerializeField] private IconView _IconView;
    [SerializeField] private PopUpView_InGame _PopUpView;

    public TextWindowView TextWindowView => _TextWindowView;
    public PlayerMoneyView PlayerMoneyView => _PlayerMoneyView;
    public PauseButton PauseButton => _PauseButton;
    public IconView IconView => _IconView;
    public PopUpView_InGame PopUpView => _PopUpView;
}
