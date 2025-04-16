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
        description.text = "������ �׸� Ŭ�� �� ������ Ȯ���Ͻ� �� �ֽ��ϴ�.";
    }

    public override void RefreshPopUp()
    {
        RefreshPopUp(gameRules.Count,
            () =>
            {
                for(int i = 0; i < ActiveObjList.Count; i++)
                {
                    
                    RuleButton ruleButton = ActiveObjList[i].GetComponent<RuleButton>();
                    ruleButton.Setpanel(gameRules[i].Title); // ��ư �ؽ�Ʈ ����

                    int localI = i; // Ŭ���� ������ �����Ͽ� ���ٿ� ����� i���� ���������� ���
                    ruleButton.SetCallback( // ��ư Ŭ���� ���� �ؽ�Ʈ ����
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
