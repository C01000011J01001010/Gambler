
using UnityEngine;
using System.Collections.Generic;
using PublicSet;

public class PlayerEtc : CardGamePlayerBase
{
    public OnlyOneLivesPlayerPanel AsisstantPanel {  get; private set; }
    public List<CardGamePlayerBase> PlayerList { get { return CardGamePlayManager.Instance.playerList; } }
    public CardGamePlayerBase Prey { get { return CardGamePlayManager.Instance.Prey; } }



    public void SetAsisstantPanel(OnlyOneLivesPlayerPanel value)
    {
        AsisstantPanel = value;
    }
    public override void AddCoin(int value)
    {
        coin += value;
        AsisstantPanel.PlayerBalanceUpdate();
    }

    public override void TryMinusCoin(int value, out bool isBankrupt, out bool hasDebt)
    {
        isBankrupt = false;
        hasDebt = false;

        // 파산 여부 체크
        if (coin <= value)
        {
            isBankrupt = true;

            // 지불할 돈이 부족한지 체크
            if (coin < value)
            {
                hasDebt = true;
            }
        }

        // coin이 (-)마이너스가 된다면 주최측이 빚으로 달고 대신 내준다는 설정
        coin = coin - value;

        AsisstantPanel.PlayerBalanceUpdate();
    }

    public void SelectCard_OnStartTime()
    {
        for (int i = 0; i <closedCardList.Count; i++)
        {
            TrumpCardDefault card = closedCardList[i].GetComponent<TrumpCardDefault>();
            if(card != null)
            {
                card.TrySelectThisCard_OnGameSetting(this);
            }
        }
    }

    public void SelectTarget_OnPlayTime()
    {
        Debug.Log($"컴퓨터 \"{gameObject.name}\"가 공격할 대상을 선택합니다.");

        // 먹잇감이 존재하고 아직 파산하지 않았다면
        if(Prey != null && PlayerList.Contains(Prey)) // 자신이 먹잇감이면 공격 시도도 못함
        {
            if (TrySetAttackTarget(CardGamePlayManager.Instance.Prey)) return; // 성공했으면 여기서 종료
            else Debug.LogWarning("먹잇감이 있으나 설정 실패함");
        }

        // 자신 이외의 다른 플레이어 찾기
        CardGamePlayerBase weekPlayer;
        int i = 0;
        if (PlayerList[i] != this) weekPlayer = PlayerList[i];
        else weekPlayer = PlayerList[++i];
        for (i++ ; i < PlayerList.Count; i++)
        {
            if (PlayerList[i] == this) continue;

            if(weekPlayer.closedCardList.Count > PlayerList[i].closedCardList.Count) // 손패가 더 적은 쪽을 공략
            {
                weekPlayer = PlayerList[i];
            }
            else if(weekPlayer.closedCardList.Count == PlayerList[i].closedCardList.Count &&
                weekPlayer.openedCardList.Count < PlayerList[i].openedCardList.Count) // 손패가 같을 경우 공개된 카드가 더 많은 쪽을 공략
            {
                weekPlayer = PlayerList[i];
            }
        }

        // 올바르게 찾아졌다면 여기서 멈춤
        if (TrySetAttackTarget(weekPlayer)) return;

        // 만약을 대비해 랜덤대상을 설정
        int randomPlayerIndex;
        do{
            randomPlayerIndex = Random.Range(0, PlayerList.Count);
        } while (TrySetAttackTarget(PlayerList[randomPlayerIndex]) == false); // 세팅에 실패했으면 반복
    }

