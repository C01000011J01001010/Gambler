using PublicSet;
using UnityEngine;
using UnityEngine.Events;

public class ItemCallback : CallbackBase, ICallback<eItemCallback>
{
    /// <summary>
    /// 콜백함수를 반환하는 함수
    /// </summary>
    /// <param name="index">값으로 콜백함수를 선택함</param>
    /// <returns> 버튼에 연결할 콜백함수 </returns>
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

        // 퀘스트를 수주한 경우
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
            Debug.Log($"허기를 {itemInfo.value_Use.ToString()}만큼 회복했습니다.");
        }
        else
        {
            Debug.LogAssertion("InventoryPopUp이 없음");
        }
    }
}
