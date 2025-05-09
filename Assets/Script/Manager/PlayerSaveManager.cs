using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;


public interface IEntryInfo<T_Enum>
    where T_Enum : Enum
{
    public int id { get; set; }
    public T_Enum type { get; set; }
}

/// <summary>
/// ������ ���� �ڷᱸ��
/// </summary>
public class cPlayerItem : IEntryInfo<eItemType>
{
    /// <summary>
    /// ���� �����ϰ� �ִ� ������ ��ȣ, 0���� ����
    /// </summary>
    public int id {  get; set; }

    /// <summary>
    /// ������ �𵨸�
    /// </summary>
    public eItemType type { get; set; }

    /// <summary>
    /// ������ ����
    /// </summary>
    private int _quantity;

    public int quantity
    {
        get => _quantity;
        set => _quantity = value > 0 ? value : 1;
    }

    // ������ ������ ���� string���� ��ȯ
    public override string ToString()
    {
        // �߰� ������ ���� ����
        cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(type);
        return $"{id}:{type}:{quantity}:{itemInfo.isNeedCheck.ToString()}";
    }

    // string���� �����ߴ� ������ ��밡���� �����ͷ� ��ȯ
    public static cPlayerItem DataSplit(string data)
    {
        string[] parts = data.Split(':');

        if (parts.Length == 4 &&
            int.TryParse(parts[0], out int id) &&
            eItemType.TryParse(parts[1], out eItemType type)&&
            int.TryParse(parts[2], out int quantity) &&
            bool.TryParse(parts[3], out bool isNeedCheck))
        {
            cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(type);
            itemInfo.isNeedCheck = isNeedCheck;

            return new cPlayerItem(id, type, quantity); ;
        }
        else
        {
            Debug.LogWarning("DataSplit ����");
        }

        return default;
    }

    /*
    public bool Equals(sItem other)
    {
        return this.type == other.type;
    }

    public override bool Equals(object obj)
    {
        // ���ϸ�Ī�� ���� ���Ǻ� ��
        return obj is sItem other && Equals(other);
        //if (obj is sQuest)
        //{
        //    sQuest other = (sQuest)obj;
        //    return Equals(other);
        //}
    }

    public override int GetHashCode()
    {
        return type.GetHashCode();
    }
    */

    
    public cPlayerItem(int id, eItemType type, int quantity)
    {
        this.id = id;
        this.type = type;
        this.quantity = quantity;
        return;
    }

    public cPlayerItem(cPlayerItem item)
    {
        id = item.id;
        type = item.type;
        quantity = item.quantity;
        return;
    }
}

/// <summary>
/// ����Ʈ ���� �ڷᱸ��
/// </summary>
public class cPlayerQuest : IEntryInfo<eQuestType> //: IEquatable<sQuest>
{
    public int id { get; set; } // ����Ʈ ����
    public eQuestType type { get; set; } // ����Ʈ ��ȣ

    // ������ ������ ���� string���� ��ȯ
    public override string ToString()
    {
        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(type);

        // ���������� �Ϻ� �����͸� ���� ����
        return $"{id}:{type}:{questInfo.isNeedCheck.ToString()}:{questInfo.isComplete.ToString()}:{questInfo.hasReceivedReward.ToString()}";
    }

    // string���� �����ߴ� ������ ��밡���� �����ͷ� ��ȯ
    public static cPlayerQuest DataSplit(string data)
    {
        string[] parts = data.Split(':');

        if (parts.Length == 5 &&
            int.TryParse(parts[0], out int id) &&
            eQuestType.TryParse(parts[1], out eQuestType type)&&
            bool.TryParse(parts[2], out bool isNeedCheck) &&
            bool.TryParse(parts[3], out bool isComplete) &&
            bool.TryParse(parts[4], out bool hasReceivedReward)
            )
            
        {
            // �����͸� �ҷ����鼭 ���������� �����͵� ���� ����
            cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(type);
            questInfo.isNeedCheck = isNeedCheck;
            questInfo.isComplete = isComplete;
            questInfo.hasReceivedReward = hasReceivedReward;

            return new cPlayerQuest(id, type); ;
        }
        return default;
    }
    /*
    public bool Equals(sQuest other)
    {
        return this.type == other.type;
    }

    public override bool Equals(object obj)
    {
        // ���ϸ�Ī�� ���� ���Ǻ� ��
        return obj is sQuest other && Equals(other);
        //if (obj is sQuest)
        //{
        //    sQuest other = (sQuest)obj;
        //    return Equals(other);
        //}
    }

    public override int GetHashCode()
    {
        return type.GetHashCode();
    }
    */


    public cPlayerQuest(int id, eQuestType type)
    {
        this.id = id;
        this.type = type;
        return;
    }