    public void SelectCard_OnPlayTime(bool isAttack)
    {
        Debug.Log($"컴퓨터 \"{gameObject.name}\"가 사용할 카드를 선택합니다.");

        if (closedCardList.Count <= 0)
        {
            Debug.LogWarning("player {gameObject.name}는 사용할 수 있는 카드가 없음");
            return;
        }

        TrumpCardDefault selectedCard = null;

        // 먹잇감이 있으면 내 카드중 값이 가장 큰 것을 선택
        if (isAttack)
        {
            if(CardGamePlayManager.Instance.Prey != null)
            {
                selectedCard = closedCardList[0].GetComponent<TrumpCardDefault>();

                // 카드가 1장만 있는경우
                if (closedCardList.Count == 1)
                {
                    if (selectedCard.TrySelectThisCard_OnPlayTime(this)) return;
                }

                // 카드가 2장 이상인 경우
                TrumpCardDefault currentCardScript = null;
                TrumpCardDefault joker = null;

                // 값이 가장 큰 카드 찾기
                for (int i = 1; i < closedCardList.Count; i++)
                {
                    currentCardScript =  closedCardList[i].GetComponent<TrumpCardDefault>();

                    // 조커는 따로 관리
                    if (currentCardScript.trumpCardInfo.cardType == eCardType.Joker)
                    {
                        joker = currentCardScript;
                        continue;
                    }

                    // 조커카드를 제외한 가장 큰값의 카드를 선택, 첫 카드가 조커일 가능성도 있음
                    else if (selectedCard.trumpCardInfo.cardValue < currentCardScript.trumpCardInfo.cardValue ||
                                selectedCard.trumpCardInfo.cardType == eCardType.Joker)
                    {
                        selectedCard = currentCardScript;
                    }
                }
                // 선택된 카드가 너무 작은 경우
                if(selectedCard.trumpCardInfo.cardValue < 7)
                    if(joker != null) selectedCard = joker; // 조커가 있으면 조커를 선택카드로 변경

                if (selectedCard.TrySelectThisCard_OnPlayTime(this)) return;
            }
        }

        // 그렇지 않으면 상대의 손패중에 공개된 카드를 고려하여 선택
        TrumpCardDefault revealedCardScript = null;

        // 공격
        if (isAttack)
        {
            foreach (GameObject revealedCard in AttackTarget.revealedCardList)
            {
                revealedCardScript = revealedCard.GetComponent<TrumpCardDefault>();
                foreach (var myCard in closedCardList)
                {
                    selectedCard = myCard.GetComponent<TrumpCardDefault>();

                    // 상대 패에 확인된 카드가 내 카드와 문양이 같으면 선택
                    if (revealedCardScript.trumpCardInfo.cardType == selectedCard.trumpCardInfo.cardType ||
                    revealedCardScript.trumpCardInfo.cardValue == selectedCard.trumpCardInfo.cardValue)
                    {
                        if (selectedCard.TrySelectThisCard_OnPlayTime(this)) return;
                    }

                }
            }
        }
        

        // 공격 or 수비 : 조커가 있는 경우 조커를 선택
        // 공격할 때는 조커를 아끼지만 수비할 때는 카운터용으로 우선해서 사용
        foreach (var myCard in closedCardList)
        {
            selectedCard = myCard.GetComponent<TrumpCardDefault>();
            
        }

        // 수비
        if (!isAttack)
        {
            foreach (GameObject revealedCard in CardGamePlayManager.Instance.Attacker.revealedCardList)
            {
                revealedCardScript = revealedCard.GetComponent<TrumpCardDefault>();
                foreach (var myCard in closedCardList)
                {
                    selectedCard = myCard.GetComponent<TrumpCardDefault>();

                    // 상대 패에 확인된 카드가 내 카드와 문양도 숫자도 같지 않으면 선택
                    if (revealedCardScript.trumpCardInfo.cardType != selectedCard.trumpCardInfo.cardType &&
                    revealedCardScript.trumpCardInfo.cardValue != selectedCard.trumpCardInfo.cardValue)
                    {
                        if (selectedCard.TrySelectThisCard_OnPlayTime(this)) return;
                    }
                }
            }
        }


        //// 조커도 없는 경우(공격)
        //if (isAttack)
        //{
        //    // 상대의 오픈패 중에 내 수중에 있는 카드와 같은 타입이 있다면 해당 카드를 선택
        //    TrumpCardDefault OpenedCardOfTarget = null;
        //    foreach (var OpenedCardObjOfTarget in AttackTarget.openedCardList)
        //    {
        //        OpenedCardOfTarget = OpenedCardObjOfTarget.GetComponent<TrumpCardDefault>();

        //        // 오픈된 카드의 타입이 상대의 손패에 없는 경우
        //        // 카드를 공격에 사용하고 실패한 케이스에 해당함
        //        if (AttackTarget.cardCountPerType_OnGame[OpenedCardOfTarget.trumpCardInfo.cardType] == 0)
        //        {
        //            Debug.Log("해당카드의 타입은 이미 전부 오픈되었음");
        //            continue;
        //        }

        //        // 오픈된 카드와 같은 문양이 아직 손패에 있는 경우
        //        foreach (var myCard in closedCardList)
        //        {
        //            selectedCard = myCard.GetComponent<TrumpCardDefault>();
        //            if (OpenedCardOfTarget.trumpCardInfo.cardType == selectedCard.trumpCardInfo.cardType) // 이 경우 숫자가 같은 것은 의미 없음
        //            {
        //                if (selectedCard.TrySelectThisCard_OnPlayTime(this)) return;
        //            }
        //        }
        //    }
        //}

        // 카드 선택을 못한 경우 랜덤으로 설정
        int randomCardIndex;
        do
        {
            randomCardIndex = Random.Range(0, closedCardList.Count);
            selectedCard = closedCardList[randomCardIndex].GetComponent<TrumpCardDefault>();
        } while ((selectedCard.TrySelectThisCard_OnPlayTime(this)) == false); // 세팅에 실패하면 반복
        return;
    }



    public override void AttackOtherPlayers(List<CardGamePlayerBase> PlayerList)
    {
        // 컴퓨터의 공격대상 선택
        SelectTarget_OnPlayTime();

        // 공격에 사용할 카드 선택
        SelectCard_OnPlayTime(true);

        // 모두 완료되었으면 애니메이션 실행후 다음으로 진행
        PlaySequnce_PresentCard(true);
    }

    public override void DeffenceFromOtherPlayers(CardGamePlayerBase AttackerScript)
    {
        // 수비에 사용할 카드를 선택
        SelectCard_OnPlayTime(false);

        // 모두 완료되었으면 애니메이션 실행후 다음으로 진행
        PlaySequnce_PresentCard(false);
    }
}
