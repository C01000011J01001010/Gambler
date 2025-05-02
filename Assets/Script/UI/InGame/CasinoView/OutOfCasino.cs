using UnityEngine;

public class OutOfCasino : MonoBehaviour
{
    public InGame_Canvas0 MainCanvas;

    public void Button_OutOfCasino()
    {
        MainCanvas.CasinoViewClose();
    }
}
