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
    // 에디터 편집
    public RectTransform rectTrans;
    public float ViewOnOffDelay;

    public Icon[] icons;

    // 스크립트 편집
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
                Debug.LogAssertion("인덱스 범위오류");
            }
        }
    }

    public void SetOpendIconCount(int value)
    {
        OpenedIconCount = value + 1; // 0번은 아이콘뷰 온오프이니 +1을 함
        if (OpenedIconCount >= 1)
        {
            // 0번 열거자 패스
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
                    Debug.LogWarning("정의되지 않은 딕셔너리 접근");
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

        // 아이콘박스 1개만큼의 x축 길이를 제외 -> 화살표 아이콘을 화면에 항상 보이게 만듬
        OutOfScreen_anchoredPos.x = OutOfScreen_anchoredPos.x - OutOfScreen_anchoredPos.y;

        // 상단으로부터 절반정도 아래로 내려감
        OutOfScreen_anchoredPos.y = -(OutOfScreen_anchoredPos.y/2);
        
        // 아이콘뷰가 센터에 오는 위치는 앵커기준으로 y축만 변화된 값
        Center_anchoredPos = OutOfScreen_anchoredPos;
        Center_anchoredPos.x = 0f;

        rectTrans.anchoredPosition = OutOfScreen_anchoredPos;
    }


    /// <summary>
    /// 아이콘 뷰가 열리면 일정 시간 후 닫히도록 설정
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
        // 변수 초기화
        isIconViewOpen = boolActive;

        Sequence sequence = DOTween.Sequence();

        // iconView 이동
        sequence.Append(rectTrans.DOAnchorPos(tragetAnchoredPos, ViewOnOffDelay).SetEase(Ease.OutQuad));

        // 아이콘뷰를 여는 경우
        if(isIconViewOpen)
        {
            // 이번에 아이콘뷰를 오픈했다면 다음차례는 닫는 것
            iconViewOnOffButton.SetButtonCallback(IconViewClose);

            // 아이콘뷰 자동닫기를 관리하는 코루틴
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(IconViewCloseDelay());

            sequence.AppendCallback(
                () =>
                {
                    // 이미지 교환
                    iconViewOnOffButton.SetImage_ToClose();

                    // 아이콘뷰가 열렸으니 어떤 알림이 왔든 온오프의 가이드는 꺼짐
                    TryClickGuideOff(eIcon.IconViewOnOff);
                });
            
        }
        // 아이콘뷰를 닫는 경우
        else
        {
            iconViewOnOffButton.SetButtonCallback(IconViewOpen);
            
            // 아이콘뷰가 닫혔다면 아이콘뷰를 닫는 코루틴은 필요없음
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            sequence.AppendCallback(
                () =>
                {
                    // 이미지 교환
                    iconViewOnOffButton.SetImage_ToOpen();

                    // 아이콘뷰가 닫혔으니 확인해야할 알림이 있다면 가이드를 on
                    TryClickGuideOn(eIcon.IconViewOnOff);
                });

            
        }

        // 아이콘 뷰가 움직인 후 추가적인 처리가 필요하면 sequence에 추가
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
            // 아이콘 락이 존재할 경우 다음을 실행
            if(iconDict[choice].iconLock != null)
            {
                Transform iconLockTrans = iconDict[choice].iconLock.transform;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(iconLockTrans.DOScale(Vector3.one * 2f, 0.3f))
                        .Append(iconLockTrans.DOScale(Vector3.zero, 1f))
                        .AppendCallback(() => { Destroy(iconDict[choice].iconLock); })
                        .SetLoops(1);

                // 아이콘이 닫혀있는 경우
                if (isIconViewOpen == false)
                {
                    // IconViewProcess 내부에서 sequence를 추가하여 트위닝 시작함
                    PlaySequnce_IconViewProcess(Center_anchoredPos, true, sequence);
                }
                // 아이콘이 열려있는 경우
                else
                {
                    sequence.Play();
                }

                OpenedIconCount++;
                return true;
            }
            else
            {
                Debug.Log("아이콘 락이 이미 소멸했음");
                return false;
            }
            
        }
        else
        {
            Debug.LogWarning("아이콘 객체에 접근할 수 없음");
            return false;
        }
        
        
    }

    public void TryClickGuideOn(eIcon choice)
    {
        if (isIconViewOpen == false) // 아이콘 뷰가 닫혀있을 때
        {
            foreach(eIcon key in iconDict.Keys)
            {
                if (key == eIcon.IconViewOnOff) continue;

                // 하나라도 켜진 알람이 있으면
                if (iconDict[key].clickGuide.activeSelf)
                {
                    // 아이콘뷰 오픈을 유도
                    iconDict[eIcon.IconViewOnOff].clickGuide.gameObject.SetActive(true);

                    break; // 반복문 탈출
                }
            }
        }

        // 온오프를 호출한 경우 무시함
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
                    // 닫혀있다면 가이드를 꺼야하는지 다시한번 확인
                    if(isIconViewOpen == false)
                    {
                        foreach (eIcon key in iconDict.Keys)
                        {
                            if (key == eIcon.IconViewOnOff) continue;
                            if (iconDict[key].clickGuide.activeSelf) // 하나라도 켜져있다면 온오프의 가이드도 유지해야함
                                return;
                        }
                    }
                    
                }
                break;

            case eIcon.Quest:
                {
                    // 모든 퀘스트를 확인한 경우 &&
                    // 모든 완료된 퀘스트의 보상을 받은 경우
                    foreach (sQuest quest in QuestManager.questHashSet)
                    {
                        cQuestInfo questInfo = CsvManager.Instance.GetQuestInfo(quest.type);
                        if(questInfo.isNeedCheck == true) // 하나라도 true가 있다면 비활성화 하지 않고 함수 종료
                        {
                            return;
                        }
                    }
                }
                break;

            case eIcon.Inventory:
                {
                    // 모든 획득한 아이템을 확인한 경우

                }
                break;

            case eIcon.GameAssistant:
                {
                    // 플레이어 공격차례에 공격대상을 선택 완료한 경우
                }
                break;

            case eIcon.Message:
                {

                }
                break;
        }

        // 활성화 되어있으면 비활성화
        iconDict[choice].clickGuide.gameObject.SetActive(false);

        // 다른 아이콘뷰의 확인을 마친경우에만 실행
        if (choice != eIcon.IconViewOnOff)
        {
            // 아이콘뷰가 닫혀있을 때 확인을 끝낸 경우 아이콘뷰onOff에 대한 확인을 재검토
            TryClickGuideOff(eIcon.IconViewOnOff);
        }
            
    }
}
