using UnityEngine;
using UnityEngine.SceneManagement;
using PublicSet;

public class LobbyView : MonoBehaviour
{
    public PopUPView_Lobby popUpView;
    // 새로시작하기 버튼
    public void StartNewGame()
    {
        // 즉시 게임으로 입장
        GameManager.Instance.SetPlayerSaveKey(ePlayerSaveKey.None);
        GameManager.Instance.SceneUnloadView(()=> SceneManager.LoadScene("InGame"));
    }

    public void ContinuePopUpOpen()
    {
        popUpView.ContinuePopUpOpen();
    }

    // 자유모드 -> 최종 플레이 기록을 바탕으로 미니게임 팝업 오픈
    public void FreeModePopUpOpen()
    {

    }

    public void GameSettingPopUpOpen()
    {
        popUpView.GameSettingPopUpOpen();
    }

    // 최고기록 -> 게임종료시 최종금액중 가장높은 금액 3개를 표시
    public void BestScorePopUpOpen()
    {

    }

    public void QuitGamePopUpOpen()
    {

    }
}
