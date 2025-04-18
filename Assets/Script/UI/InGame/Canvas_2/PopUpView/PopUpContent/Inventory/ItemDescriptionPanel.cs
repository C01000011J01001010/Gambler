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
        // ������ �̸� ����
        itemName.text = itemInfo.name;

        // ���� ����
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
            Debug.LogWarning($"item {itemInfo.name}�� ������ �����ϴ�.");
        }
        


        // �������� ��밡���� ��� 
        if (itemInfo.isAvailable)
        {
            // ��ư�� Ȱ��ȭ
            ButtonSetActive(true);

            // ����ϱ� ������ Ȯ��â ����
            if (item.useCheckCallback != null)
                useButton.SetButtonCallback(item.useCheckCallback);

            // ������ ����ϱ�
            //RegisterButton.SetButtonCallback();


        }

        // ��Ÿ ������ ��� 
        else if (itemInfo.isAvailable == false)
        {
            // ��ư�� ��Ȱ��ȭ
            ButtonSetActive(false);
        }
    }

    public void ClearPanel()
    {
        itemName.text = "������ �̸�";
        description.text = "������ �׸� Ŭ�� �� ������ Ȯ���Ͻ� �� �ֽ��ϴ�.";
        ButtonSetActive(false);
    }
}
