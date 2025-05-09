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
        // 이름 설정
        contentName.text = itemInfo.name;

        // 설명문 설정
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
            Debug.LogWarning($"item {itemInfo.name}은 설명문이 없습니다.");
        }
    }

    public void ClearPanel()
    {
        contentName.text = string.Empty;
        description.text = "왼쪽의 항목 클릭 시 내용을 확인하실 수 있습니다.";
        ButtonSetActive(false);
    }
}
