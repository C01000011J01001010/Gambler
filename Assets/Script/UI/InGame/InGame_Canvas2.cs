using UnityEngine;

public class InGame_Canvas2 : MonoBehaviour
{
    [SerializeField] private GameObject _BlackView;
    [SerializeField] private EventView _EventView;
    [SerializeField] private YouLoseView _YouLoseView;

    public GameObject BlackView => _BlackView;
    public EventView EventView => _EventView;
    public YouLoseView YouLoseView => _YouLoseView;
}
