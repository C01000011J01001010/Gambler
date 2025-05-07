using PublicSet;
using Unity.VisualScripting;
using UnityEngine;


// ¾Àº¹±Í½Ã ½Ì±ÛÅæ °´Ã¼ÀÇ Ã³¸® ´ã´ç
public class Connector_InGame : Connector
{
    [SerializeField] private InGame_Canvas0 _canvas0;
    [SerializeField] private InGame_Canvas1 _canvas1;
    [SerializeField] private InGame_Canvas2 _canvas2;

    [SerializeField] private GameObject _player;
    [SerializeField] private Map _map;
    [SerializeField] private Box _box;

    public PopUpView_InGame popUpViewAsInGame { get { return popUpView as PopUpView_InGame; } }

    public InGame_Canvas0 Canvas0 => _canvas0;
    public InGame_Canvas1 Canvas1 => _canvas1;
    public InGame_Canvas2 Canvas2 => _canvas2;
    public GameObject Player => _player;
    public Map Map => _map;
    public Box Box => _box;


    protected override void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.InitConnector(this);
    }
}
