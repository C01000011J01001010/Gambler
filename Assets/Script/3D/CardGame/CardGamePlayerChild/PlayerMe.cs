using PublicSet;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMe : CardGamePlayerBase
{
    public bool isCompleteSelect_OnGameSetting {  get; private set; }
    public bool isCompleteSelect_OnPlayTime { get; private set; }
    public bool isAttack {  get; private set; }


    private SelectCompleteButton _selectCompleteButton;
    public SelectCompleteButton selectCompleteButton
    {
        get
        {
            CheckSelectCompleteButton();
            return _selectCompleteButton;
        }
    }
    private void CheckSelectCompleteButton()
    {
        if (_selectCompleteButton == null)
            _selectCompleteButton = CardGamePlayManager.Instance.cardGameView.selectCompleteButton;
    }

    private void Start()
    {
        CheckSelectCompleteButton();
        cCharacterInfo info = CsvManager.Instance.GetCharacterInfo(eCharacterType.Player);
        SetCharacterInfo(info);
    }

    public override void AddCoin(int value)
    {
        coin += value;
        PlayManager.Instance.AddPlayerMoney(value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">양수를 값으로 받음</param>
    /// <returns></returns>
    public override void TryMinusCoin(int value, out bool isBankrupt, out bool hasDebt)
    {
        isBankrupt = false;
        hasDebt = false;

        // 파산 여부 체크
        if (coin <= value)
        {
            isBankrupt = true;

            // 지불할 돈이 부족한지 체크
            if(coin < value)
            {
                hasDebt = true;
            }
        }

        // coin이 (-)마이너스가 된다면 주최측이 빚으로 달고 대신 내준다는 설정
        coin = coin - value;

        PlayManager.Instance.AddPlayerMoney(-value);
    }


    public override void InitAttribute_All()
    {
        base.InitAttribute_All();
        isCompleteSelect_OnGameSetting = false;
    }

    public override void InitAttribute_ForNextOrder()
    {
        base.InitAttribute_ForNextOrder();
        isCompleteSelect_OnPlayTime = false;
    }

    public override bool TryDownCountPerCardType(cTrumpCardInfo cardInfo)
    {
        if (cardCountPerType_GameSetting[cardInfo.cardType] > 1)
        {
            cardCountPerType_GameSetting[cardInfo.cardType]--;

            // 플레이어가 TryDownCountPerCardType를 실행할 시 선택이 완료됐는지를 확인하고 버튼을 활성화함
            selectCompleteButton.CheckCompleteSelect_OnChooseCardsToReveal(cardCountPerType_GameSetting);

            //Debug.Log($"{gameObject.name}에게 {cardInfo.cardName}카드 제거");
            //Debug.Log($"{gameObject.name}의 {cardInfo.cardType.ToString()} 남은 카드 수 :" +
            //    $" {cardCountPerType_GameSetting[cardInfo.cardType]}");

            return true;
        }
        else
        {

            //Debug.Log($"{gameObject.name}의 {cardInfo.cardType.ToString()}의 남은 카드 수 :" +
            //    $" {cardCountPerType_GameSetting[cardInfo.cardType]}");
            //Debug.Log("카드 개수를 줄일 수 없음");

            return false;
        }
    }


    public void Set_isCompleteSelect_OnGameSetting(bool value)
    {
        isCompleteSelect_OnGameSetting = value;
    }
    public virtual void Set_isCompleteSelect_OnPlayTime(bool value)
    {
        isCompleteSelect_OnPlayTime = value;
    }
    


    public override void AttackOtherPlayers(List<CardGamePlayerBase> playerList)
    {
        CardGamePlayManager.Instance.NextProgress(); // 201을 실행

        // 내 카드 보기 활성화
        CardGamePlayManager.Instance.cardGameView.cardScreenButtonSet.PlaySequence_FadeIn();

        // 카드버튼 사용 활성화
        CardGamePlayManager.Instance.cardButtonMemoryPool.SetAllButtonInteractable(true);

        // TargetDisplay에서 대상을 선택할 수 있도록 만듬
        CardGamePlayManager.Instance.cardGameView.targetDisplay.LiftRestrictionToAllSelections();
        //GameAssistantPopUp_OnlyOneLives.Instance.LiftRestrictionToAllSelections();

        // 버튼 클릭시 콜백을 변경
        isAttack = true;
        selectCompleteButton.SetButtonCallback(1);
        // 상대를 지목하고 카드 선택을 완료하면 202를 실행해야함
    }

    

    public override void DeffenceFromOtherPlayers(CardGamePlayerBase AttackerScript)
    {
        CardGamePlayManager.Instance.NextProgress(); // 301을 실행

        // 내 카드 보기 활성화
        CardGamePlayManager.Instance.cardGameView.cardScreenButtonSet.PlaySequence_FadeIn();

        // 카드버튼 사용 활성화
        CardGamePlayManager.Instance.cardButtonMemoryPool.SetAllButtonInteractable(true);

        // 버튼 클릭시 콜백을 변경
        isAttack = false;
        selectCompleteButton.SetButtonCallback(1);
        // 상대를 지목하고 카드 선택을 완료하면 302를 실행해야함
    }

    
}
