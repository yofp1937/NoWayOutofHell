using System.Collections;
using System.Collections.Generic;
using Game.Lobby;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomSearchHandler : MonoBehaviour
{
    List<RoomInfo> _cashedRoomList; // Photon에서 받아온 roomList
    List<RoomInfoGroup> _roomList; // 표시중인 roomList(새로고침할때마다 최신화된 상태를 유지)
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
        // TODO - 5초에 한번씩 누를수있게 변경
    }

    void OnClickJoinRoom()
    {
        // TODO - target Room으로 입장
        _netManager.JoinRoom(_target.RoomInfo.Name);
        // ChangeState InRoom

        // 패스워드 필요하면 패스워드 입력창이 떠야함
        // 방제목 더블클릭으로도 접속이 가능하게 만들기
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
        if (_roomList == null)
            _roomList = new List<RoomInfoGroup>();

        // 1. 현재 _roomList 기준으로 표시되고있는 방 이름 저장
        Dictionary<string, RoomInfoGroup> currentRoomDict = new Dictionary<string, RoomInfoGroup>();
        foreach (RoomInfoGroup room in _roomList)
        {
            currentRoomDict[room.RoomInfo.Name] = room;
        }

        // 2. 덮어쓰기용 새로운 List 준비
        List<RoomInfoGroup> newRoomList = new List<RoomInfoGroup>();

        // 3. _cashedRoomList 기준으로 UI를 업데이트
        foreach (RoomInfo roomInfo in _cashedRoomList)
        {
            RoomInfoGroup group;

            // 이미 화면에 있는 방이면 업데이트
            if (currentRoomDict.TryGetValue(roomInfo.Name, out group))
            {
                group.Init(roomInfo, this); // 정보 갱신
                currentRoomDict.Remove(roomInfo.Name); // Dict에서 수정 완료된 데이터 제거
            }
            else // 새로 생긴 방이면 PoolManager를 통해 생성
            {
                GameObject obj = PoolManager.Instance.Get(PoolManager.Instance.RoomInfoGroup);
                obj.transform.SetParent(_scrollContentT);

                group = obj.GetComponent<RoomInfoGroup>();
                group.Init(roomInfo, this);
            }
            newRoomList.Add(group);
        }

        // 4. 화면에 있었지만 사라진 방은 제거
        foreach (RoomInfoGroup room in currentRoomDict.Values)
        {
            room.gameObject.SetActive(false);
        }
        _roomList = newRoomList;
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
