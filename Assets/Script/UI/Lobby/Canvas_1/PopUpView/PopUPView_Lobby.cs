using UnityEngine;
public class PopUPView_Lobby : PopUpViewBase
{
    public ContinuePopUp continuePopUp;
    public GameSettingPopUp gameSettingPopUp;

    public override void MakePopUpSingleTone()
    {
        continuePopUp.MakeSingleTone();
        gameSettingPopUp.MakeSingleTone();
    }

    public void ContinuePopUpOpen()
    {
        continuePopUp.gameObject.SetActive(true);
        continuePopUp.transform.SetAsLastSibling();
    }

    public void GameSettingPopUpOpen()
    {
        gameSettingPopUp.gameObject.SetActive(true);
        gameSettingPopUp.transform.SetAsLastSibling();
    }

    
}
