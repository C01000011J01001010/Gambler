using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public enum checkCase
{
    @default,
    QuestComplete
}
public class CheckPopUp : SimplePopUpBase
{
    [SerializeField] private CaseQuest caseQuest;

    protected void PopUpUpChange(checkCase checkCase)
    {
        switch(checkCase)
        {
            case checkCase.@default:
                caseQuest.gameObject.SetActive(false); 
                mainDescription.gameObject.SetActive(true);
                break;

            case checkCase.QuestComplete:
                caseQuest.gameObject.SetActive(true);
                mainDescription.gameObject.SetActive(false);
                break;
        }
    }

    public override void UpdateMainDescription(List<string> descriptionList)
    {
        PopUpUpChange(checkCase.@default);

        if (descriptionList.Count > 0)
            mainDescription.text = descriptionList[0];

        // 2개 이상의 문자열인 경우 줄바꿈을 적용함
        for (int i = 1; i < descriptionList.Count; i++)
        {
            mainDescription.text += $"\n{descriptionList[i]}";
        }
    }

    public override void UpdateMainDescription(string description)
    {
        PopUpUpChange(checkCase.@default);
        mainDescription.text = description;
    }

    public void RefreshPopUp(cQuestInfo questInfo)
    {
        PopUpUpChange(checkCase.QuestComplete);
        caseQuest.SetPanel(questInfo);
    }
}
