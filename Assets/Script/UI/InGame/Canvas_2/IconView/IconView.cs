using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using PublicSet;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System;

public class IconView : MonoBehaviour
{
    // ������ ����
    public RectTransform rectTrans;
    public float ViewOnOffDelay;

    public Icon[] icons;

    // ��ũ��Ʈ ����
    private IconViewOnOffButton iconViewOnOffButton
    {
        get { return iconDict[eIcon.IconViewOnOff].iconButton as IconViewOnOffButton; }
    }
    private Vector2 Center_anchoredPos;
    private Vector2 OutOfScreen_anchoredPos;
    bool isIconViewOpen;
    Coroutine currentCoroutine;
    public int OpenedIconCount {  get; private set; }

    

    private Dictionary<eIcon, Icon> _iconDict;
    private Dictionary<eIcon, Icon> iconDict
    {
        get
        {
            if (_iconDict == null)
            {
                InitIconDict();
            }
            return _iconDict;
        }
        set { _iconDict = value; }
    }

    public void InitIconDict()
    {
        _iconDict = new Dictionary<eIcon, Icon>();

        foreach(eIcon key in Enum.GetValues(typeof(eIcon)))
        {
            int index = (int)key;

            if(index < icons.Length)
            {
                if (iconDict.ContainsKey(key)) iconDict[key] = icons[index];
                else iconDict.Add(key, icons[index]);
                index++;
            }
            else
            {
                Debug.LogAssertion("�ε��� ��������");
            }
        }
    }

    public void SetOpendIconCount(int value)
    {
        OpenedIconCount = value + 1; // 0���� �����ܺ� �¿����̴� +1�� ��
        if (OpenedIconCount >= 1)
        {
            // 0�� ������ �н�
            for (int i = 1; i < OpenedIconCount; i++)
            {
                if(Enum.IsDefined(typeof(eIcon), i))
                {
                    eIcon key = (eIcon)i;

                    Destroy(iconDict[key].iconLock);
                    iconDict[key].iconLock = null;
                }
                else
                {
                    Debug.LogWarning("���ǵ��� ���� ��ųʸ� ����");
                }
            }
        }
    }

    private void Awake()
    {
        if (ViewOnOffDelay < 0.1f)
        {
            ViewOnOffDelay = 0.5f;
        }
        SetPos();

        InitIconDict();
        OpenedIconCount = 0;

    }

    private void SetPos()
    {
        OutOfScreen_anchoredPos = rectTrans.rect.size;

        // �����ܹڽ� 1����ŭ�� x�� ���̸� ���� -> ȭ��ǥ �������� ȭ�鿡 �׻� ���̰� ����
        OutOfScreen_anchoredPos.x = OutOfScreen_anchoredPos.x - OutOfScreen_anchoredPos.y;

        // ������κ��� �������� �Ʒ��� ������
        OutOfScreen_anchoredPos.y = -(OutOfScreen_anchoredPos.y/2);
        
        // �����ܺ䰡 ���Ϳ� ���� ��ġ�� ��Ŀ�������� y�ุ ��ȭ�� ��
        Center_anchoredPos = OutOfScreen_anchoredPos;
        Center_anchoredPos.x = 0f;

        rectTrans.anchoredPosition = OutOfScreen_anchoredPos;
    }


