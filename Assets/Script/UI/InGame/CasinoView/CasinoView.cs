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
        TextWindowView textViewScript = (GameManager.connector as Connector_InGame).textWindowView.GetComponent<TextWindowView>();
        if (textViewScript != null)
        {
            textViewScript.StartTextWindow(eSystemGuide.WelcomeFromGM);
            //textViewScript.StartTextWindow(eTextScriptFile.GameMaster);
        }
    }
}
