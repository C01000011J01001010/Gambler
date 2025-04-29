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
    // ��ũ��Ʈ�� ����
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

        // ī���� ���Ӻ䰡 �ƴ� ��쿡�� �������̽��� Ȱ��ȭ
        if (GameManager.Instance.isCasinoGameView == false)
        {
            GameManager.connector_InGame.interfaceView.SetActive(true);
        }
    }

    public static void PlaySequnce_BlackViewProcess(float delay, Action middleCallBack, Action endCallback = null)
    {
        //isBlakcViewReady = false;

        // ��ȭâ ���� �Ͻ�����
        GameManager.connector_InGame.textWindowView.SetActive(false);
        GameManager.Instance.Pause_theGame();

        // ���� ȭ�鰡���� Ȱ��ȭ
        GameManager.connector.blackView.SetActive(true);

        // ȭ���� �˰� ���ߴٰ� �ٽ� ���󺹱͵�
        if (blackViewImage == null)
        {
            blackViewImage = GameManager.connector.blackView.GetComponent<Image>();
        }

        // ������ ����
        Sequence sequence = DOTween.Sequence();

        // ������ ����
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
            // ��Ȱ��ȭ�� �ؾ� ȭ�� Ŭ���� ������
            GameManager.connector.blackView.SetActive(false);

            // ���� ����
            GameManager.Instance.Continue_theGame();

            //isBlakcViewReady = true;
        }
        );

        if (endCallback != null)
        {
            sequence.AppendCallback(() => endCallback());
        }


        sequence.SetLoops(1);

        // ������ �÷���(������ �÷��̴� ������ ���� 1ȸ�� �÷��� ������)
        sequence.Play();
    }

    public static void CasinoViewOpen()
    {
        //Debug.Log("ī���� ����");
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
        Debug.LogAssertion("���ǵ��� ���� �ݹ��Լ�");
    }

    
}
