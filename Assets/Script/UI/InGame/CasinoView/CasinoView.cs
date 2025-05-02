using PublicSet;
using UnityEngine;

public class CasinoView : MonoBehaviour
{
    [SerializeField] private OnlyOneLivesButton _onlyOneLivesButton;
    public OnlyOneLivesButton onlyOneLivesButton { get { return _onlyOneLivesButton; } }

    public void InitGameButtonCallback()
    {
        onlyOneLivesButton.InitButtonCallback();
    }



    public void StartDealerDialogue()
    {
        GameManager.connector_InGame.Canvas1.TextWindowView.StartTextWindow(eSystemGuide.WelcomeFromGM);
    }
}
