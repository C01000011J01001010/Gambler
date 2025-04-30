using System.Collections.Generic;
using UnityEngine;

public class TargetDisplay : MonoBehaviour
{
    public GameObject eyeOpenCloseClickGuide;
    public PlayerEtc[] players;
    public TargetButton[] targetButtonArr;
    public List<TargetButton> targetButtonList { get; private set; } // �Ļ�� 1�� ����
    

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
        // Ŭ�����̵� ����
        eyeOpenCloseClickGuide.SetActive(false);

        for (int i = 0; i < targetButtonList.Count; i++)
        {
            targetButtonList[i].TryActivate_Button();
            targetButtonList[i].ClearButtonCallback();
        }
    }

    public void LiftRestrictionToAllSelections()
    {
        // Ŭ�����̵� �ѱ�
        eyeOpenCloseClickGuide.SetActive(true);

        for (int i = 0; i < targetButtonList.Count; i++)
        {
            targetButtonList[i].TryActivate_Button();

            // ���� ��� ���ý� �⺻ �ݹ��Լ� ȣ�� �� Ŭ�����̵� ����
            targetButtonList[i].InitCallback(()=>eyeOpenCloseClickGuide.SetActive(false));
        }
    }

}
