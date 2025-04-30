using PublicSet;
using UnityEngine;
using UnityEngine.Events;

public class TargetButton : Selection_ButtonBase<TargetButton>
{
    private CardGamePlayerBase player;


    public void SetPlayer(CardGamePlayerBase player)
    {
        this.player = player;
        button.interactable = true;
        TryChangePortraitImage();
    }

    public bool TryChangePortraitImage()
    {
        bool isSueccessed = false;
        Sprite sprite = PortraitImageResource.Instance.TryGetImage(player.characterInfo.CharaterIndex, out isSueccessed);
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
        bool result = CardGamePlayManager.Instance.playerMe.TrySetAttackTarget(player);
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

    
}
