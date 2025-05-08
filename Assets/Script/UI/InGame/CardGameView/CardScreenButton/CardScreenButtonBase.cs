using UnityEngine;

public class CardScreenButtonBase : Deactivatable_ButtonBase
{
    // Ä³½Ì
    private CardScreen _cardScreen;

    public CardScreen cardScreen
    {
        get
        {
            CheckCardScreen();
            return _cardScreen;
        }
    }

    protected virtual void CheckCardScreen()
    {
        if (_cardScreen == null)
            _cardScreen = GameManager.connector_InGame.Canvas0.CardGameView.cardScreen;
    }
}
