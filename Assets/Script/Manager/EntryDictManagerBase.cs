using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntryDictManagerBase<T_Class, T_Entry, T_Type> : Singleton<T_Class>
    where T_Class : EntryDictManagerBase<T_Class, T_Entry, T_Type>
    where T_Entry : IEntryInfo<T_Type>
    where T_Type : Enum
{
    public Dictionary<T_Type, T_Entry> EntryDict { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        InitAllDict();
    }

    public abstract void InitAllDict();
    public abstract void ClearAllDict();
    

    /// <summary>
    /// id는 0부터 시작함
    /// </summary>
    /// <returns></returns>
    public int GetNewLastId()
    {
        if (EntryDict.Count == 0)
        {
            Debug.Log("저장된 데이터가 없음");
            return 0; // 가장 처음 가능한 ID는 0
        }

        // 현재 존재하는 ID만 수집
        HashSet<int> existingIds = new HashSet<int>();
        foreach (var entry in EntryDict.Values)
        {
            existingIds.Add(entry.id);
        }

        // 연속되는 가장 작은 미사용 ID 찾기
        int nextId = 0;
        while (existingIds.Contains(nextId))
        {
            nextId++;
        }

        
        return nextId;
    }

    
}