    public cPlayerQuest(cPlayerQuest quest)
    {
        id = quest.id;
        type = quest.type;
        return;
    }

}

/// <summary>
/// �÷��̾� ������ �ڷᱸ��
/// </summary>
public struct sPlayerStatus
{
    public int hp; // ü��
    public int agility; // ��ø��
    public int hunger; // ���
    public int coin; // ������



    public override string ToString()
    {
        return $"{hp}:{agility}:{hunger}:{coin}";
    }

    // string���� �����ߴ� ������ ��밡���� �����ͷ� ��ȯ
    public static sPlayerStatus DataSplit(string data)
    {
        sPlayerStatus playerStatus = new sPlayerStatus();
        string[] parts = data.Split(':');

        if (parts.Length == 4 &&
            int.TryParse(parts[0], out playerStatus.hp) &&
            int.TryParse(parts[1], out playerStatus.agility) &&
            int.TryParse(parts[2], out playerStatus.hunger) &&
            int.TryParse(parts[3], out playerStatus.coin))
        {
            return playerStatus;
        }

        // ��Ÿ���� ��ȯ���ΰ�� �� �Ӽ��� �⺻���� �ش� ��Ÿ���� ��ȯ
        // int�� �⺻���� 0
        // float�� �⺻���� 0.0f
        // bool�� �⺻���� false
        // ����Ÿ���� ��ȯ���ΰ�� �⺻������ null�� ��ȯ
        return default;
    }

    public static sPlayerStatus defaultData
    {
        get
        {
            return default;
        }
        
    }

    public override bool Equals(object obj)
    {
        return obj is sPlayerStatus status &&
               hp == status.hp &&
               agility == status.agility &&
               hunger == status.hunger &&
               coin == status.coin;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(hp, agility, hunger, coin);
    }

    // == ������ ������
    public static bool operator ==(sPlayerStatus left, sPlayerStatus right)
    {
        // ��ü�� �������� üũ
        return left.hp == right.hp &&
               left.agility == right.agility &&
               left.hunger == right.hunger &&
               left.coin == right.coin;
    }

    // != ������ ������
    public static bool operator !=(sPlayerStatus left, sPlayerStatus right)
    {
        return !(left == right);
    }

    // ������
    public sPlayerStatus(int hp, int agility, int hunger, int money)
    {
        this.hp = hp;
        this.agility = agility;
        this.hunger = hunger;
        this.coin = money;
    }

    public sPlayerStatus(sPlayerStatus status)
    {
        hp = status.hp;
        agility = status.agility;
        hunger = status.hunger;
        coin = status.coin;
    }
}

public class PlayerSaveManager : Singleton<PlayerSaveManager>
{
    string currentPlayerSaveKey { get { return GameManager.Instance.currentPlayerSaveKey.ToString(); } }

    public const string defaultSaveKey_SavedDate = "SavedDate"; // �����Ͱ� ����� ��¥�� �ð��� ����
    public const string defaultSaveKey_Items = "SavedItems";
    public const string defaultSaveKey_Quests = "SavedQuests";
    public const string defaultSaveKey_RemainingPeriod = "SavedRemainingPeriod";
    public const string defaultSaveKey_Stage = "SavedStage";
    public const string defaultSaveKey_PlayerStatus = "SavedPlayerStatus";
    public const string defaultSaveKey_OpenedIconCount = "SavedOpenedIconCount";
    public const string defaultSaveKey_DynamicInteractable = "SavedDynamicInteractable";

    public string currentPlayerSaveKey_SavedDate
    { get { return (currentPlayerSaveKey + defaultSaveKey_SavedDate); } }
    public string currentPlayerSaveKey_Items
    { get { return (currentPlayerSaveKey + defaultSaveKey_Items); } }
    public string currentPlayerSaveKey_Quests
    { get { return (currentPlayerSaveKey + defaultSaveKey_Quests); } }
    public string currentPlayerSaveKey_RemainingPeriod
    { get { return (currentPlayerSaveKey + defaultSaveKey_RemainingPeriod); } }
    public string currentPlayerSaveKey_Stage
    { get { return (currentPlayerSaveKey + defaultSaveKey_Stage); } }
    public string currentPlayerSaveKey_PlayerStatus
    { get { return (currentPlayerSaveKey + defaultSaveKey_PlayerStatus); } }
    public string currentPlayerSaveKey_OpenedIconCount
    { get { return (currentPlayerSaveKey + defaultSaveKey_OpenedIconCount); } }
    public string currentPlayerSaveKey_DynamicInteractable
    { get { return (currentPlayerSaveKey + defaultSaveKey_DynamicInteractable); } }

