using UnityEngine;
using System.Collections.Generic;
using PublicSet;
using DG.Tweening;
using System.Linq;



// 싱글톤이지만 부모객체가 존재하여 씬 이동시 파괴됨
public class CardGamePlayManager : Singleton<CardGamePlayManager>
{
    // 에디터 연결
    public Transform playerParent;
    public CardGameView cardGameView;
    

    public PlayerMe playerMe;
    public CardManager cardManager;
    public DiceManager diceManager;
    public DeckOfCards deckOfCards;
    
    public PopUpView_InGame popUpView;
    public MainCameraAnimation mainCamAnime;
    public GameAssistantPopUp_OnlyOneLives gameAssistantPopUp;

    // 스크립트 편집
    public eOOLProgress currentProgress { get; private set; }
    public bool isDistributionCompleted { get; private set; }
    public List<CardGamePlayerBase> playerList {  get; private set; }
    public Queue<CardGamePlayerBase> OrderedPlayerQueue { get; private set; }
    public eCriteria currentCriteria { get; private set; }

    //
    public int layerOfMe { get; private set; }
    public int coinMultiple { get; private set; }  // 코인 배수


    //
    public CardGamePlayerBase Attacker { get; private set; }
    public CardGamePlayerBase Deffender { get; private set; }
    public CardGamePlayerBase Joker { get; private set; }
    public CardGamePlayerBase Victim { get; private set; }
    public CardGamePlayerBase Prey {  get; private set; }
    public int ExpressionValue { get; private set; }



    

    // 캐싱
    private CardButtonMemoryPool _cardButtonMemoryPool;
    public CardButtonMemoryPool cardButtonMemoryPool
    {
        get
        {
            CheckCardButtonMemoryPool();
            return _cardButtonMemoryPool;
        }
    }
    public void CheckCardButtonMemoryPool()
    {
        if (_cardButtonMemoryPool == null)
            _cardButtonMemoryPool = GameManager.connector_InGame.Canvas0.CardGameView.cardScreen.cardButtonMemoryPool;
    }

    protected override void Awake()
    {
        base.Awake();
        layerOfMe = LayerMask.NameToLayer("Me");
    }

    private void Start()
    {
        CheckCardButtonMemoryPool();
    }

    public void SetAttacker(CardGamePlayerBase value)
    {
        Attacker = value;
    }

    public void SetDeffender(CardGamePlayerBase value)
    {
        Deffender = value;
    }

    public void SetJoker(CardGamePlayerBase value)
    {
        Joker = value;
    }

    public void SetVictim(CardGamePlayerBase value)
    {
        Victim = value;
    }

    /// <summary>
    /// 새 게임이 시작될때 먹잇감은 초기화됨
    /// </summary>
    public void ClearPrey()
    {
        Prey = null;
    }

    /// <summary>
    /// 한차례의 공방이 끝날때마다 먹잇감이 있나 탐색함
    /// </summary>
    public void TrySetPrey()
    {
        foreach(CardGamePlayerBase player in playerList)
        {
            if(player.closedCardList.Count == 0)
            {
                if (Prey == null)
                {
                    Prey = player; // 먹잇감이 없으면 해당 플레이어를 설정
                }
                //else if (Prey != player && player.CompareTag("Player")) Prey = player; // 주인공이 우선해서 먹잇감이 됨
            }
        }
    }

    

    public void InitPlayerList()
    {
        if (playerList == null) playerList = new List<CardGamePlayerBase>();
        else playerList.Clear();

        if (OrderedPlayerQueue == null) OrderedPlayerQueue = new Queue<CardGamePlayerBase>();
        else OrderedPlayerQueue.Clear();

        for (int i = 0; i < playerParent.childCount; i++)
        {
            CardGamePlayerBase player = playerParent.GetChild(i).GetComponent<CardGamePlayerBase>();
            playerList.Add(player);
        }
    }

