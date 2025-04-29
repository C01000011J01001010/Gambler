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

            default: { Debug.Log($"{type.ToString()}�� �����̺�Ʈ�� �������� ����"); return null; }
        }
    }

    public void GameEnding()
    {
        Debug.LogWarning("������");
    }
}
