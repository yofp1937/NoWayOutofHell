using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using System;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI StateText;

    public bool IsConnected; // 네트워크 연결 상태 확인

    List<RoomInfo> _cachedRoomList = new List<RoomInfo>(); // 이전에 불러왔던 방 리스트들

    public static event Action<Photon.Realtime.Player> OnPlayerEntered;
    public static event Action<Photon.Realtime.Player> OnPlayerLeft;

    void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
    }

    void Update()
    {
        StateText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    /// <summary>
    /// PhotonNetwork에 닉네임 등록
    /// </summary>
    public void SetNickname(string nickname)
    {
        PhotonNetwork.LocalPlayer.NickName = nickname;
    }

    /// <summary>
    /// PhotonNetwork에 등록된 닉네임 받아오기
    /// </summary>
    public string GetNickname()
    {
        return PhotonNetwork.LocalPlayer.NickName;
    }

    /// <summary>
    /// 서버 연결
    /// </summary>
    public void ConncetToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// ConnectToServer의 콜백함수
    /// </summary>
    public override void OnConnectedToMaster()
    {
        print($"{PhotonNetwork.LocalPlayer.NickName} 서버 접속 완료");
        IsConnected = true;

        // 로비로 접속해야 만들어져있는 방의 갯수를 확인할수있기에 바로 Lobby로 접속
        JoinLobby();
    }

    /// <summary>
    /// 서버 연결 해제
    /// </summary>
    public void DisconnectToServer()
    {
        PhotonNetwork.Disconnect();
    }

    /// <summary>
    /// DisconnectToServer의 콜백함수
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause)
    {
        print($"{PhotonNetwork.LocalPlayer.NickName} 서버 접속 해제 완료");
        IsConnected = false;
    }

    void JoinLobby()
    {
        // 로비는 4가지 기준으로 다른 로비를 만들 수 있다.
        // 1:로비 이름, 2:로비 타입, 3:게임 버전, 4:지역

        // 1번과 2번은 아래 주석처럼 TypedLobby를 생성해서 첫번째 인자에 로비 이름, 2번째 인자에 로비 타입을 넣어서 정할수있다.
        // TypedLobby test = new TypedLobby("로비이름내맘대로", LobbyType.Default);

        // 2번 로비 타입의 경우에는 LobbyType.Default, LobbyType.SqlLobby, LobbyType.AsyncRandomLobby 3가지가 존재하는데
        // Default의 경우에는 만들어진 방 목록을 확인할수있지만 검색을 통해 방을 필터링하는 기능은 없다.(모든 방 확인)
        // SqlLobby의 경우에는 방 목록도 확인이 가능하고, Sql-like 조건으로 검색을통해 방을 필터링할수있다.(내가 원하는 방 확인)
        // AsyncRandomLobby의 경우는 방 목록을 확인할수없고 오직 랜덤으로 방에 입장할수있는 기능만 존재한다(빠른 무작위 매칭)
        // 세가지 모두 JoinRandomRoom을 통해 랜덤 방에 매칭은 가능하다

        // 3번 게임 버전의 경우는 마스터서버에 연결하기전 PhotonNetwork.GameVersion = Application.version;을 통해
        // Photon의 게임버전을 현 게임버전과 동기화시켜주면 같은 게임버전의 유저들끼리만 만날수있음

        // 4번 지역의 경우는 Photon에서 마스터서버에 연결할때 자동으로 가까운 region을 선택해 입장함
        // 하지만 PhotonNetwork.ConnectToRegion(string region) 메서드를 사용하여 내가 원하는 지역으로 입장 가능
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("로비 접속 완료");
    }

    public void CreateRoom(string roomName, string password, int maxPlayer) // 방 생성
    {
        RoomOptions roomOptions = new RoomOptions();
        Hashtable hash = new Hashtable();

        roomOptions.MaxPlayers = maxPlayer;
        roomOptions.IsVisible = true;

        hash["host"] = PhotonNetwork.LocalPlayer.NickName;

        // password는 Trim을 실행하고 전송된값임
        // password가 존재하면 CustomProperties에 password 설정
        if (password != "")
        {
            hash["password"] = password;
            hash["needPassword"] = true;
        }

        // CustomRoomProperties는 방에 참가후 내부에서 확인할수있는 정보
        // CustomRoomPropertiesForLobby는 로비에서 확인할수있는 정보 제공용 속성
        roomOptions.CustomRoomProperties = hash;

        roomOptions.CustomRoomPropertiesForLobby = new string[] { "host", "needPassword" }; // 패스워드는 로비에서 확인 불가하게 설정

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void JoinRoom(string roomName)
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("이미 방에 참가중입니다.");
            return;
        }

        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinRoom(roomName);
            LobbyManager.Instance.ChangeState(LobbyState.InRoom);
        }
        else
        {
            Debug.Log("로비에 입장해주세요.");
        }
    }

    public void LeaveRoom()
    {
        if (!IsConnected || !PhotonNetwork.InRoom)
        {
            Debug.Log("방을 나갈수 없음");
            return;
        }

        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        LobbyManager.Instance.ChangeState(LobbyState.MainMenu);
    }

    public override void OnCreatedRoom()
    {
        string roomName = PhotonNetwork.CurrentRoom.Name;
        int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        string myNickname = PhotonNetwork.LocalPlayer.NickName;

        print($"방 생성 완료");
        print($"방 제목: {roomName}");
        print($"최대 인원: {maxPlayers}");
        print($"참가자: {myNickname}");

        LobbyManager.Instance.ChangeState(LobbyState.InRoom);
    }

    public override void OnJoinedRoom()
    {
        print("방 참가 완료");
        UIManager.Instance.GameRoomPanel.GetComponent<GameRoomHandler>().Init(PhotonNetwork.CurrentRoom);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("방 생성 실패");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("방 참가 실패");
    }

    public List<RoomInfo> GetRoomList()
    {
        return _cachedRoomList;
    }

    /// <summary>
    /// Photon에서 누군가 방을 만들거나, 삭제하거나, 인원이 바뀔때마다 실시간으로 콜백해주는 함수
    /// 로비에 접속할때도 호출
    /// </summary>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // roomList에는 모든방 말고 데이터가 변경된 방들만 담겨서 넘어옴
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) // 해당 방이 사라져야하는지 검사(IsVisible이 flase로 바뀌었는지, CurrentPlayer가 0이 됐는지 등)
            {
                _cachedRoomList.RemoveAll(r => r.Name == roomInfo.Name); // 사라져야하면 리스트에서 데이터 전부 제거
            }
            else
            {
                int index = _cachedRoomList.FindIndex(r => r.Name == roomInfo.Name); // cachedroomList에 roomInfo와 같은 이름의 방이 있는지 검사
                if (index == -1) // 없다면
                {
                    _cachedRoomList.Add(roomInfo); // cahcedRoomList에 추가
                }
                else // 있다면
                {
                    _cachedRoomList[index] = roomInfo; // 데이터 업데이트
                }
            }
        }
    }

    public Room GetCurrentRoomData()
    {
        // Debug.Log(PhotonNetwork.CurrentRoom);
        return PhotonNetwork.CurrentRoom;
    }

    public Dictionary<int, Photon.Realtime.Player> GetPlayersInCurrentRoom()
    {
        return PhotonNetwork.CurrentRoom.Players;
    }

    /// <summary>
    /// 다른 플레이어가 방에 참가했을때 호출
    /// </summary>
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // 방 UI 업데이트
        OnPlayerEntered?.Invoke(newPlayer);
    }

    /// <summary>
    /// 다른 플레이어가 방을 떠났을때 호출
    /// </summary>
    public override void OnPlayerLeftRoom(Photon.Realtime.Player leavedPlayer)
    {
        // 방 UI 업데이트
        OnPlayerLeft?.Invoke(leavedPlayer);
    }
}