    public void SaveTotalData()
    {
        Debug.Log($"SaveData -> currentPlayer : {currentPlayerSaveKey}");

        // ����Ǵ� ��¥
        string SavedDate = DateTime.Now.ToString("yyyy�� MM�� dd��:HH�� mm�� ss��");

        // ��� ���
        Debug.Log($"���� �ð� : {SavedDate}");

        SaveData(currentPlayerSaveKey_SavedDate, SavedDate);

        //LoadItems(ePlayerSaveKey.None);
        string itemData = string.Join(",", ItemManager.Instance.PlayerItemDict);
        SaveData(currentPlayerSaveKey_Items, itemData);

        //LoadQuests(ePlayerSaveKey.None);
        string questData = string.Join(",", QuestManager.Instance.PlayerQuestDict);
        SaveData(currentPlayerSaveKey_Quests, questData);

        int remainingPeriod = GameManager.Instance.currentRemainingPeriod;
        SaveData(currentPlayerSaveKey_RemainingPeriod, remainingPeriod);

        int numStage = (int)GameManager.Instance.currentStage;
        SaveData(currentPlayerSaveKey_Stage, numStage);

        string playerStatus = PlayManager.Instance.currentPlayerStatus.ToString();
        SaveData(currentPlayerSaveKey_PlayerStatus, playerStatus);

        int openedIconCount = GameManager.connector_InGame.Canvas1.IconView.OpenedIconCount;
        SaveData(currentPlayerSaveKey_OpenedIconCount, openedIconCount);

        string dynamicIntaractableData = DynamicInteractableBase.GetStringToSave();
        SaveData(currentPlayerSaveKey_DynamicInteractable, dynamicIntaractableData);
    }
    

    //private void OnDisable()
    //{
    //    DeleteDefaultData();
    //}

    ///// <summary>
    ///// ���� ����� ���̵����ʹ� ��� ����
    ///// </summary>
    //private void DeleteDefaultData()
    //{
    //    string playerKey = ePlayerSaveKey.None.ToString();
    //    PlayerPrefs.DeleteKey(playerKey + defaultSaveKey_SavedDate);
    //    PlayerPrefs.DeleteKey(playerKey + defaultSaveKey_Items);
    //    PlayerPrefs.DeleteKey(playerKey + defaultSaveKey_Quests);
    //    PlayerPrefs.DeleteKey(playerKey + defaultSaveKey_RemainingPeriod);
    //    PlayerPrefs.DeleteKey(playerKey + defaultSaveKey_Stage);
    //    PlayerPrefs.DeleteKey(playerKey + defaultSaveKey_PlayerStatus);
    //}

    

    
    

    protected override void Awake()
    {
        base.Awake();
        
    }


    public string LoadSavedDate(ePlayerSaveKey saveKey)
    {
        string savedData = LoadData(saveKey.ToString() + defaultSaveKey_SavedDate, string.Empty);

        Debug.Log($"savedData : {savedData}");
        if (string.IsNullOrEmpty(savedData))
        {
            Debug.LogWarning("�������� ����");
            return string.Empty;
        }
        else
        {
            Debug.Log("�������� ����");
            return savedData;
        }
    }

    public Dictionary<eItemType, cPlayerItem> LoadItems(ePlayerSaveKey saveKey)
    {
        ItemManager.Instance.ClearAllDict();

        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        string savedData = LoadData(saveKey.ToString() + defaultSaveKey_Items, string.Empty);
        
        Debug.Log($"savedData : {savedData}");
        if (string.IsNullOrEmpty(savedData))
        {
            Debug.LogWarning("����� �����Ͱ� ���� : LoadItems");
            return new Dictionary<eItemType, cPlayerItem>();
        }

        // id1:serail1 , id2:serail2 ....
        string[] itemStrings = savedData.Split(',');

        foreach (string itemString in itemStrings)
        {
            // id : serail
            cPlayerItem item = cPlayerItem.DataSplit(itemString);

            // �����Ͱ� �߸��Ȱ�� �н�
            if (item.id == 0 && item.type == 0 && item.quantity == 0)
            {
                continue;
            }

            // �ùٸ� �����͸� ��ųʸ��� �߰�
            else
            {
                ItemManager.Instance.PlayerItemDict.Add(item.type, item);
            }
        }

        Debug.Log("������ �ε� ���� : LoadItems");
        return ItemManager.Instance.PlayerItemDict;
    }

