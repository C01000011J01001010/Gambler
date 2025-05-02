using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PublicSet;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices.WindowsRuntime;




public class GameManager : Singleton<GameManager>
{
    // 싱글톤과 논싱글톤의 연결자
    static private Connector _connector;
    static public Connector connector
    {
        get
        {
            if (_connector == null) Debug.LogAssertion("커넥터 연결 안됐음");
            return _connector;
        }
    }

    static public Connector_Lobby connector_Lobby
    {
        get
        {
            if (connector is Connector_Lobby) return (connector as Connector_Lobby);
            else { Debug.LogAssertion("커넥터 캐스팅 실패 : connector_Lobby"); return null; }
        }
    }
    static public Connector_InGame connector_InGame
    {
        get
        {
            if (connector is Connector_InGame) return (connector as Connector_InGame);
            else { Debug.LogAssertion("커넥터 캐스팅 실패 : connector_InGame"); return null; }
        }
    }


    // 에디터에서 수정
    public float defaultGamespeed;
    public int maxStage;

    // 스크립트에서 수정
    public bool isGamePause {  get; private set; }
    public bool isGameOver { get; private set; }
    public bool isCasinoGameView {  get; private set; }
    public float gameSpeed { get; private set; }

    public ePlayerSaveKey _currentPlayerSaveKey;
    public eScene _currentScene;
    public eMap _currentMap;
    public eStage _currentStage;
    [SerializeField] private int _currentRemainingPeriod;

    public ePlayerSaveKey currentPlayerSaveKey
    { get { return _currentPlayerSaveKey; } private set {  _currentPlayerSaveKey = value; } }
    public eScene currentScene
    { get { return _currentScene; } private set { _currentScene = value; } }
    public eMap currentMap
    { get { return _currentMap; } private set { _currentMap = value; } }
    public eStage currentStage
    { get { return _currentStage; } private set { _currentStage = value; } }
    public int currentRemainingPeriod 
    { get { return _currentRemainingPeriod; } private set { _currentRemainingPeriod = value; } }

    public void SetPlayerSaveKey(ePlayerSaveKey value)
    {
        currentPlayerSaveKey = value;
    }
    public void SetCurrentScene(eScene value)
    {
        currentScene = value;
        InitConnector();
        SceneLoadView();
    }
    public void SetCurrentMap(eMap value)
    {
        currentMap = value;
    }
    public void SetCurrentStage(eStage value)
    {
        currentStage = value;
    }
    public void SetRemainingPeriod(int value)
    {
        currentRemainingPeriod = value;
    }
    public void CountDownRemainingPeriod(out bool isGameOver)
    {
        isGameOver = false;

        currentRemainingPeriod--;

        if (currentRemainingPeriod == 0)
        {
            GameOver();
            return;
        }

        QuestManager.InitRefeatableQuest();
    }

    public Dictionary<eStage, string> stageMessageDict;

    private void InitCurrentGame()
    {
        isGameOver = false;
    }
    public void Init_StageMessageDict()
    {
        stageMessageDict = new Dictionary<eStage, string>();
        stageMessageDict.Add(eStage.Stage1, "STAGE 1\n여기가 대체 어디야?");
        stageMessageDict.Add(eStage.Stage2, "STAGE 2\n카지노에 입성하자");
    }

    protected override void Awake()
    {
        base.Awake();
        Init_StageMessageDict();
        Continue_theGame();
    }

    public void Pause_theGame()
    {
        gameSpeed = 0;
        isGamePause = true;
    }
    public void Continue_theGame()
    {
        gameSpeed = defaultGamespeed;
        isGamePause = false;
    }

    public void ChangeCardGameView(bool boolValue)
    {
        isCasinoGameView = boolValue;
    }

    public void InitConnector()
    {
        switch(currentScene)
        {
            case eScene.Title:{
                    _connector = GameObject.FindGameObjectWithTag("Connector").GetComponent<Connector>();
                    if (_connector == null) Debug.LogAssertion($"커넥터 Connector 연결 실패");
                } break;
            case eScene.Lobby: { 
                    _connector = GameObject.FindGameObjectWithTag("Connector").GetComponent<Connector_Lobby>();
                    if (_connector == null) Debug.LogAssertion($"커넥터 Connector_Lobby 연결 실패");
                } break;
            case eScene.InGame: {
                    _connector = GameObject.FindGameObjectWithTag("Connector").GetComponent<Connector_InGame>();
                    if (_connector == null) Debug.LogAssertion($"커넥터 Connector_InGame 연결 실패");
                } break;
        }

        
    }

    

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("테스트 시작");
            CallbackManager.Instance.interactiveTextCallback.EnterCasino();
        }
    }
