using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PublicSet;

public class PopUpView_InGame : PopUpViewBase
{
    // ������ ����

    // ������ �˾�
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
        // csv�� ���������� �ε�Ǵ� ���� �ٸ��� ��������
        questPopUp.RefreshPopUp();
        inventoryPopUp.RefreshPopUp();
    }



    /// <summary>
    /// �˾��� awake���� �̱������ ȣ���� ���� ����
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

    
    // ��ư �ݹ�
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

    // ��ư �ݹ�
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

    

    

    // ��ư �ݹ�
    public void CardGameRulePopUpOpen()
    {
        //gameObject.SetActive(true);
        CardGameRulePopUp.gameObject.SetActive(true);
        CardGameRulePopUp.transform.SetAsLastSibling();
    }
}
