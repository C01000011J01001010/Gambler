using PublicSet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionPanel : MonoBehaviour
{
    [SerializeField] private Text itemName;
    [SerializeField] private Text description;
    [SerializeField] private ItemUseButton useButton;
    [SerializeField] private ItemRegisterButton RegisterButton;

    private PopUpView_InGame popUpView
    {
        get
        {
            return GameManager.connector_InGame.popUpView_Script;
        }
    }

    private void ButtonSetActive(bool active)
    {
        useButton.gameObject.SetActive(active);
        RegisterButton.gameObject.SetActive(active);
    }

    public void SetPanel(ItemDefault item, cItemInfo itemInfo)
    {
        // 아이템 이름 설정
        itemName.text = itemInfo.name;

        // 설명문 설정
        if(itemInfo.descriptionList.Count >0)
        {
            description.text = itemInfo.descriptionList[0];

            for(int i = 1; i < itemInfo.descriptionList.Count; i++)
            {
                description.text += $"\n{itemInfo.descriptionList[i]}";
            }
        }
        else
        {
            Debug.LogWarning($"item {itemInfo.name}은 설명문이 없습니다.");
        }
        


        // 아이템이 사용가능한 경우 
        if (itemInfo.isAvailable)
        {
            // 버튼셋 활성화
            ButtonSetActive(true);

            // 사용하기 누를시 확인창 열림
            if (item.useCheckCallback != null)
                useButton.SetButtonCallback(item.useCheckCallback);

            // 퀵슬롯 등록하기
            //RegisterButton.SetButtonCallback();


        }

        // 기타 잡템의 경우 
        else if (itemInfo.isAvailable == false)
        {
            // 버튼셋 비활성화
            ButtonSetActive(false);
        }
    }

    public void ClearPanel()
    {
        itemName.text = "아이템 이름";
        description.text = "왼쪽의 항목 클릭 시 내용을 확인하실 수 있습니다.";
        ButtonSetActive(false);
    }
}
