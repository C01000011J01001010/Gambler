using PublicSet;
using UnityEngine;
using UnityEngine.UI;

public abstract class DescriptionPanelBase<T_OptionButton, T_EntryData, cInfo> : MonoBehaviour
        where T_OptionButton : PopUpOptionButtonBase<T_OptionButton, T_EntryData, cInfo>
        where T_EntryData : class
        where cInfo : class, INeedCheck
{
    [SerializeField] protected Text contentName;
    [SerializeField] protected Text description;

    protected PopUpView_InGame popUpView => GameManager.connector_InGame.popUpViewAsInGame;

    protected abstract void ButtonSetActive(bool active);

    public virtual void SetPanel(T_OptionButton optionButton, cItemInfo itemInfo)
    {
        // �̸� ����
        contentName.text = itemInfo.name;

        // ���� ����
        if (itemInfo.descriptionList.Count > 0)
        {
            description.text = itemInfo.descriptionList[0];

            for (int i = 1; i < itemInfo.descriptionList.Count; i++)
            {
                description.text += $"\n{itemInfo.descriptionList[i]}";
            }
        }
        else
        {
            Debug.LogWarning($"item {itemInfo.name}�� ������ �����ϴ�.");
        }
    }

    public void ClearPanel()
    {
        contentName.text = string.Empty;
        description.text = "������ �׸� Ŭ�� �� ������ Ȯ���Ͻ� �� �ֽ��ϴ�.";
        ButtonSetActive(false);
    }
}
