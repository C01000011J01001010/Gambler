using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PublicSet;

public class PopUpView_InGame : PopUpViewBase
{
    // 에디터 연결

    // 아이콘 팝업
    public OptionPopUp optionPopUp;
    public GameSettingPopUp gameSettingPopUp;

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

    private void Start()
    {
        // csv에 접근하지만 로드되는 씬이 다르니 문제없음
        questPopUp.RefreshPopUp();
        inventoryPopUp.RefreshPopUp();
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
        optionPopUp.gameObject.SetActive(true);
        optionPopUp.transform.SetAsLastSibling();
    }

    public void GameSettingPopUpOpen()
    {
        gameSettingPopUp.gameObject.SetActive(true);
        gameSettingPopUp.transform.SetAsLastSibling();
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
            GameManager.connector_InGame.Canvas1.TextWindowView.StartTextWindow(eSystemGuide.GameAssistantNotAvailable);
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
