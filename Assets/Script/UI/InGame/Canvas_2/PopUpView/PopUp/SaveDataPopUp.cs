using PublicSet;
using System;
using UnityEngine;

public class SaveDataPopUp : PopUpBase_FullScreen<SavedPlayerDataPanel>
{
    int count = 0;
    protected override void Awake()
    {
        // None을 제외한 개수
        count = Enum.GetValues(typeof(ePlayerSaveKey)).Length;
        if(Enum.IsDefined(typeof(ePlayerSaveKey), "None")) count--;

        InitializePool(count);
    }

    public override void RefreshPopUp()
    {
        RefreshPopUp(count,
            () =>
            {
                int index = 0;
                foreach (ePlayerSaveKey saveKey in Enum.GetValues(typeof(ePlayerSaveKey)))
                {
                    if (saveKey == ePlayerSaveKey.None) continue;

                    SavedPlayerDataPanel panel = ActiveObjList[index++].GetComponent<SavedPlayerDataPanel>();
                    if (panel != null)
                    {
                        panel.SetPanel(saveKey);
                    }
                    else
                    {
                        Debug.LogAssertion($"{gameObject.name}의 스크립트 확인 바람");
                    }

                }
            });
    }

    
}