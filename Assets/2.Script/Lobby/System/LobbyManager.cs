using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lobby 상태 흐름 관리용
public enum LobbyState
{
    EnterNickname, // 1. 닉네임 입력
    MainMenu, // 2. 메인 메뉴 화면
    CreateRoom, // 3. 방 만들기
    SearchRoom, // 4. 방 참여하기
    InRoom // 5. 방에 입장한 상태
}

public class LobbyManager : Singleton<LobbyManager>
{
    public LobbyState CurrentState; // 현재 상태 확인

    void Start()
    {
        EnterState(LobbyState.EnterNickname);
    }

    public void ChangeState(LobbyState newState)
    {
        ExitState(CurrentState);
        CurrentState = newState;
        EnterState(newState);
    }

    void EnterState(LobbyState state)
    {
        switch (state)
        {
            case LobbyState.EnterNickname:
                Debug.Log("[EnterState]: 닉네임 입력 시작");
                // TODO - UIManager의 닉네임 입력 메서드 실행
                UIManager.Instance.SetActiveNickname(true);
                break;
            case LobbyState.MainMenu:
                Debug.Log("[EnterState]: 메인메뉴 입장");
                // TODO - UIManager의 메인화면 표시 메서드 실행
                // 이때 네트워크 접속이 안돼있을시 싱글로 플레이할지 확인받고 싱글로 한다하면 다시 네트워크 검사 x(네트워크 수동 접속 버튼만 만들어놓기)
                break;
            case LobbyState.CreateRoom:
                Debug.Log("[EnterState]: 방 생성 입장");
                // TODO - 서버와 연결 안돼있으면 경고문구 표시(방을 만들어도 다른사람이 볼수없으며 싱글플레이로만 가능하다)
                UIManager.Instance.SetActiveCreateGame(true);
                break;
            case LobbyState.SearchRoom:
                Debug.Log("[EnterState]: 방 참여하기 입장");
                // TODO - 서버와 연결 안돼있으면 경고문구 표시(서버와 연결되지않아 방을 검색할수없다)
                UIManager.Instance.SetActiveRoomSearch(true);
                // 1.방 만들기 or 방 검색
                // 2.방 접속시 캐릭터 색상 선택, 맵 선택
                // 3.채팅도 가능하고 모두가 준비완료해야 시작할수있음(방장은 플레이어 강퇴도 가능)
                break;
            case LobbyState.InRoom:
                Debug.Log("[EnterState]: 방에 참여함");
                // TODO 서버에 GameRoom 동기화 요청
                break;
        }
    }

    void ExitState(LobbyState state)
    {
        switch (state)
        {
            case LobbyState.EnterNickname:
                // 닉네임 패널이 점차 사라져야함
                Debug.Log("닉네임 입력 종료");
                UIManager.Instance.SetActiveNickname(false);
                UIManager.Instance.BrightenBackground();
                break;
            case LobbyState.MainMenu:
                Debug.Log("메인메뉴 종료");
                break;
            case LobbyState.CreateRoom:
                Debug.Log("방 만들기 종료");
                UIManager.Instance.SetActiveCreateGame(false);
                break;
            case LobbyState.SearchRoom:
                Debug.Log("방 참여하기 종료");
                UIManager.Instance.SetActiveRoomSearch(false);
                break;
            case LobbyState.InRoom:
                Debug.Log("방 나갔음");
                GameManager.Instance.NetworkManager.LeaveRoom();
                UIManager.Instance.SetActiveGameRoom(false);
                // TODO NetworkManager에게 방 나가기 요청
                break;
        }
    }
}
