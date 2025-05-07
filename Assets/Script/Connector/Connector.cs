using UnityEngine;

public class Connector : MonoBehaviour
{
    public GameObject blackView;
    public PopUpViewBase popUpView;

    protected virtual void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.InitConnector(this);
    }
}
