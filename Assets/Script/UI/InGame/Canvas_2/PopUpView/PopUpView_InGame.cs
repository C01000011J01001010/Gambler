using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PublicSet;

public class PopUpView_InGame : PopUpViewBase
{
    // 에디터 연결

    // 아이콘 팝업
    public GameObject optionPopUp;
    public InventoryPopUp inventoryPopUp;
    public QuestPopUp questPopUp;
    public GameAssistantPopUp_OnlyOneLives gameAssistantPopUp_OnlyOneLives;
    public SaveDataPopUp saveDataPopUp;
    public CardGameRulePopUp CardGameRulePopUp;


    protected override void Awake()
    {
        MakePopUpSingleTone();
        InitPopUp();
    }
    /// <summary>
    /// 팝업은 awake에서 싱글톤생성 호출을 하지 않음
    /// </summary>
    public override void MakePopUpSingleTone()
    {
        inventoryPopUp.MakeSingleTone();
        questPopUp.MakeSingleTone();
        gameAssistantPopUp_OnlyOneLives.MakeSingleTone();
        CardGameRulePopUp.MakeSingleTone();
        saveDataPopUp.MakeSingleTone();
        CardGameRulePopUp.MakeSingleTone();
    }
    public void InitPopUp()
    {
        inventoryPopUp.InitAttribute();
    }

    
    // 버튼 콜백
    public void OptionPopUpOpen()
    {
        //gameObject.SetActive(true);
        optionPopUp.SetActive(true);
        optionPopUp.transform.SetAsLastSibling();
    }

    public void InventoryPopUpOpen()
    {
        //gameObject.SetActive(true);
        inventoryPopUp.gameObject.SetActive(true);
        inventoryPopUp.transform.SetAsLastSibling();
    }

    // 버튼 콜백
    public void QuestPopUpOpen()
    {
        //gameObject.SetActive(true);
        questPopUp.gameObject.SetActive(true);
        questPopUp.transform.SetAsLastSibling();
    }

    //public void QuestContentPopUpOpen()
    //{
    //    //gameObject.SetActive(true);
    //    questContentPopUp.gameObject.SetActive(true);
    //    questContentPopUp.transform.SetAsLastSibling();
    //}

    public void GameAssistantPopUpOpen_OnlyOneLives()
    {
        if(CardGamePlayManager.Instance.cardGameView.gameObject.activeInHierarchy)
        {
            gameAssistantPopUp_OnlyOneLives.gameObject.SetActive(true);
            gameAssistantPopUp_OnlyOneLives.transform.SetAsLastSibling();
            return;
        }
        else
        {
            GameManager.connector_InGame.textWindowView_Script.StartTextWindow(eSystemGuide.GameAssistantNotAvailable);
            return;
        }
        
    }

    public void SaveDataPopUpOpen()
    {
        //gameObject.SetActive(true);
        saveDataPopUp.gameObject.SetActive(true);
        saveDataPopUp.transform.SetAsLastSibling();
    }

    

    

    // 버튼 콜백
    public void CardGameRulePopUpOpen()
    {
        //gameObject.SetActive(true);
        CardGameRulePopUp.gameObject.SetActive(true);
        CardGameRulePopUp.transform.SetAsLastSibling();
    }
}
