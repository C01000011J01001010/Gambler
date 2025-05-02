using DG.Tweening;
using PublicSet;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TargetButton : Selection_ButtonBase<TargetButton>
{
    private CardGamePlayerBase mPlayer;


    protected override void OnDisable()
    {
        // ���� �ѵ� ȭ���� �����Ǿ����
    }

    public void SetPlayer(CardGamePlayerBase player)
    {
        mPlayer = player;
        TryChangePortraitImage();
    }

    public bool ComparePlayer(object player)
    {
        if(player != null && mPlayer == (player as CardGamePlayerBase))
        {
            return true;
        }
        return false;
    }

    public bool TryChangePortraitImage()
    {
        bool isSueccessed = false;
        Sprite sprite = PortraitImageResource.Instance.TryGetImage(mPlayer.characterInfo.CharaterIndex, out isSueccessed);
        if (isSueccessed)
        {
            image.sprite = sprite;

            Debug.Log("�̹��� ��ȯ ����");
        }
        else
        {
            Debug.LogAssertion("�̹��� ��ȯ ����");
        }

        return isSueccessed;
    }

    public void SelectPlayer()
    {
        bool result = CardGamePlayManager.Instance.playerMe.TrySetAttackTarget(mPlayer);
        CardGamePlayManager.Instance.cardGameView.selectCompleteButton.TryActivate_Button();
    }

    public void InitCallback(UnityAction plusCallback)
    {
        SetCallback(this, 
            ()=>
            { 
                SelectPlayer();
                if (plusCallback != null)
                    plusCallback();
            });
    }

    public void PlayerReadyToPlay()
    {
        SetBackruptMark(false);
        TryActivate_Button();
    }

    public void PlayerBankrupt()
    {
        SetBackruptMark(true);
        TryDeactivate_Button();
    }

    private void SetBackruptMark(bool value)
    {
        if (value)
        {
            

            float delay = 0.5f;
            MethodManager.PlaySequence_FadeIn(buttonText.gameObject, delay);
        }
        else
        {
            buttonText.gameObject.SetActive(value);
        }    
        
    }
}
