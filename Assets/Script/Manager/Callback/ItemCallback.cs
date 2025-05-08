using PublicSet;
using UnityEngine;
using UnityEngine.Events;

public class ItemCallback : CallbackBase, ICallback<eItemCallback>
{
    /// <summary>
    /// �ݹ��Լ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="index">������ �ݹ��Լ��� ������</param>
    /// <returns> ��ư�� ������ �ݹ��Լ� </returns>
    public UnityAction CallbackList(eItemCallback index)
    {
        switch (index)
        {
            case eItemCallback.CasinoTicket: return CasinoTicket;
            case eItemCallback.EatMeal: return EatMeal;

            default: return TrashFuc;
        }
    }


    public void CasinoTicket()
    {
        GameManager.Instance.NextStage();
        EventManager.Instance.SetEventMessage(stageMessageDict[currentStage]);
        EventManager.Instance.PlaySequnce_EventAnimation();

        // ����Ʈ�� ������ ���
        sQuest quest = new sQuest(0, eQuestType.UseCasinoEntryTicket);
        if (QuestManager.questHashSet.Contains(quest))
        {
            cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);
            if (questInfo.isComplete == false) questInfo.checkEndCondition();
        }

    }


    public void EatMeal()
    {
        InventoryPopUp inven = connector_InGame.popUpViewAsInGame.inventoryPopUp.GetComponent<InventoryPopUp>();
        if (inven != null)
        {
            cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(inven.currentClickItem.type);
            Debug.Log($"��⸦ {itemInfo.value_Use.ToString()}��ŭ ȸ���߽��ϴ�.");
        }
        else
        {
            Debug.LogAssertion("InventoryPopUp�� ����");
        }
    }
}
