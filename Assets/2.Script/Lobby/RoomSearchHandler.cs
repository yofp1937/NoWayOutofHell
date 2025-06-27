using System.Collections;
using System.Collections.Generic;
using Game.Lobby;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomSearchHandler : MonoBehaviour
{
    List<RoomInfo> _cashedRoomList; // Photon에서 받아온 roomList
    List<RoomInfoGroup> _roomList; // 표시중인 roomList
    RoomInfoGroup _target;

    [SerializeField] Transform _scrollContentT;
    [SerializeField] Button _refreshButton;
    [SerializeField] Button _joinButton;
    [SerializeField] Button _exitButton;

    NetworkManager _netManager;

    // TODO
    // 1.방 클릭시 _targetRoom이 해당 방의 정보로 변경됨
    // 2._refereshButton 클릭시 NetworkManager에서 방정보 새로 불러옴
    // 3._joinButton 클릭시 _targetRoom으로 입장
    // 4._exitButton 클릭시 메인메뉴로 이동

    // content 안에있는 RoomInfo Group이 클릭될시
    // 1.Image를 활성화(선택된 방이 표시되게)
    // 2._targetRoom이 설정됨


    void Awake()
    {
        _refreshButton.onClick.AddListener(OnClickRefresh);
        _joinButton.onClick.AddListener(OnClickJoinRoom);
        _exitButton.onClick.AddListener(OnClickExit);

        _netManager = GameManager.Instance.NetworkManager;
    }

    public void GetRoomListAndUpdateUI()
    {
        if (_netManager == null) _netManager = GameManager.Instance.NetworkManager;

        _cashedRoomList = _netManager.GetRoomList();
        SetRoomList();
    }

    void OnClickRefresh()
    {
        GetRoomListAndUpdateUI();
    }

    void OnClickJoinRoom()
    {
        // TODO - target Room으로 입장
        // ChangeState InRoom
        GameObject obj = PoolManager.Instance.Get(PoolManager.Instance.RoomInfoGroup);
        obj.transform.SetParent(_scrollContentT);
        obj.GetComponent<RoomInfoGroup>().Init(null, this);
    }

    void OnClickExit()
    {
        LobbyManager.Instance.ChangeState(LobbyState.MainMenu);
    }

    void SetRoomList()
    {
        // 1._cahsedRoomList와 _roomList의 방이름을 비교해가며 수정 사항을 업데이트함
        // 2._cahsed엔 있는데 _roomList엔 없는 방이 존재하면 PoolManager를 통해 RoomInfo Group을 반환받음
        // 3.RoomInfo Group에 데이터 세팅함
    }

    public void ChangeTarget(RoomInfoGroup target)
    {
        if (_target != null)
        {
            _target.SetImageEnable(false);
        }
        _target = target;
    }
}
