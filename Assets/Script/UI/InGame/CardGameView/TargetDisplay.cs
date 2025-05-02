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
        targetButtonList = new List<TargetButton>(targetButtonArr);

        for (int i = 0; i < players.Length; i++)
        {
            targetButtonArr[i].SetPlayer(players[i]);
            targetButtonArr[i].PlayerReadyToPlay();
        }
    }

    public void PlaceRestrictionToAllSelections()
    {
        // 클릭가이드 종료
        //eyeOpenCloseClickGuide.SetActive(false);

        // 파산하지 않은 모든 객체에 동일한 처리를 함
        TargetButton.ClearCurrentSelectedObj();

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

    public void TargetBankrupt(PlayerEtc bankruptTarget)
    {
        for(int i = 0; i< targetButtonList.Count; i++)
        {
            bool result = targetButtonList[i].ComparePlayer(bankruptTarget);

            if (result)
            {
                targetButtonList[i].PlayerBankrupt();
                targetButtonList.RemoveAt(i);
                break;
            }
        }


    }

}
