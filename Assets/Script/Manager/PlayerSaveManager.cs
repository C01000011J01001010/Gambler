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
/// 아이템 저장 자료구조
/// </summary>
public class cPlayerItem : IEntryInfo<eItemType>
{
    /// <summary>
    /// 내가 소유하고 있는 아이템 번호, 0부터 시작
    /// </summary>
    public int id {  get; set; }

    /// <summary>
    /// 아이템 모델명
    /// </summary>
    public eItemType type { get; set; }

    /// <summary>
    /// 아이템 개수
    /// </summary>
    private int _quantity;

    public int quantity
    {
        get => _quantity;
        set => _quantity = value > 0 ? value : 1;
    }

    // 데이터 저장을 위해 string으로 변환
    public override string ToString()
    {
        // 추가 정보도 같이 저장
        cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(type);
        return $"{id}:{type}:{quantity}:{itemInfo.isNeedCheck.ToString()}";
    }

    // string으로 저장했던 정보를 사용가능한 데이터로 변환
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
            Debug.LogWarning("DataSplit 실패");
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
        // 패턴매칭에 의한 조건부 비교
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
/// 퀘스트 저장 자료구조
/// </summary>
public class cPlayerQuest : IEntryInfo<eQuestType> //: IEquatable<sQuest>
{
    public int id { get; set; } // 퀘스트 순서
    public eQuestType type { get; set; } // 퀘스트 번호

    // 데이터 저장을 위해 string으로 변환
    public override string ToString()
    {
        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(type);

        // 참조변수의 일부 데이터를 같이 저장
        return $"{id}:{type}:{questInfo.isNeedCheck.ToString()}:{questInfo.isComplete.ToString()}:{questInfo.hasReceivedReward.ToString()}";
    }

    // string으로 저장했던 정보를 사용가능한 데이터로 변환
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
            // 데이터를 불러오면서 참조변수의 데이터도 같이 복원
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
        // 패턴매칭에 의한 조건부 비교
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
/// 플레이어 데이터 자료구조
/// </summary>
public struct sPlayerStatus
{
    public int hp; // 체력
    public int agility; // 민첩성
    public int hunger; // 허기
    public int coin; // 소지금



    public override string ToString()
    {
        return $"{hp}:{agility}:{hunger}:{coin}";
    }

    // string으로 저장했던 정보를 사용가능한 데이터로 변환
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

        // 값타입이 반환형인경우 각 속성이 기본값인 해당 값타입을 반환
        // int의 기본값은 0
        // float의 기본값은 0.0f
        // bool의 기본값은 false
        // 참조타입이 반환형인경우 기본값으로 null을 반환
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

    // == 연산자 재정의
    public static bool operator ==(sPlayerStatus left, sPlayerStatus right)
    {
        // 객체가 동일한지 체크
        return left.hp == right.hp &&
               left.agility == right.agility &&
               left.hunger == right.hunger &&
               left.coin == right.coin;
    }

    // != 연산자 재정의
    public static bool operator !=(sPlayerStatus left, sPlayerStatus right)
    {
        return !(left == right);
    }

    // 생성자
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

    public const string defaultSaveKey_SavedDate = "SavedDate"; // 데이터가 저장된 날짜와 시간을 저장
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

        // 저장되는 날짜
        string SavedDate = DateTime.Now.ToString("yyyy년 MM월 dd일:HH시 mm분 ss초");

        // 결과 출력
        Debug.Log($"저장 시간 : {SavedDate}");

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
    ///// 게임 종료시 더미데이터는 모두 삭제
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
            Debug.LogWarning("저장기록이 없음");
            return string.Empty;
        }
        else
        {
            Debug.Log("저장기록이 있음");
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
            Debug.LogWarning("저장된 데이터가 없음 : LoadItems");
            return new Dictionary<eItemType, cPlayerItem>();
        }

        // id1:serail1 , id2:serail2 ....
        string[] itemStrings = savedData.Split(',');

        foreach (string itemString in itemStrings)
        {
            // id : serail
            cPlayerItem item = cPlayerItem.DataSplit(itemString);

            // 데이터가 잘못된경우 패스
            if (item.id == 0 && item.type == 0 && item.quantity == 0)
            {
                continue;
            }

            // 올바른 데이터를 딕셔너리에 추가
            else
            {
                ItemManager.Instance.PlayerItemDict.Add(item.type, item);
            }
        }

        Debug.Log("데이터 로딩 성공 : LoadItems");
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

            // 데이터가 잘못된경우 패스
            if (quest.id == 0 && quest.type == 0)
            {
                continue;
            }

            // 올바른 데이터를 딕셔너리에 추가
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
            Debug.LogWarning("데이터가 비었음 : RemainingPeriod");
            return savedData;
        }
        else if(savedData < 0)
        {
            Debug.LogAssertion("의도하지 않은 데이터 저장 : RemainingPeriod");
            return savedData;
        }
        else
        {
            Debug.Log("데이터 로딩 성공 : LoadRemainingPeriod");
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
            Debug.LogWarning("데이터가 비었음 : LoadStage");
            return eStage.Stage1;
        }
        else
        {
            if(Enum.IsDefined(typeof(eStage), savedData))
            {
                Debug.Log("데이터 로딩 성공 : LoadStage");
                GameManager.Instance.SetStage((eStage)savedData);
                return (eStage)savedData;
            }
            else
            {
                Debug.LogError("데이터 로딩 실패 : LoadStage");
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
            Debug.Log("데이터가 비었음 : sPlayerStatus");
            return sPlayerStatus.defaultData;
        }

        sPlayerStatus playerStatus = sPlayerStatus.DataSplit(savedData);

        // 데이터가 잘못된경우 패스
        if (playerStatus == sPlayerStatus.defaultData)
        {
            Debug.LogAssertion("데이터 저장 오류 : sPlayerStatus");
        }

        return playerStatus;
    }

    public int LoadOpenedIconCount(ePlayerSaveKey saveKey)
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        int savedData = LoadData(saveKey.ToString() + defaultSaveKey_OpenedIconCount, 0);

        Debug.Log($"savedData : {savedData}");
        if (savedData == 0) Debug.LogWarning("데이터가 비었음 : LoadOpenedIconCount");
        else Debug.Log("데이터 로딩 성공 : LoadOpenedIconCount");

        GameManager.connector_InGame.Canvas1.IconView.SetOpendIconCount(savedData);
        return savedData;
    }

    public void LoadDynamicInteractable(ePlayerSaveKey saveKey)
    {
        //string savedData = PlayerPrefs.GetString(savedItemsKey, string.Empty);
        string savedData = LoadData(saveKey.ToString() + defaultSaveKey_DynamicInteractable, string.Empty);

        Debug.Log($"savedData : {savedData}");
        if (savedData == string.Empty) Debug.LogWarning("데이터가 비었음 : LoadOpenedIconCount");

        else Debug.Log("데이터 로딩 성공 : LoadDynamicInteractable");

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

        // 초기화시 현재 환경설정창에서 보이는 판넬을 초기화시킴
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
