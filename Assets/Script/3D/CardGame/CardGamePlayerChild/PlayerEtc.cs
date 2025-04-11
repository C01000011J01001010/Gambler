
using UnityEngine;
using System.Collections.Generic;
using PublicSet;

public class PlayerEtc : CardGamePlayerBase
{
    public OnlyOneLivesPlayerPanel AsisstantPanel {  get; private set; }

    public void SetAsisstantPanel(OnlyOneLivesPlayerPanel value)
    {
        AsisstantPanel = value;
    }
    public override void AddCoin(int value)
    {
        coin += value;
        AsisstantPanel.PlayerBalanceUpdate();
    }

    public override int TryMinusCoin(int value, out bool isBankrupt)
    {
        if (coin <= value)
        {
            value = coin; // 남은돈에 한하여 상대방에게 지급함
            coin = 0;
            isBankrupt = true;
        }
        else
        {
            coin = coin - value;
            isBankrupt = false;
        }
        AsisstantPanel.PlayerBalanceUpdate();
        return value;
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

    public void SelectTarget_OnPlayTime(List<CardGamePlayerBase> playerList)
    {
        Debug.Log($"컴퓨터 \"{gameObject.name}\"가 공격할 대상을 선택합니다.");

        // 먹잇감이 있으면 대상으로 설정
        if(CardGamePlayManager.Instance.Prey != null) // 자신이 먹잇감이면 공격 시도도 못함
        {
            if (TrySetAttackTarget(CardGamePlayManager.Instance.Prey)) return; // 성공했으면 여기서 종료
            else Debug.LogWarning("먹잇감이 있으나 설정 실패함");
        }

        // 자신 이외의 다른 플레이어 찾기
        CardGamePlayerBase weekPlayer;
        int i = 0;
        if (playerList[i] != this) weekPlayer = playerList[i];
        else weekPlayer = playerList[++i];
        for (i++ ; i <playerList.Count; i++)
        {
            if (playerList[i] == this) continue;

            if(weekPlayer.closedCardList.Count > playerList[i].closedCardList.Count) // 손패가 더 적은 쪽을 공략
            {
                weekPlayer = playerList[i];
            }
            else if(weekPlayer.closedCardList.Count == playerList[i].closedCardList.Count &&
                weekPlayer.openedCardList.Count < playerList[i].openedCardList.Count) // 손패가 같을 경우 공개된 카드가 더 많은 쪽을 공략
            {
                weekPlayer = playerList[i];
            }
        }

        // 올바르게 찾아졌다면 여기서 멈춤
        if (TrySetAttackTarget(weekPlayer)) return;

        // 만약을 대비해 랜덤대상을 설정
        int randomPlayerIndex;
        do{
            randomPlayerIndex = Random.Range(0, playerList.Count);
        } while (TrySetAttackTarget(playerList[randomPlayerIndex]) == false); // 세팅에 실패했으면 반복
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
        if(isAttack)
        {
            // 먹잇감이 있으면 내 카드중 값이 가장 큰 것을 선택
            if(CardGamePlayManager.Instance.Prey != null)
            {
                selectedCard = closedCardList[0].GetComponent<TrumpCardDefault>();

                // 카드가 1장만 있는경우
                if (closedCardList.Count == 1)
                {
                    if (TyrSetPresentedCard(selectedCard))
                    {
                        Debug.Log($"사용된 카드 : {PresentedCardScript}");
                        return;
                    }
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

                if (TyrSetPresentedCard(selectedCard))
                {
                    Debug.Log($"사용된 카드 : {PresentedCardScript}");
                    return;
                }
            }

            // 상대의 손패중에 공개된 카드를 우선해서 선택
            TrumpCardDefault revealedCardScript = null;
            foreach (GameObject revealedCard in AttackTarget.revealedCardList)
            {
                if(closedCardList.Contains(revealedCard))
                {
                    revealedCardScript = revealedCard.GetComponent<TrumpCardDefault>();
                    foreach (var myCard in closedCardList)
                    {
                        selectedCard = myCard.GetComponent<TrumpCardDefault>();
                        if (revealedCardScript.trumpCardInfo.cardType == selectedCard.trumpCardInfo.cardType ||
                            revealedCardScript.trumpCardInfo.cardValue == selectedCard.trumpCardInfo.cardValue)
                        {
                            if (selectedCard.TrySelectThisCard_OnPlayTime(this))
                            {
                                Debug.Log($"사용된 카드 : {PresentedCardScript}");
                                return;
                            }
                        }
                    }
                }
            }

            // 상대의 오픈패 중에 내 수중에 있는 카드와 같은 타입이 있다면 해당 카드를 선택
            TrumpCardDefault OpenedCardOfTarget = null;
            foreach (var targetCard in AttackTarget.openedCardList)
            {
                OpenedCardOfTarget = targetCard.GetComponent<TrumpCardDefault>();
                foreach (var myCard in closedCardList)
                {
                    selectedCard = myCard.GetComponent<TrumpCardDefault>();
                    if (OpenedCardOfTarget.trumpCardInfo.cardType == selectedCard.trumpCardInfo.cardType ||
                        OpenedCardOfTarget.trumpCardInfo.cardValue == selectedCard.trumpCardInfo.cardValue)
                    {
                        if (selectedCard.TrySelectThisCard_OnPlayTime(this))
                        {
                            if (TyrSetPresentedCard(selectedCard))
                            {
                                Debug.Log($"사용된 카드 : {PresentedCardScript}");
                                return;
                            }
                        }
                    }
                }
            }
        }
        

        // 공격 or 수비 : 조커가 있는 경우 조커를 선택
        foreach (var myCard in closedCardList)
        {
            selectedCard = myCard.GetComponent<TrumpCardDefault>();
            if (selectedCard.trumpCardInfo.cardType == PublicSet.eCardType.Joker)
            {
                if(selectedCard.TrySelectThisCard_OnPlayTime(this))
                {
                    if (TyrSetPresentedCard(selectedCard))
                    {
                        Debug.Log($"사용된 카드 : {PresentedCardScript}");
                        return;
                    }
                }
            }
        }


        // 카드 선택을 못한 경우 랜덤으로 설정
        int randomCardIndex;
        do
        {
            randomCardIndex = Random.Range(0, closedCardList.Count);
            selectedCard = closedCardList[randomCardIndex].GetComponent<TrumpCardDefault>();
        } while ((selectedCard.TrySelectThisCard_OnPlayTime(this)) == false); // 세팅에 실패하면 반복
        if (TyrSetPresentedCard(selectedCard))
        {
            Debug.Log($"사용된 카드 : {PresentedCardScript}");
        }
    }



    public override void AttackOtherPlayers(List<CardGamePlayerBase> PlayerList)
    {
        // 컴퓨터의 공격대상 선택
        SelectTarget_OnPlayTime(PlayerList);

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

    



    //public void PlaySequnce_Deffence()
    //{
    //    Sequence sequence = DOTween.Sequence();
    //    float returnDelay;

    //    // 카드를 제시하는 애니메이션
    //    returnDelay = GetSequnce_PresentCard(sequence, false);

    //    // 내 수비 끝내기
    //    Debug.Log($"{gameObject.name}이 수비를 실행함");

    //    // 양쪽 카드를 오픈
    //    sequence.AppendInterval(progressDelay);
    //    sequence.AppendCallback(CardGamePlayManager.Instance.NextProgress); // progress 302 실행
    //    //sequence.AppendCallback(()=> CardGamePlayManager.Instance.CardOpenAtTheSameTime(AttackerScript, this));

    //    sequence.SetLoops(1);
    //    sequence.Play();
    //    //Debug.Log($"애니메이션 시간 : {returnDelay}");
    //}
}