    public Dictionary<eQuestType, cPlayerQuest> LoadQuests(ePlayerSaveKey saveKey)
    {
        QuestManager.Instance.ClearAllDict();

        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        string savedData = LoadData(saveKey.ToString() + defaultSaveKey_Quests, string.Empty);

        Debug.Log($"savedData : {savedData}");
        if (string.IsNullOrEmpty(savedData))
        {
            QuestManager.Instance.PlayerQuestDict.Clear();
            return new Dictionary<eQuestType, cPlayerQuest>();
        }

        string[] questStrings = savedData.Split(',');

        foreach (string questString in questStrings)
        {
            // id : serail
            cPlayerQuest quest = cPlayerQuest.DataSplit(questString);
            cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);

            // �����Ͱ� �߸��Ȱ�� �н�
            if (quest.id == 0 && quest.type == 0)
            {
                continue;
            }

            // �ùٸ� �����͸� ��ųʸ��� �߰�
            else
            {
                QuestManager.Instance.PlayerQuestDict.Add(quest.type, quest);
                QuestManager.Instance.TryAddRefeatableQuest(questInfo);
            }
        }
        return QuestManager.Instance.PlayerQuestDict;
    }
    public int LoadRemainingPeriod(ePlayerSaveKey saveKey)
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        int savedData = LoadData(saveKey.ToString() + defaultSaveKey_RemainingPeriod, 0);

        Debug.Log($"savedData : {savedData}");
        if (savedData == 0)
        {
            Debug.LogWarning("�����Ͱ� ����� : RemainingPeriod");
            return savedData;
        }
        else if(savedData < 0)
        {
            Debug.LogAssertion("�ǵ����� ���� ������ ���� : RemainingPeriod");
            return savedData;
        }
        else
        {
            Debug.Log("������ �ε� ���� : LoadRemainingPeriod");
            return savedData;
        }
    }

    public eStage LoadStage(ePlayerSaveKey saveKey)
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        int savedData = LoadData(saveKey.ToString() + defaultSaveKey_Stage, 0);

        Debug.Log($"savedData : {savedData}");
        if (savedData == 0)
        {
            Debug.LogWarning("�����Ͱ� ����� : LoadStage");
            return eStage.Stage1;
        }
        else
        {
            if(Enum.IsDefined(typeof(eStage), savedData))
            {
                Debug.Log("������ �ε� ���� : LoadStage");
                GameManager.Instance.SetStage((eStage)savedData);
                return (eStage)savedData;
            }
            else
            {
                Debug.LogError("������ �ε� ���� : LoadStage");
                return eStage.Defualt;
            }
        }
    }
    public sPlayerStatus LoadPlayerStatus(ePlayerSaveKey saveKey)
    {
        

        string savedData = LoadData(saveKey.ToString() + defaultSaveKey_PlayerStatus, string.Empty);
        

        Debug.Log($"savedData : {savedData}");
        if (string.IsNullOrEmpty(savedData))
        {
            Debug.Log("�����Ͱ� ����� : sPlayerStatus");
            return sPlayerStatus.defaultData;
        }

        sPlayerStatus playerStatus = sPlayerStatus.DataSplit(savedData);

        // �����Ͱ� �߸��Ȱ�� �н�
        if (playerStatus == sPlayerStatus.defaultData)
        {
            Debug.LogAssertion("������ ���� ���� : sPlayerStatus");
        }

        return playerStatus;
    }

    public int LoadOpenedIconCount(ePlayerSaveKey saveKey)
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        int savedData = LoadData(saveKey.ToString() + defaultSaveKey_OpenedIconCount, 0);

        Debug.Log($"savedData : {savedData}");
        if (savedData == 0) Debug.LogWarning("�����Ͱ� ����� : LoadOpenedIconCount");
        else Debug.Log("������ �ε� ���� : LoadOpenedIconCount");

        GameManager.connector_InGame.Canvas1.IconView.SetOpendIconCount(savedData);
        return savedData;
    }

    public void LoadDynamicInteractable(ePlayerSaveKey saveKey)
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        string savedData = LoadData(saveKey.ToString() + defaultSaveKey_DynamicInteractable, string.Empty);

        Debug.Log($"savedData : {savedData}");
        if (savedData == string.Empty) Debug.LogWarning("�����Ͱ� ����� : LoadOpenedIconCount");

        else Debug.Log("������ �ε� ���� : LoadDynamicInteractable");

        DynamicInteractableBase.SetFileNumToLoad(savedData);
    }

    // --------------------
    public void SaveData(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }
    public void SaveData(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }
    public void SaveData(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }
    
    public int LoadData(string key, int defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }
    public float LoadData(string key, float defaultValue)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }
    public string LoadData(string key, string defaultValue)
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }

    public void PlayerDataReset()
    {
        PlayerPrefs.DeleteAll();

        // �ʱ�ȭ�� ���� ȯ�漳��â���� ���̴� �ǳ��� �ʱ�ȭ��Ŵ
        switch (GameManager.Instance.currentScene)
        {
            case eScene.Lobby:
                
                GameManager.connector_Lobby.popUpViewAsLobby.gameSettingPopUp.currentPanel.RefreshPanel();
                break;

            case eScene.InGame:
                GameManager.connector_InGame.popUpViewAsInGame.gameSettingPopUp.currentPanel.RefreshPanel();
                break;
        }
    }
}
