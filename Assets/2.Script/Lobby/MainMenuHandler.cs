using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] Button _createGameBtn;
    [SerializeField] Button _joinGameBtn;
    [SerializeField] Button _optionBtn;
    [SerializeField] Button _exitBtn;

    void Awake()
    {
        _createGameBtn.onClick.AddListener(OnClickCreateGame);
        _joinGameBtn.onClick.AddListener(OnClickJoinGame);
        _optionBtn.onClick.AddListener(OnClickOption);
        _exitBtn.onClick.AddListener(OnClickExit);
    }

    void OnClickCreateGame()
    {
        if (LobbyManager.Instance.CurrentState != LobbyState.MainMenu) return;
        LobbyManager.Instance.ChangeState(LobbyState.CreateRoom);
    }

    void OnClickJoinGame()
    {
        if (LobbyManager.Instance.CurrentState != LobbyState.MainMenu) return;
        LobbyManager.Instance.ChangeState(LobbyState.SearchRoom);
    }

    void OnClickOption()
    {
        if (LobbyManager.Instance.CurrentState != LobbyState.MainMenu) return;
        // TODO 옵션 설정 보여줘야함(비디오설정, 마우스설정, 조작키 등)
    }

    void OnClickExit()
    {
        if (LobbyManager.Instance.CurrentState != LobbyState.MainMenu) return;
        // TODO 정말 종료하시겠습니까 패널 나오고 확인 취소로 동작하게 만들기
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}