    public void EnterCardGame()
    {
        InitPlayerList();

        for (int i = 0; i < playerList.Count; i++)
        {
            if(playerList[i].CompareTag("Player"))
            {
                //Debug.Log("플레이어의 코인을 연동");
                playerList[i].SetCoin(PlayManager.Instance.currentPlayerStatus.coin);
            }
            else
            {
                //Debug.Log($"컴퓨터{gameObject.name}한테 랜덤한 코인을 증정");
                playerList[i].SetCoin();
            }
        }

        // 스타트버튼 텍스트 초기화 후 화면에서 숨기기
        cardGameView.InitStartButtonText();
        cardGameView.InitStartButton();

        // 인터페이스 초기화
        cardGameView.diceSet.Init();
        cardGameView.cardScreenButtonSet.Init();

        // 게임 어시스턴트 초기화
        popUpView.gameAssistantPopUp_OnlyOneLives.RefreshPopUp();

        // 게임어시스턴트를 사용할 수 있도록 시도
        GameManager.connector_InGame.Canvas1.IconView.TryIconUnLock(eIcon.GameAssistant);

        // TargetDisplay의 선택기능은 필요한 순간까지 제한
        cardGameView.targetDisplay.InitAttribute();
        cardGameView.targetDisplay.PlaceRestrictionToAllSelections();
        //gameAssistantPopUp.PlaceRestrictionToAllSelections();

        // 게임 진행도 초기화
        SetProgress_EnterGame();


        // 퀘스트 수주한 경우
        if (QuestManager.Instance.PlayerQuestDict.ContainsKey(eQuestType.StartFirstGame))
        {
            cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(eQuestType.StartFirstGame);
            if (questInfo.isComplete == false) questInfo.checkEndCondition();
        }
            
    }
    public void InitCurrentGame()
    {
        foreach (CardGamePlayerBase player in playerList)
        {
            player.InitAttribute_All();
        }
        cardGameView.InitAttribute();

        isDistributionCompleted = false;

        ClearPrey();
    }

    /// <summary>
    /// cardGameView에 처음 진입할때만 실행
    /// </summary>
    public void SetProgress_EnterGame()
    {
        currentProgress = eOOLProgress.num101_BeforeStartGame;
        GameManager.connector_InGame.Canvas1.TextWindowView.StartTextWindow(currentProgress);
    }

