using DG.Tweening;
using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : Singleton<EventManager>
{
    public Connector_InGame connector_InGame { get { return GameManager.connector_InGame; } }
    


    Dictionary<eStage, string> StageMessageDict;

    protected override void Awake()
    {
        base.Awake();
        Init_StageMessageDict();
    }
    public void Init_StageMessageDict()
    {
        StageMessageDict = new Dictionary<eStage, string>();
        StageMessageDict.Add(eStage.Stage1, "STAGE 1\n���Ⱑ ��ü ����?");
        StageMessageDict.Add(eStage.Stage2, "STAGE 2\nī���뿡 �Լ�����");
    }


    readonly float intervalDelay = 0.5f; // �̺�Ʈ ������ ��������ȭ�� ���� ���ð�
    readonly float delay = 1f; // ȭ�� ����ð�
    readonly Color colorClearAlpha = new Color(1, 1, 1, 0);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sequence"></param>
    /// <param name="middleCallback">�̺�Ʈ�� ���� ����</param>
    /// <param name="eventText">�̺�Ʈ ������ ���� �ؽ�Ʈ</param>
    public void GetSequnce_EventAnimation(Sequence sequence, CanvasGroup canvasGroup)
    {
        //Debug.Log("stage �ִϸ��̼� ����");


        // �̺�Ʈȭ�� Ȱ��ȭ
        connector_InGame.Canvas2.EventView.gameObject.SetActive(true);

        // �̹��� ���� �ʱ�ȭ
        canvasGroup.alpha = 0f;

        // �̺�Ʈ ���� �� ����
        sequence.AppendInterval(intervalDelay);

        // �̺�Ʈ ȭ�� ���̵���
        sequence.Append(canvasGroup.DOFade(1f, delay));

        // ����
        sequence.AppendInterval(delay);

        // �̺�Ʈ ȭ�� ���̵�ƿ�
        sequence.Append(canvasGroup.DOFade(0f, delay));

        // �̺�Ʈ ȭ�� ��Ȱ��ȭ
        sequence.AppendCallback(() => connector_InGame.Canvas2.EventView.gameObject.SetActive(false));
    }


    public void SetEventMessage(string message)
    {
        connector_InGame.Canvas2.EventView.SetTextContent(message); // �̹��� ���� �ؽ�Ʈ ����
    }

    public void PlaySequnce_EventAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        GetSequnce_EventAnimation(sequence, connector_InGame.Canvas2.EventView.canvasGroup);

        sequence.SetLoops(1);
        sequence.Play();
    }
}
