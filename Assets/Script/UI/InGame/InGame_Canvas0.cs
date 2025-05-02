using PublicSet;
using UnityEngine;

public class InGame_Canvas0 : MonoBehaviour
{
    [SerializeField] private GameObject _InterfaceView;
    [SerializeField] private CasinoView _CasinoView;
    [SerializeField] private CardGameView _CardGameView;


    public GameObject InterfaceView => _InterfaceView;
    public CasinoView CasinoView => _CasinoView;
    public CardGameView CardGameView => _CardGameView;



    void Start()
    {
        CloseAllOfView();
        InterfaceView.SetActive(true);
    }

    public void CloseAllOfView()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf == true)
            {
                child.gameObject.SetActive(false);
            }
        }
    }


    public void CasinoViewOpen()
    {
        CloseAllOfView();
        GameManager.Instance.ChangeCardGameView(true);
        CasinoView.gameObject.SetActive(true);
    }

    public void CasinoViewClose()
    {
        float delay = 2.0f;
        GameManager.Instance.ChangeCardGameView(false);
        CallbackBase.PlaySequnce_BlackViewProcess(
            delay,
            CloseAllOfView,
            () => InterfaceView.SetActive(true)
            );
    }

    

}
