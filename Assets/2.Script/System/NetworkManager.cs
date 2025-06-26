using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI StateText;

    public bool IsConnected; // 네트워크 연결 상태 확인

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

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("로비 접속 완료");
    }

    public void CreateRoom(string roomName, int maxPlayer) // 방 생성
    {
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayer });
    }

    public void JoinRoom()
    {
        // PhotonNetwork.JoinRoom(RoomInput.text);
    }

    public void JoinOrCreateRoom()
    {
        // PhotonNetwork.JoinOrCreateRoom(RoomInput.text, new RoomOptions { MaxPlayers = 4 }, null);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
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

        // TODO
        // 1.방참여로 LobbyManager의 StateChange
        LobbyManager.Instance.ChangeState(LobbyState.InRoom);
        // 2.UIManager에서 방 로비 불러오기
    }

    public override void OnJoinedRoom()
    {
        print("방 참가 완료");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("방 생성 실패");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("방 참가 실패");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("방 랜덤참가 실패");
    }
}
