using PublicSet;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardGameRulePopUp : PopUpBase_FullScreen<CardGameRulePopUp>
{
    [SerializeField]private Text description;

    private Dictionary<int, cOnlyOneLivesGameRule> _gameRules;
    private Dictionary<int, cOnlyOneLivesGameRule> gameRules
    {
        get 
        { 
            if(_gameRules == null) _gameRules = CsvManager.Instance.GetGameRule_OnlyOneLives();
            return _gameRules;
        } 
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        description.text = "왼쪽의 항목 클릭 시 내용을 확인하실 수 있습니다.";
    }

    public override void RefreshPopUp()
    {
        RefreshPopUp(gameRules.Count,
            () =>
            {
                for(int i = 0; i < ActiveObjList.Count; i++)
                {
                    
                    RuleButton ruleButton = ActiveObjList[i].GetComponent<RuleButton>();
                    ruleButton.Setpanel(gameRules[i].Title); // 버튼 텍스트 수정

                    int localI = i; // 클로저 참조를 생각하여 람다에 사용할 i값은 지역변수로 사용
                    ruleButton.SetCallback( // 버튼 클릭시 우측 텍스트 수정
                        ()=> 
                        {
                            description.text = gameRules[localI].DescriptionList[0];
                            for (int j = 1; j < gameRules[localI].DescriptionList.Count; j++)
                            {
                                description.text += $"\n\n{gameRules[localI].DescriptionList[j]}";
                            }
                        }
                        );
                }
            });
    }
}