    /// <summary>
    /// ������ �䰡 ������ ���� �ð� �� �������� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator IconViewCloseDelay()
    {
        yield return new WaitForSeconds(5f);
        IconViewClose();
        currentCoroutine = null;
    }
    public void IconViewOpen()
    {
        PlaySequnce_IconViewProcess(Center_anchoredPos, true);
    }
    public void IconViewClose()
    {
        PlaySequnce_IconViewProcess(OutOfScreen_anchoredPos, false);
    }
    private void PlaySequnce_IconViewProcess(Vector3 tragetAnchoredPos ,bool boolActive, Sequence sequenceAppend = null)
    {
        // ���� �ʱ�ȭ
        isIconViewOpen = boolActive;

        Sequence sequence = DOTween.Sequence();

        // iconView �̵�
        sequence.Append(rectTrans.DOAnchorPos(tragetAnchoredPos, ViewOnOffDelay).SetEase(Ease.OutQuad));

        // �����ܺ並 ���� ���
        if(isIconViewOpen)
        {
            // �̹��� �����ܺ並 �����ߴٸ� �������ʴ� �ݴ� ��
            iconViewOnOffButton.SetButtonCallback(IconViewClose);

            // �����ܺ� �ڵ��ݱ⸦ �����ϴ� �ڷ�ƾ
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(IconViewCloseDelay());

            sequence.AppendCallback(
                () =>
                {
                    // �̹��� ��ȯ
                    iconViewOnOffButton.SetImage_ToClose();

                    // �����ܺ䰡 �������� � �˸��� �Ե� �¿����� ���̵�� ����
                    TryClickGuideOff(eIcon.IconViewOnOff);
                });
            
        }
        // �����ܺ並 �ݴ� ���
        else
        {
            iconViewOnOffButton.SetButtonCallback(IconViewOpen);
            
            // �����ܺ䰡 �����ٸ� �����ܺ並 �ݴ� �ڷ�ƾ�� �ʿ����
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            sequence.AppendCallback(
                () =>
                {
                    // �̹��� ��ȯ
                    iconViewOnOffButton.SetImage_ToOpen();

                    // �����ܺ䰡 �������� Ȯ���ؾ��� �˸��� �ִٸ� ���̵带 on
                    TryClickGuideOn(eIcon.IconViewOnOff);
                });

            
        }

        // ������ �䰡 ������ �� �߰����� ó���� �ʿ��ϸ� sequence�� �߰�
        if (sequenceAppend != null)
        {
            sequence.Append(sequenceAppend);
        }


        sequence.SetLoops(1);
        sequence.Play();
    }
    public bool TryIconUnLock(eIcon choice)
    {
        if(iconDict.ContainsKey(choice) && choice != eIcon.IconViewOnOff)
        {
            // ������ ���� ������ ��� ������ ����
            if(iconDict[choice].iconLock != null)
            {
                Transform iconLockTrans = iconDict[choice].iconLock.transform;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(iconLockTrans.DOScale(Vector3.one * 2f, 0.3f))
                        .Append(iconLockTrans.DOScale(Vector3.zero, 1f))
                        .AppendCallback(() => { Destroy(iconDict[choice].iconLock); })
                        .SetLoops(1);

                // �������� �����ִ� ���
                if (isIconViewOpen == false)
                {
                    // IconViewProcess ���ο��� sequence�� �߰��Ͽ� Ʈ���� ������
                    PlaySequnce_IconViewProcess(Center_anchoredPos, true, sequence);
                }
                // �������� �����ִ� ���
                else
                {
                    sequence.Play();
                }

                OpenedIconCount++;
                return true;
            }
            else
            {
                Debug.Log("������ ���� �̹� �Ҹ�����");
                return false;
            }
            
        }
        else
        {
            Debug.LogWarning("������ ��ü�� ������ �� ����");
            return false;
        }
        
        
    }

    public void TryClickGuideOn(eIcon choice)
    {
        if (isIconViewOpen == false) // ������ �䰡 �������� ��
        {
            foreach(eIcon key in iconDict.Keys)
            {
                if (key == eIcon.IconViewOnOff) continue;

                // �ϳ��� ���� �˶��� ������
                if (iconDict[key].clickGuide.activeSelf)
                {
                    // �����ܺ� ������ ����
                    iconDict[eIcon.IconViewOnOff].clickGuide.gameObject.SetActive(true);

                    break; // �ݺ��� Ż��
                }
            }
        }

        // �¿����� ȣ���� ��� ������
        if (choice != eIcon.IconViewOnOff)
        {
            iconDict[choice].clickGuide.gameObject.SetActive(true);
            TryClickGuideOn(eIcon.IconViewOnOff);
        }
            
    }

    public void TryClickGuideOff(eIcon choice)
    {
        switch (choice)
        {
            case eIcon.IconViewOnOff:
                {
                    // �����ִٸ� ���̵带 �����ϴ��� �ٽ��ѹ� Ȯ��
                    if(isIconViewOpen == false)
                    {
                        foreach (eIcon key in iconDict.Keys)
                        {
                            if (key == eIcon.IconViewOnOff) continue;
                            if (iconDict[key].clickGuide.activeSelf) // �ϳ��� �����ִٸ� �¿����� ���̵嵵 �����ؾ���
                                return;
                        }
                    }
                    
                }
                break;

            case eIcon.Quest:
                {
                    // ��� ����Ʈ�� Ȯ���� ��� &&
                    // ��� �Ϸ�� ����Ʈ�� ������ ���� ���
                    foreach (sQuest quest in QuestManager.questHashSet)
                    {
                        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);
                        if(questInfo.isNeedCheck == true) // �ϳ��� true�� �ִٸ� ��Ȱ��ȭ ���� �ʰ� �Լ� ����
                        {
                            return;
                        }
                    }
                }
                break;

            case eIcon.Inventory:
                {
                    // ��� ȹ���� �������� Ȯ���� ���

                }
                break;

            case eIcon.GameAssistant:
                {
                    // �÷��̾� �������ʿ� ���ݴ���� ���� �Ϸ��� ���
                }
                break;

            case eIcon.Message:
                {

                }
                break;
        }

        // Ȱ��ȭ �Ǿ������� ��Ȱ��ȭ
        iconDict[choice].clickGuide.gameObject.SetActive(false);

        // �ٸ� �����ܺ��� Ȯ���� ��ģ��쿡�� ����
        if (choice != eIcon.IconViewOnOff)
        {
            // �����ܺ䰡 �������� �� Ȯ���� ���� ��� �����ܺ�onOff�� ���� Ȯ���� �����
            TryClickGuideOff(eIcon.IconViewOnOff);
        }
            
    }
}