#endif


    private void OnEnable()
    {
        // 씬이 로드될 때 호출될 함수를 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 게임 종료시 콜백제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void SceneLoadView(Action LoadSceneCallback = null)
    {
        ProcessSceneView(true, Color.black, Color.clear, LoadSceneCallback);
    }

    public void SceneUnloadView(Action LoadSceneCallback)
    {
        ProcessSceneView(false, Color.clear, Color.black, LoadSceneCallback);
    }

    public void ProcessSceneView(bool isSceneLoad, Color startColor, Color targetColor, Action callback = null)
    {
        connector.blackView.SetActive(true);
        Image blackViewImage = connector.blackView.GetComponent<Image>();
        blackViewImage.color = startColor;

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(0.3f)
                .Append(blackViewImage.DOColor(targetColor, 0.7f));

        if(isSceneLoad)
        {
            sequence.AppendCallback(() => _connector.blackView.SetActive(false));
        }
        if (callback != null)
        {
            sequence.AppendCallback(() => callback());
        }

        sequence.SetLoops(1);

        sequence.Play();
    }

    public void SetStage(eStage stageEnum)
    {
        currentStage = stageEnum;
    }

    public void NextStage()
    {
        currentStage++;
    }


    private void StartNewGame()
    {
        // 디데이는 30일부터 시작해서 0이 되면 게임이 종료됨
        SetRemainingPeriod(30);

        // 기본값으로 설정
        PlayManager.Instance.InitPlayerStatus();
        SetStage(eStage.Stage1);
        connector_InGame.Canvas1.IconView.SetOpendIconCount(0);
        ItemManager.HashSetAllClear();
        QuestManager.HashSetAllClear();

        connector_InGame.Map.ChangeMapTo(eMap.InsideOfHouse);
        SetStage(eStage.Stage1);
        SceneLoadView(
            () =>
            {
                PlayManager.Instance.StartPlayerMonologue();
            }
            );
    }

    private void ContinueGame()
    {
        // 남은기간 불러오기
        SetRemainingPeriod(PlayerSaveManager.Instance.LoadRemainingPeriod(currentPlayerSaveKey));

        // 플레이어 정보 불러오기
        sPlayerStatus playerStatus =  PlayerSaveManager.Instance.LoadPlayerStatus(currentPlayerSaveKey);
        PlayManager.Instance.InitPlayerStatus(playerStatus);
        PlayerSaveManager.Instance.LoadStage(currentPlayerSaveKey); // 여기서 데이터 Set함
        PlayerSaveManager.Instance.LoadOpenedIconCount(currentPlayerSaveKey); // 여기서 Set함
        PlayerSaveManager.Instance.LoadItems(currentPlayerSaveKey);
        PlayerSaveManager.Instance.LoadQuests(currentPlayerSaveKey);
        PlayerSaveManager.Instance.LoadDynamicInteractable(currentPlayerSaveKey);




        connector_InGame.Map.ChangeMapTo(eMap.InsideOfHouse);
        SceneLoadView(
            () =>
            {
                PlayManager.Instance.StartPlayerMonologue();
            }
            );
    }

    

    public void GameOver()
    {
        isGameOver = true;
        
        // 이미 검은화면이면 바로 게임 종료
        if (connector.blackView.activeInHierarchy)
        {
            connector_InGame.Canvas2.YouLoseView.gameObject.SetActive(true);
        }
        else
        {
            float delay = 1f;
            CallbackBase.PlaySequnce_BlackViewProcess(
            delay,
            () => connector_InGame.Canvas2.YouLoseView.gameObject.SetActive(true));
        }
    }


    /// <summary>
    /// Awake -> OnEnable(SceneManager.sceneLoaded에 추가) -> 씬로드 -> OnSceneLoaded -> Start
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        { 
            case 0: SetCurrentScene(eScene.Title);  break;
            case 1: 
                {
                    SetCurrentScene(eScene.Lobby);

                    // 퀘스트 정보 초기화
                    foreach (eQuestType type in Enum.GetValues(typeof(eQuestType)))
                    {
                        if (type == eQuestType.None) continue;

                        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(type);

                        questInfo.InitBool();
                    }

                    // 아이템 정보 초기화
                    foreach(eItemType type in Enum.GetValues(typeof(eItemType)))
                    {
                        if(type == eItemType.None) continue;

                        cItemInfo itemInfo = CsvManager.Instance.GetItemInfo(type);

                        itemInfo.InitBool();
                    }
                }
                break;
            case 2:
                {
                    InitCurrentGame();
                    SetCurrentScene(eScene.InGame); 
                    switch (currentPlayerSaveKey)
                    {
                        case ePlayerSaveKey.None: StartNewGame(); break; // 새로시작하기 누른 경우
                        default: ContinueGame(); break; // 이어하기 누른 경우
                    }
                }
                break;
        }
        
    }
}
