using PublicSet;
using System;
using UnityEngine;

public abstract class DataManagePopUp<T_Class> : PopUpBase_FullScreen<T_Class> where T_Class : MonoBehaviour
{
    protected int DataCount = 0;

    protected override void Awake()
    {
        AdjustContentCellSize();
        // None을 제외한 개수
        DataCount = Enum.GetValues(typeof(ePlayerSaveKey)).Length - 1;
        InitializePool(DataCount);
    }

    protected void OnEnable()
    {
        RefreshPopUp();
        ScrollToTop();
    }

    public override void RefreshPopUp()
    {
        RefreshPopUp(DataCount,
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