    public void SetProgress(eOOLProgress progress)
    {
        Debug.Log($"현재 진행상황 : {currentProgress.ToString()}");
        currentProgress = progress;

        Debug.Log($"다음 진행상황 : {currentProgress.ToString()}");
        GameManager.connector_InGame.Canvas1.TextWindowView.StartTextWindow(currentProgress);
    }
    /// <summary>
    /// 모든 진행사항은 직접 제어하지 않고 해당 함수로 제어함
    /// </summary>
    public void NextProgress()
    {
        Debug.Log($"현재 진행상황 : {currentProgress.ToString()}");

        if (currentProgress == eOOLProgress.num104_OnChooseFirstPlayer || 
            currentProgress == eOOLProgress.num408_OnChooseNextPlayer)
        {
            if (Attacker != null)
            {
                // 카드가 1장만 존재하였고 방어할 때 조커를 사용했으면 공격에 쓸 카드가 없음
                // 선택할 카드가 없는 경우
                if (Attacker.closedCardList.Count <= 0)
                {
                    currentProgress = eOOLProgress.num203_PlayerCantAttack;
                }
                // 플레이어의 경우
                else if (Attacker.CompareTag("Player"))
                {
                    currentProgress = eOOLProgress.num201_AttackTurnPlayer;
                }
                // 컴퓨터의 경우
                else
                {
                    currentProgress = eOOLProgress.num202_AttackDone;
                }
            }
            else
            {
                Debug.LogAssertion("이번 진행의 공격자가 설정되지 않았음");
            }
            
        }
        else if (currentProgress == eOOLProgress.num202_AttackDone)
        {
            if(Deffender != null)
            {
                // 카드가 1장만 존재하였고 조커를 사용했거나 공격에 실패한경우 방어할 카드가 없음
                // 선택할 카드가 없는 경우
                if (Deffender.closedCardList.Count <= 0)
                {
                    currentProgress = eOOLProgress.num303_PlayerCantDefense;
                }
                else if (Deffender.CompareTag("Player"))
                {
                    currentProgress = eOOLProgress.num301_DefenseTrun_Player;
                }
                else
                {
                    currentProgress = eOOLProgress.num302_DefenseDone;
                }
            }
            else
            {
                Debug.LogAssertion("이번 진행의 방어자가 설정되지 않았음");
            }
        }
        else if(currentProgress == eOOLProgress.num302_DefenseDone ||
                currentProgress == eOOLProgress.num303_PlayerCantDefense)
        {
            currentProgress = eOOLProgress.num401_CardOpenAtTheSameTime;
        }

        else if(currentProgress == eOOLProgress.num401_CardOpenAtTheSameTime)
        {
            switch(currentCriteria)
            {
                case eCriteria.JokerWin: currentProgress = eOOLProgress.num402_OnJokerAppear; break;
                case eCriteria.AttakkerWin: currentProgress = eOOLProgress.num403_OnAttackSuccess; break;
                case eCriteria.DeffenderWin: currentProgress = eOOLProgress.num404_OnDefenceSuccess; break;
                case eCriteria.HuntingTime: currentProgress = eOOLProgress.num405_OnHuntPrey; break;
                default: Debug.LogAssertion("설정되지 않은 값이 들어갔음"); break;
            }
        }
        else if(currentProgress == eOOLProgress.num203_PlayerCantAttack ||
            currentProgress == eOOLProgress.num402_OnJokerAppear ||
            currentProgress == eOOLProgress.num403_OnAttackSuccess ||
            currentProgress == eOOLProgress.num404_OnDefenceSuccess ||
            currentProgress == eOOLProgress.num405_OnHuntPrey||
            currentProgress == eOOLProgress.num406_OnPlayerBankrupt_GetOut ||
            currentProgress == eOOLProgress.num407_OnPlayerBankrupt_GoToMining) // 주인공의 경우 다음으로 넘어가지 않고 게임이 종료됨
        {
            // 다음차례가 있으면 큐에서 그 선수의 명단을 꺼낸 후 다음차례를 진행
            if(OrderedPlayerQueue.Count > 0)
            {
                Debug.Log($"남은 플레이어 순서 : {OrderedPlayerQueue.Count}");

                TrySetPrey(); // 먹잇감이 있나 확인

                foreach (var player in playerList)
                {
                    player.InitAttribute_ForNextOrder();
                }

                Attacker = OrderedPlayerQueue.Dequeue();
                currentProgress = eOOLProgress.num408_OnChooseNextPlayer;
            }
            // 그렇지 않으면 게임을 종료
            else
            {
                // 플레이어가 둘 이상인 경우 게임을 지속
                if (playerList.Count >= 2)
                    currentProgress = eOOLProgress.num501_final;

                // 플레이어가 1명만 남은경우
                else if (playerList.Count == 1) 
                    currentProgress = eOOLProgress.num502_EndGame;
                else
                {
                    Debug.LogError("예기치 않은 처리");
                    return;
                }
                    
            }
        }
        else if(currentProgress == eOOLProgress.num501_final) // 게임이 끝나고 다음 게임을 시작한 경우
        {
            currentProgress = eOOLProgress.num102_BeforeRotateDiceAndDistribution;
        }
        else // 1씩 증가하는 경우 ++을 사용
        {
            currentProgress++;
        }


        Debug.Log($"다음 진행상황 : {currentProgress.ToString()}");
        GameManager.connector_InGame.Canvas1.TextWindowView.StartTextWindow(currentProgress);

        
    }
    public void InitOrderedPlayerQueue()
    {
        // 첫번째 순서 찾기
        int firstPlayerIndex = 0;
        
        for (int i = 1; i<playerList.Count; i++ )
        {
            if (playerList[firstPlayerIndex].myDiceValue > playerList[i].myDiceValue)
            {
                firstPlayerIndex = i;
            }
        }
        Debug.Log($"{playerList[firstPlayerIndex].gameObject.name}의 주사위값은 " +
            $"{playerList[firstPlayerIndex].myDiceValue}으로 제일 작습니다. 첫번째로 공격을 시작합니다.");

        // 첫번째 플레이어부터 반시계방향으로 큐에 넣기
        OrderedPlayerQueue.Clear();
        for (int i = 0; i < playerList.Count; i++)
        {
            int finalIndex = (firstPlayerIndex + i) % playerList.Count;
            OrderedPlayerQueue.Enqueue(playerList[finalIndex]);
        }

        SetAttacker(OrderedPlayerQueue.Dequeue());
        //return OrderedPlayerQueue;
    }


    public void StartPlayerAttack()
    {
        Attacker.AttackOtherPlayers(playerList);

    }


