using PublicSet;
using UnityEngine;

public class RewardButton : Deactivatable_ButtonBase
{
    public cQuestInfo questInfo { get; private set; }
    public PopUpView_InGame popUpView_InGame { get {  return GameManager.connector_InGame.popUpView_Script; } }   


    private void Start()
    {
        SetButtonCallback(PlayerGetReward);
    }
    
    public void BindQuestToButton(cQuestInfo questInfo)
    {
        this.questInfo = questInfo;
    }

    public void PlayerGetReward()
    {
        if(questInfo.rewardCoin >0)
        {
            PlayManager.Instance.AddPlayerMoney(questInfo.rewardCoin);
        }
        if(questInfo.rewardItemType != eItemType.None)
        {
            ItemManager.Instance.PlayerGetItem(questInfo.rewardItemType);
        }

        popUpView_InGame.checkPopUp.RefreshPopUp(questInfo);
        popUpView_InGame.CheckPopUpOpen();


        // 보상은 1번만
        TryDeactivate_Button();
        questInfo.hasReceivedReward = true;

        questInfo.isNeedCheck = false;

        // 정보대로 팝업을 리프레시
        GameManager.connector_InGame.popUpView_Script.questPopUp.RefreshPopUp();
    }
}
