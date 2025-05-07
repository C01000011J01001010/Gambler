using UnityEngine;
using UnityEngine.UI;

public class CardGameStartButton : MonoBehaviour
{
    // Ä³½Ì
    private CardGameView _GameView;
    public CardGameView GameView
    {
        get
        {
            CheckCardGameView();
            return _GameView;
        }
    }
    private void CheckCardGameView()
    {
        if (_GameView == null)
            _GameView = GameManager.connector_InGame.Canvas0.CardGameView;
    }

    private void Awake()
    {
        CheckCardGameView();
    }

    public void OnButtonClick()
    {
        GameView.StartGame();
    }
}
