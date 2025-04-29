using DG.Tweening;
using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface ICallback<T>
{
    public abstract UnityAction CallbackList(T index);
}

public abstract class CallbackBase : MonoBehaviour
{
    // 스크립트로 편집
    protected static eStage currentStage { get { return GameManager.Instance.currentStage; } }
    protected static Dictionary<eStage, string> stageMessageDict { get { return GameManager.Instance.stageMessageDict; } }
    protected static Connector_InGame connector_InGame { get { return GameManager.connector_InGame; } }
    protected static sPlayerStatus playerStatus { get { return PlayManager.Instance.currentPlayerStatus; } }

    protected static Image blackViewImage;

    public static void TextWindowPopUp_Open()
    {
        GameManager.connector_InGame.textWindowView.SetActive(true);
        GameManager.connector_InGame.interfaceView.SetActive(false);
    }

    public static void TextWindowPopUp_Close()
    {
        GameManager.connector_InGame.textWindowView.SetActive(false);

        // 카지노 게임뷰가 아닌 경우에만 인터페이스를 활성화
        if (GameManager.Instance.isCasinoGameView == false)
        {
            GameManager.connector_InGame.interfaceView.SetActive(true);
        }
    }

    public static void PlaySequnce_BlackViewProcess(float delay, Action middleCallBack, Action endCallback = null)
    {
        //isBlakcViewReady = false;

        // 대화창 끄고 일시정지
        GameManager.connector_InGame.textWindowView.SetActive(false);
        GameManager.Instance.Pause_theGame();

        // 먼저 화면가림막 활성화
        GameManager.connector.blackView.SetActive(true);

        // 화면이 검게 변했다가 다시 원상복귀됨
        if (blackViewImage == null)
        {
            blackViewImage = GameManager.connector.blackView.GetComponent<Image>();
        }

        // 시퀀스 생성
        Sequence sequence = DOTween.Sequence();

        // 시퀀스 설정
        sequence.AppendCallback(() => blackViewImage.color = Color.clear);
        sequence.Append(blackViewImage.DOColor(Color.black, delay / 2));

        if (middleCallBack != null)
        {
            sequence.AppendCallback(() => middleCallBack());
        }

        sequence.Append(blackViewImage.DOColor(Color.clear, delay / 2));

        sequence.AppendCallback(
        () =>
        {
            // 비활성화를 해야 화면 클릭이 가능함
            GameManager.connector.blackView.SetActive(false);

            // 게임 지속
            GameManager.Instance.Continue_theGame();

            //isBlakcViewReady = true;
        }
        );

        if (endCallback != null)
        {
            sequence.AppendCallback(() => endCallback());
        }


        sequence.SetLoops(1);

        // 시퀀스 플레이(시퀀스 플레이는 생성후 최초 1회만 플레이 가능함)
        sequence.Play();
    }

    public static void CasinoViewOpen()
    {
        //Debug.Log("카지노 입장");
        float delay = 2.0f;
        PlaySequnce_BlackViewProcess(delay,
            () =>
            {
                GameManager.connector_InGame.canvas0_InGame.CasinoViewOpen();
            },
            () =>
            {
                GameManager.connector_InGame.canvas0_InGame.casinoView.StartDealerDialogue();
            }
            );
    }

    public static void TrashFuc()
    {
        Debug.LogAssertion("정의되지 않은 콜백함수");
    }

    
}
