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
        targetButtonList = new List<TargetButton>(targetButtonArr);

        for (int i = 0; i < players.Length; i++)
        {
            targetButtonArr[i].SetPlayer(players[i]);
            targetButtonArr[i].PlayerReadyToPlay();
        }
    }

    public void PlaceRestrictionToAllSelections()
    {
        // Ŭ�����̵� ����
        //eyeOpenCloseClickGuide.SetActive(false);

        // �Ļ����� ���� ��� ��ü�� ������ ó���� ��
        TargetButton.ClearCurrentSelectedObj();

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