    public void StartPlayerDeffence()
    {
        Deffender.DeffenceFromOtherPlayers(Attacker);
    }


    // 게임 진행

    public void GameSetting()
    {
        if(playerList == null)
        {
            Debug.LogAssertion("players == null");
            return;
        }

        // 반복문이지만 주사위를 한번 굴린 후 종료됨, 모든 처리가 끝난 후 다시 호출되어야함
        for (int i = 0; i < playerList.Count; i++)
        {
            // 이미 주사위를 돌렸으면 패스(직접 주사위를 돌린 Player(Me)는 자동으로 패스됨)
            if (playerList[i].diceDone == true)
            {
                //Debug.Log($"{player[i].gameObject.name}은 이미 주사위를 돌렸음");
                continue;
            }
            Debug.Log($"주사위를 굴리지 않은 플레이어 숫자 == {4 - i}");
            Debug.Log($"{playerList[i].gameObject.name}의 주사위가 굴러갑니다");
            diceManager.RotateDice(playerList[i].gameObject);
            return; // 주사위를 한번 돌렸으면 함수를 종료
        }
        // 주사위를 다 돌렸는데 이 함수에 진입했다면 다음 처리를 진행해줌

        // 주사위값이 가장 큰 플레이어부터 시계순으로 큐를 초기화
        InitOrderedPlayerQueue();

        Sequence sequence = DOTween.Sequence();

        // Interface 변경
        Sequence appendSequence = DOTween.Sequence();
        cardGameView.diceSet.GetSequnce_ChangeInterfaceNext(appendSequence);
        sequence.Append(appendSequence);

        // 필요없어진 카드덱을 제거
        sequence.JoinCallback(deckOfCards.StartDisappearEffect);

        sequence.AppendCallback(NextProgress);

        // 컴퓨터가 자동으로 카드를 고르도록 만듬
        foreach (CardGamePlayerBase player in playerList)
        {
            // 플레이어는 카드를 직접 고르니 다음으로 넘어감
            if(player.CompareTag("Player"))
            {
                continue;
            }

            PlayerEtc computer =  player.gameObject.GetComponent<PlayerEtc>();

            if (computer != null)
            {
                computer.SelectCard_OnStartTime();
            }

        }

        // 플레이어(me)가 카드 선택을 완료하는 버튼을 활성화
        cardButtonMemoryPool.SetAllButtonInteractable(true);
        isDistributionCompleted = true;
        cardGameView.selectCompleteButton.TryActivate_Button();
    }

    public void  CardOpenAtTheSameTime()
    {
        // 각 카드를 동시에 오픈
        Sequence sequence = DOTween.Sequence();

        // 카메라를 뒤집기 전 화면을 확대
        mainCamAnime.GetSequnce_CameraZoomIn(sequence);

        if (Attacker != null && Attacker.PresentedCardScript != null)
        {
            Sequence appendSequence = DOTween.Sequence();
            Attacker.PresentedCardScript.GetSequnce_TryCardOpen(appendSequence, Attacker);
            sequence.Append(appendSequence);
        }
        else Debug.LogWarning("공격자가 카드를 선택하지 못함");

        // 먹잇감인 플레이어는 카드를 선택하지 못함
        if (Deffender != null && Deffender.PresentedCardScript != null)
        {
            Sequence joinSequnce = DOTween.Sequence();
            Deffender.PresentedCardScript.GetSequnce_TryCardOpen(joinSequnce, Deffender);
            sequence.Join(joinSequnce);
        }
        else Debug.Log("방어자가 카드를 선택하지 못함");

        // 카드를 자세히 확인하기 위한 시간
        float delay = 1.5f;
        sequence.AppendInterval(delay);

        // 카드 오픈 후 결과 확인
        sequence.AppendCallback(DetermineTheResult);

        sequence.SetLoops(1);
        sequence.Play();
    }
    
