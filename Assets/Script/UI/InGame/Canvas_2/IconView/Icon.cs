using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
    [SerializeField] private IconButtonBase _iconButton;
    [SerializeField] private GameObject _iconLock;
    [SerializeField] private GameObject _clickGuide;
    public IconButtonBase iconButton { get { return _iconButton; } }
    public GameObject iconLock { get { return _iconLock; }  set { _iconLock = value; } }
    public GameObject clickGuide {  get { return _clickGuide; } }
}
