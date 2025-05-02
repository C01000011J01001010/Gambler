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
    // �̱���� ��̱����� ������
    static private Connector _connector;
    static public Connector connector
    {
        get
        {
            if (_connector == null) Debug.LogAssertion("Ŀ���� ���� �ȵ���");
            return _connector;
        }
    }

    static public Connector_Lobby connector_Lobby
    {
        get
        {
            if (connector is Connector_Lobby) return (connector as Connector_Lobby);
            else { Debug.LogAssertion("Ŀ���� ĳ���� ���� : connector_Lobby"); return null; }
        }
    }
    static public Connector_InGame connector_InGame
    {
        get
        {
            if (connector is Connector_InGame) return (connector as Connector_InGame);
            else { Debug.LogAssertion("Ŀ���� ĳ���� ���� : connector_InGame"); return null; }
        }
    }


    // �����Ϳ��� ����
    public float defaultGamespeed;
    public int maxStage;

    // ��ũ��Ʈ���� ����
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
        stageMessageDict.Add(eStage.Stage1, "STAGE 1\n���Ⱑ ��ü ����?");
        stageMessageDict.Add(eStage.Stage2, "STAGE 2\nī���뿡 �Լ�����");
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
                    if (_connector == null) Debug.LogAssertion($"Ŀ���� Connector ���� ����");
                } break;
            case eScene.Lobby: { 
                    _connector = GameObject.FindGameObjectWithTag("Connector").GetComponent<Connector_Lobby>();
                    if (_connector == null) Debug.LogAssertion($"Ŀ���� Connector_Lobby ���� ����");
                } break;
            case eScene.InGame: {
                    _connector = GameObject.FindGameObjectWithTag("Connector").GetComponent<Connector_InGame>();
                    if (_connector == null) Debug.LogAssertion($"Ŀ���� Connector_InGame ���� ����");
                } break;
        }

        
    }

    

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("�׽�Ʈ ����");
            CallbackManager.Instance.interactiveTextCallback.EnterCasino();
        }
    }
#endif


    private void OnEnable()
    {
        // ���� �ε�� �� ȣ��� �Լ��� �߰�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // ���� ����� �ݹ�����
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
        // ���̴� 30�Ϻ��� �����ؼ� 0�� �Ǹ� ������ �����
        SetRemainingPeriod(30);

        // �⺻������ ����
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
        // �����Ⱓ �ҷ�����
        SetRemainingPeriod(PlayerSaveManager.Instance.LoadRemainingPeriod(currentPlayerSaveKey));

        // �÷��̾� ���� �ҷ�����
        sPlayerStatus playerStatus =  PlayerSaveManager.Instance.LoadPlayerStatus(currentPlayerSaveKey);
        PlayManager.Instance.InitPlayerStatus(playerStatus);
        PlayerSaveManager.Instance.LoadStage(currentPlayerSaveKey); // ���⼭ ������ Set��
        PlayerSaveManager.Instance.LoadOpenedIconCount(currentPlayerSaveKey); // ���⼭ Set��
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
        
        // �̹� ����ȭ���̸� �ٷ� ���� ����
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
    /// Awake -> OnEnable(SceneManager.sceneLoaded�� �߰�) -> ���ε� -> OnSceneLoaded -> Start
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

                    // ����Ʈ ���� �ʱ�ȭ
                    foreach (eQuestType type in Enum.GetValues(typeof(eQuestType)))
                    {
                        if (type == eQuestType.None) continue;

                        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(type);

                        questInfo.InitBool();
                    }

                    // ������ ���� �ʱ�ȭ
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
                        case ePlayerSaveKey.None: StartNewGame(); break; // ���ν����ϱ� ���� ���
                        default: ContinueGame(); break; // �̾��ϱ� ���� ���
                    }
                }
                break;
        }
        
    }
}
