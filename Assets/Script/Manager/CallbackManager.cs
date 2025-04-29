
using System;
using UnityEngine;

public class CallbackManager : Singleton<CallbackManager>
{
    [SerializeField] private InteractiveTextCallback _interactiveTextCallback;
    [SerializeField] private OnlyOneLivesCallback _onlyOneLivesCallback;
    [SerializeField] private ItemCallback _itemCallback;
    [SerializeField] private QuestCheckCallback _questCheckCallback;
    [SerializeField] private QuestEndEventCallback _questEndEventCallback;

    public InteractiveTextCallback interactiveTextCallback { get { return _interactiveTextCallback; } }
    public OnlyOneLivesCallback onlyOneLivesCallback { get { return _onlyOneLivesCallback; } }
    public ItemCallback itemCallback { get { return _itemCallback; } }
    public QuestCheckCallback questCheckCallback { get { return _questCheckCallback; } }
    public QuestEndEventCallback questEndEventCallback { get { return _questEndEventCallback; } }


    protected override void Awake()
    {
        base.Awake();
    }
}