    public void DetermineTheResult()
    {
        // 수비에 사용할 카드가 없는 경우
        if(Prey != null && Deffender != null
            && Prey == Deffender)
        {
            currentCriteria = eCriteria.HuntingTime;
        }

        // 조커가 있는 경우
        else if (Attacker.PresentedCardScript.trumpCardInfo.cardType == eCardType.Joker)
        {
            currentCriteria = eCriteria.JokerWin;
            SetJoker(Attacker);
            SetVictim(Deffender);
        }
        else if (Deffender.PresentedCardScript.trumpCardInfo.cardType == eCardType.Joker)
        {
            currentCriteria = eCriteria.JokerWin;
            SetJoker(Deffender);
            SetVictim(Attacker);
        }
        // 공격성공 또는 수비성공 여부 판별
        // 공격 성공
        else if (Attacker.PresentedCardScript.trumpCardInfo.cardType ==
            Deffender.PresentedCardScript.trumpCardInfo.cardType ||// 카드의 문양이 같은 경우
            Attacker.PresentedCardScript.trumpCardInfo.cardValue ==
            Deffender.PresentedCardScript.trumpCardInfo.cardValue) // 카드의 값이 같은 경우
        {
            currentCriteria = eCriteria.AttakkerWin;
        }
        else // 수비 성공
        {
            currentCriteria = eCriteria.DeffenderWin;
        }

        // 결과에 따른 수식 계산, 정산 이전에 Text로 알려야하기에 미리 계산해야함
        ResultExpression();

        // 402, 403, 404 중 하나 실행
        NextProgress();
    }

    public void ResultExpression()
    {
        int resultValue = 0;

        switch (currentCriteria)
        {
            case eCriteria.JokerWin:
                {
                    resultValue = Victim.PresentedCardScript.trumpCardInfo.cardValue;
                }
                break;

            case eCriteria.HuntingTime:
                {
                    resultValue = Attacker.PresentedCardScript.trumpCardInfo.cardValue;
                } break;

            case eCriteria.AttakkerWin:
                {
                    resultValue = Attacker.PresentedCardScript.trumpCardInfo.cardValue -
                                    Deffender.PresentedCardScript.trumpCardInfo.cardValue;
                    if (resultValue == 0)
                    {
                        resultValue = Attacker.PresentedCardScript.trumpCardInfo.cardValue;
                    }
                    resultValue = Mathf.Abs(resultValue);
                } break;
            //case eCriteria.DeffenderWin: break;
            default: break; // 필요 없으나 만일을 대비
        }


        // 파산한 플레이어가 늘어날 수록 코인배수 증가(1배, 2배, 4배)
        {
            coinMultiple = 1;
            int bankruptPlayerNum = 4 - playerList.Count;
            if (bankruptPlayerNum > 0)
            {
                coinMultiple = coinMultiple << bankruptPlayerNum; // 2의 bankruptPlayerNum승,
            }
        }

        // 남은 플레이어 숫자의 제곱에 비례하여 이동하는 코인이 결정됨(1배, 4배, 9배로 증가)
        {
            //coinMultiple = 5 - playerList.Count;
            //coinMultiple *= coinMultiple;
        }

        int defaultMultiple = 10; //게임의 난이도에 따라 변경 할 수 있도록 만들까?
        ExpressionValue = resultValue * coinMultiple * defaultMultiple;

        if (Prey != null) // 먹잇감이 있으면 2배 이벤트
        {
            ExpressionValue *= 2;
        }
    }

    /// <summary>
    /// 배당금 지불 상황에 따라 다음 순서를 정함
    /// </summary>
    /// <param name="sequence"></param>
    /// <param name="isBankrupt"></param>
    /// <param name="hasDebt"></param>
    /// <param name="doomedVictim"></param>
    private void GetSequnce_DetermineNextProgress(Sequence sequence, bool isBankrupt, bool hasDebt, CardGamePlayerBase doomedVictim)
    {
        if (isBankrupt)
        {
            // 텍스트에 띄우기 위해 파산한 사람은 피해자로 설정
            if(doomedVictim != Victim)
                SetVictim(doomedVictim);

            if(hasDebt)
            {
                sequence.AppendCallback(() => SetProgress(eOOLProgress.num407_OnPlayerBankrupt_GoToMining));
            }
            else
            {
                sequence.AppendCallback(() => SetProgress(eOOLProgress.num406_OnPlayerBankrupt_GetOut));
            }
        }
        else
        {
            sequence.AppendCallback(NextProgress); // 407 또는 501로 이동
        }
    }

