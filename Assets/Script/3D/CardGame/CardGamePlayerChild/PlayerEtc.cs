
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

        // �Ļ� ���� üũ
        if (coin <= value)
        {
            isBankrupt = true;

            // ������ ���� �������� üũ
            if (coin < value)
            {
                hasDebt = true;
            }
        }

        // coin�� (-)���̳ʽ��� �ȴٸ� �������� ������ �ް� ��� ���شٴ� ����
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
        Debug.Log($"��ǻ�� \"{gameObject.name}\"�� ������ ����� �����մϴ�.");

        // ���հ��� �����ϰ� ���� �Ļ����� �ʾҴٸ�
        if(Prey != null && PlayerList.Contains(Prey)) // �ڽ��� ���հ��̸� ���� �õ��� ����
        {
            if (TrySetAttackTarget(CardGamePlayManager.Instance.Prey)) return; // ���������� ���⼭ ����
            else Debug.LogWarning("���հ��� ������ ���� ������");
        }

        // �ڽ� �̿��� �ٸ� �÷��̾� ã��
        CardGamePlayerBase weekPlayer;
        int i = 0;
        if (PlayerList[i] != this) weekPlayer = PlayerList[i];
        else weekPlayer = PlayerList[++i];
        for (i++ ; i < PlayerList.Count; i++)
        {
            if (PlayerList[i] == this) continue;

            if(weekPlayer.closedCardList.Count > PlayerList[i].closedCardList.Count) // ���а� �� ���� ���� ����
            {
                weekPlayer = PlayerList[i];
            }
            else if(weekPlayer.closedCardList.Count == PlayerList[i].closedCardList.Count &&
                weekPlayer.openedCardList.Count < PlayerList[i].openedCardList.Count) // ���а� ���� ��� ������ ī�尡 �� ���� ���� ����
            {
                weekPlayer = PlayerList[i];
            }
        }

        // �ùٸ��� ã�����ٸ� ���⼭ ����
        if (TrySetAttackTarget(weekPlayer)) return;

        // ������ ����� ��������� ����
        int randomPlayerIndex;
        do{
            randomPlayerIndex = Random.Range(0, PlayerList.Count);
        } while (TrySetAttackTarget(PlayerList[randomPlayerIndex]) == false); // ���ÿ� ���������� �ݺ�
    }

    public void SelectCard_OnPlayTime(bool isAttack)
    {
        Debug.Log($"��ǻ�� \"{gameObject.name}\"�� ����� ī�带 �����մϴ�.");

        if (closedCardList.Count <= 0)
        {
            Debug.LogWarning("player {gameObject.name}�� ����� �� �ִ� ī�尡 ����");
            return;
        }

        TrumpCardDefault selectedCard = null;

        // ���հ��� ������ �� ī���� ���� ���� ū ���� ����
        if (isAttack)
        {
            if(CardGamePlayManager.Instance.Prey != null)
            {
                selectedCard = closedCardList[0].GetComponent<TrumpCardDefault>();

                // ī�尡 1�常 �ִ°��
                if (closedCardList.Count == 1)
                {
                    if (selectedCard.TrySelectThisCard_OnPlayTime(this)) return;
                }

                // ī�尡 2�� �̻��� ���
                TrumpCardDefault currentCardScript = null;
                TrumpCardDefault joker = null;

                // ���� ���� ū ī�� ã��
                for (int i = 1; i < closedCardList.Count; i++)
                {
                    currentCardScript =  closedCardList[i].GetComponent<TrumpCardDefault>();

                    // ��Ŀ�� ���� ����
                    if (currentCardScript.trumpCardInfo.cardType == eCardType.Joker)
                    {
                        joker = currentCardScript;
                        continue;
                    }

                    // ��Ŀī�带 ������ ���� ū���� ī�带 ����, ù ī�尡 ��Ŀ�� ���ɼ��� ����
                    else if (selectedCard.trumpCardInfo.cardValue < currentCardScript.trumpCardInfo.cardValue ||
                                selectedCard.trumpCardInfo.cardType == eCardType.Joker)
                    {
                        selectedCard = currentCardScript;
                    }
                }
                // ���õ� ī�尡 �ʹ� ���� ���
                if(selectedCard.trumpCardInfo.cardValue < 7)
                    if(joker != null) selectedCard = joker; // ��Ŀ�� ������ ��Ŀ�� ����ī��� ����

                if (selectedCard.TrySelectThisCard_OnPlayTime(this)) return;
            }
        }

        // �׷��� ������ ����� �����߿� ������ ī�带 ����Ͽ� ����
        TrumpCardDefault revealedCardScript = null;

        // ����
        if (isAttack)
        {
            foreach (GameObject revealedCard in AttackTarget.revealedCardList)
            {
                revealedCardScript = revealedCard.GetComponent<TrumpCardDefault>();
                foreach (var myCard in closedCardList)
                {
                    selectedCard = myCard.GetComponent<TrumpCardDefault>();

                    // ��� �п� Ȯ�ε� ī�尡 �� ī��� ������ ������ ����
                    if (revealedCardScript.trumpCardInfo.cardType == selectedCard.trumpCardInfo.cardType ||
                    revealedCardScript.trumpCardInfo.cardValue == selectedCard.trumpCardInfo.cardValue)
                    {
                        if (selectedCard.TrySelectThisCard_OnPlayTime(this)) return;
                    }

                }
            }
        }
        

        // ���� or ���� : ��Ŀ�� �ִ� ��� ��Ŀ�� ����
        // ������ ���� ��Ŀ�� �Ƴ����� ������ ���� ī���Ϳ����� �켱�ؼ� ���
        foreach (var myCard in closedCardList)
        {
            selectedCard = myCard.GetComponent<TrumpCardDefault>();
            
        }

        // ����
        if (!isAttack)
        {
            foreach (GameObject revealedCard in CardGamePlayManager.Instance.Attacker.revealedCardList)
            {
                revealedCardScript = revealedCard.GetComponent<TrumpCardDefault>();
                foreach (var myCard in closedCardList)
                {
                    selectedCard = myCard.GetComponent<TrumpCardDefault>();

                    // ��� �п� Ȯ�ε� ī�尡 �� ī��� ���絵 ���ڵ� ���� ������ ����
                    if (revealedCardScript.trumpCardInfo.cardType != selectedCard.trumpCardInfo.cardType &&
                    revealedCardScript.trumpCardInfo.cardValue != selectedCard.trumpCardInfo.cardValue)
                    {
                        if (selectedCard.TrySelectThisCard_OnPlayTime(this)) return;
                    }
                }
            }
        }


        //// ��Ŀ�� ���� ���(����)
        //if (isAttack)
        //{
        //    // ����� ������ �߿� �� ���߿� �ִ� ī��� ���� Ÿ���� �ִٸ� �ش� ī�带 ����
        //    TrumpCardDefault OpenedCardOfTarget = null;
        //    foreach (var OpenedCardObjOfTarget in AttackTarget.openedCardList)
        //    {
        //        OpenedCardOfTarget = OpenedCardObjOfTarget.GetComponent<TrumpCardDefault>();

        //        // ���µ� ī���� Ÿ���� ����� ���п� ���� ���
        //        // ī�带 ���ݿ� ����ϰ� ������ ���̽��� �ش���
        //        if (AttackTarget.cardCountPerType_OnGame[OpenedCardOfTarget.trumpCardInfo.cardType] == 0)
        //        {
        //            Debug.Log("�ش�ī���� Ÿ���� �̹� ���� ���µǾ���");
        //            continue;
        //        }

        //        // ���µ� ī��� ���� ������ ���� ���п� �ִ� ���
        //        foreach (var myCard in closedCardList)
        //        {
        //            selectedCard = myCard.GetComponent<TrumpCardDefault>();
        //            if (OpenedCardOfTarget.trumpCardInfo.cardType == selectedCard.trumpCardInfo.cardType) // �� ��� ���ڰ� ���� ���� �ǹ� ����
        //            {
        //                if (selectedCard.TrySelectThisCard_OnPlayTime(this)) return;
        //            }
        //        }
        //    }
        //}

        // ī�� ������ ���� ��� �������� ����
        int randomCardIndex;
        do
        {
            randomCardIndex = Random.Range(0, closedCardList.Count);
            selectedCard = closedCardList[randomCardIndex].GetComponent<TrumpCardDefault>();
        } while ((selectedCard.TrySelectThisCard_OnPlayTime(this)) == false); // ���ÿ� �����ϸ� �ݺ�
        return;
    }



    public override void AttackOtherPlayers(List<CardGamePlayerBase> PlayerList)
    {
        // ��ǻ���� ���ݴ�� ����
        SelectTarget_OnPlayTime();

        // ���ݿ� ����� ī�� ����
        SelectCard_OnPlayTime(true);

        // ��� �Ϸ�Ǿ����� �ִϸ��̼� ������ �������� ����
        PlaySequnce_PresentCard(true);
    }

    public override void DeffenceFromOtherPlayers(CardGamePlayerBase AttackerScript)
    {
        // ���� ����� ī�带 ����
        SelectCard_OnPlayTime(false);

        // ��� �Ϸ�Ǿ����� �ִϸ��̼� ������ �������� ����
        PlaySequnce_PresentCard(false);
    }
}
