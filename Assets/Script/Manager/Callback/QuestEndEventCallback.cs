using PublicSet;
using UnityEngine;
using UnityEngine.Events;

public class QuestEndEventCallback : CallbackBase, ICallback<eQuestType>
{
    public UnityAction CallbackList(eQuestType type)
    {
        switch (type)
        {
            case eQuestType.Collect10000Coins: return GameEnding;

            default: { Debug.Log($"{type.ToString()}는 종료이벤트가 존재하지 않음"); return null; }
        }
    }

    public void GameEnding()
    {
        Debug.LogWarning("수정중");
    }
}
