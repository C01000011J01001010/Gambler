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
    /// id�� 0���� ������
    /// </summary>
    /// <returns></returns>
    public int GetNewLastId()
    {
        if (EntryDict.Count == 0)
        {
            Debug.Log("����� �����Ͱ� ����");
            return 0; // ���� ó�� ������ ID�� 0
        }

        // ���� �����ϴ� ID�� ����
        HashSet<int> existingIds = new HashSet<int>();
        foreach (var entry in EntryDict.Values)
        {
            existingIds.Add(entry.id);
        }

        // ���ӵǴ� ���� ���� �̻�� ID ã��
        int nextId = 0;
        while (existingIds.Contains(nextId))
        {
            nextId++;
        }

        
        return nextId;
    }

    
}