    public void OnJokerAppear()
    {
        Victim.TryMinusCoin(ExpressionValue, out bool isBankrupt, out bool hasDebt);
        Joker.AddCoin(ExpressionValue);
        

        Sequence sequence = DOTween.Sequence();

        // 화면 줌 아웃
        mainCamAnime.GetSequnce_CameraZoomOut(sequence);

        // 카드 정리 , 주인에게 돌아갈 카드만 언셀렉함
        Victim.PresentedCardScript.UnselectThisCard_OnPlayTime(Victim);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Joker, true);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Victim, true);

        GetSequnce_DetermineNextProgress(sequence, isBankrupt, hasDebt, Victim);



        sequence.SetLoops(0);
        sequence.Play();
    }

    public void OnAttackSuccess()
    {
        Deffender.TryMinusCoin(ExpressionValue, out bool isBankrupt, out bool hasDebt);
        Attacker.AddCoin(ExpressionValue);

        Sequence sequence = DOTween.Sequence();

        // 화면 줌 아웃
        mainCamAnime.GetSequnce_CameraZoomOut(sequence);

        // 카드 정리 , 주인에게 돌아갈 카드만 언셀렉함
        Attacker.PresentedCardScript.UnselectThisCard_OnPlayTime(Attacker);
        Deffender.PresentedCardScript.UnselectThisCard_OnPlayTime(Deffender);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Attacker, true);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Deffender, true);

        GetSequnce_DetermineNextProgress(sequence, isBankrupt, hasDebt, Deffender);


        sequence.SetLoops(0);
        sequence.Play();
    }

    public void OnDefenceSuccess()
    {
        Sequence sequence = DOTween.Sequence();

        // 화면 줌 아웃
        mainCamAnime.GetSequnce_CameraZoomOut(sequence);

        // 카드 정리 , 주인에게 돌아갈 카드 언셀렉
        Deffender.PresentedCardScript.UnselectThisCard_OnPlayTime(Deffender);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Attacker, true);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Deffender, true);

        sequence.AppendCallback(NextProgress); // 406 또는 501로 이동

        sequence.SetLoops(0);
        sequence.Play();
    }

    public void OnHuntPrey()
    {
        Prey.TryMinusCoin(ExpressionValue, out bool isBankrupt, out bool hasDebt);
        Attacker.AddCoin(ExpressionValue);

        Sequence sequence = DOTween.Sequence();

        // 화면 줌 아웃
        mainCamAnime.GetSequnce_CameraZoomOut(sequence);

        // 카드 정리 , 주인에게 돌아갈 카드만 언셀렉함
        Attacker.PresentedCardScript.UnselectThisCard_OnPlayTime(Attacker);
        CardGameAnimationManager.Instance.GetSequnce_OrganizeCardAnimaition(sequence, Attacker, true);

        GetSequnce_DetermineNextProgress(sequence, isBankrupt, hasDebt, Prey);

        sequence.SetLoops(0);
        sequence.Play();
    }

    public void OnPlayerBankrupt()
    {
        Debug.Log($"플레이어{Victim.characterInfo.CharacterName} 파산");

        if (Victim.CompareTag("Player")) 
        {
            GameManager.Instance.GameOver();
            return;
        }// 파산한게 주인공이면 뒤의 연산은 필요없음

        // 파산한 플레이어가 prey설정된경우 prey 클리어
        if(Victim == Prey) ClearPrey();

        // 리스트에서 제거하여 정산값을 조정 및 대상에서 제외
        playerList.Remove(Victim);

        // Queue에서 해당 플레이어의 순서를 제거
        { 
            Queue<CardGamePlayerBase> tempQueue = new Queue<CardGamePlayerBase>(OrderedPlayerQueue);
            OrderedPlayerQueue.Clear();
            while (tempQueue.Count > 0)
            {
                CardGamePlayerBase player = tempQueue.Dequeue();
                if(player == Victim)
                {
                    continue;
                }
                else
                {
                    OrderedPlayerQueue.Enqueue(player);
                }
            }
        }
        PlayerEtc bankruptTarget = Victim as PlayerEtc;

        // 게임어시스턴트에서 해당 플레이어 삭제, PlayerMe는 "if (Victim.CompareTag("Player"))" 에서 걸러졌음
        GameAssistantPopUp_OnlyOneLives.Instance.ReturnObject(bankruptTarget.AsisstantPanel.gameObject);

        // 게임 타겟에서 해당 플레이어 삭제
        cardGameView.targetDisplay.TargetBankrupt(bankruptTarget);

        NextProgress();
    }
}
