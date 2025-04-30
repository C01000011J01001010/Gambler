using System.Collections.Generic;
using UnityEngine;

public class TargetDisplay : MonoBehaviour
{
    public GameObject eyeOpenCloseClickGuide;
    public PlayerEtc[] players;
    public TargetButton[] targetButtonArr;
    public List<TargetButton> targetButtonList { get; private set; } // 파산시 1명씩 제거
    

    public void InitAttribute()
    {
        InitPlayers();
        targetButtonList = new List<TargetButton>(targetButtonArr);

        foreach (var button in targetButtonArr)
        {
            button.TryActivate_Button();
        }
    }

    public void InitPlayers()
    {
        for(int i = 0; i < players.Length; i++)
        {
            targetButtonArr[i].SetPlayer(players[i]);
        }
    }

    public void PlaceRestrictionToAllSelections()
    {
        // 클릭가이드 종료
        eyeOpenCloseClickGuide.SetActive(false);

        for (int i = 0; i < targetButtonList.Count; i++)
        {
            targetButtonList[i].TryActivate_Button();
            targetButtonList[i].ClearButtonCallback();
        }
    }

    public void LiftRestrictionToAllSelections()
    {
        // 클릭가이드 켜기
        eyeOpenCloseClickGuide.SetActive(true);

        for (int i = 0; i < targetButtonList.Count; i++)
        {
            targetButtonList[i].TryActivate_Button();

            // 공격 대상 선택시 기본 콜백함수 호출 후 클릭가이드 종료
            targetButtonList[i].InitCallback(()=>eyeOpenCloseClickGuide.SetActive(false));
        }
    }

}
