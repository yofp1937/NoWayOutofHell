using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameRoomHandler : MonoBehaviour
{
    [Header("# Room Info")]
    [SerializeField] TextMeshProUGUI _roomName;
    [SerializeField] TextMeshProUGUI _playerText;
    [SerializeField] Image _lock;
    [SerializeField] TextMeshProUGUI _roomLeader;

    [Header("# Connect Player")]
    [SerializeField] GameObject[] _connectPlayerUI;
    Dictionary<int, Photon.Realtime.Player> PlayerDict;

    // [Header("# Chat")]
    // 채팅은 따로 스크립트 생성하고 참조하는게 나아보임

    [Header("# Button")]
    [SerializeField] Button _startBtn;
    [SerializeField] Button _exitBtn;

    [Header("# ETC")]
    NetworkManager _netManager;

    // TODO
    // 1.방이 생성되면 방 제목, 방장, MaxPlayer, 자물쇠 세팅
    // 2.접속중인 유저에도 1번 플레이어 등록
    // 3.게임준비/시작 버튼에 리스너 연결(방장이면 모두가 레디중일때 누르면 겜시작, 파티원이면 준비했다 풀었다 가능)
    // 4.나가기 누를시 방 나가고 MainMenu로 이동

    // 타인 접속시 PlayerText, 접속중인 유저 업데이트해야함

    void Awake()
    {
        _netManager = GameManager.Instance.NetworkManager;

        _startBtn.onClick.AddListener(OnClickGameStart);
        _exitBtn.onClick.AddListener(OnClickExit);
    }

    /// <summary>
    /// 방 생성시 호출
    /// </summary>
    /// <param name="data">방의 정보</param>
    /// <param name="player">방장 정보</param>
    public void Init(Room data)
    {
        if (_netManager == null) _netManager = GameManager.Instance.NetworkManager;
        if (!_netManager.IsConnected) return;

        string roomName = data.Name;
        bool isLock = data.CustomProperties.ContainsKey("needPassword");
        string roomLeader = data.CustomProperties.ContainsKey("host") ? data.CustomProperties["host"].ToString() : "Unknown";
        int maxPlayer = data.MaxPlayers;

        PlayerDict = new Dictionary<int, Photon.Realtime.Player>();
        foreach (var kvp in data.Players)
        {
            Debug.Log($"kvp: {kvp}, Key: {kvp.Key}, Value: {kvp.Value}");
            PlayerDict.Add(kvp.Key, kvp.Value);
        }

        // 닉네임칸 초기화
        foreach (GameObject connectPlayer in _connectPlayerUI)
        {
            TextMeshProUGUI nickText = connectPlayer.GetComponentInChildren<TextMeshProUGUI>();
            nickText.text = "";
        }
        gameObject.SetActive(true);

        SetUI(roomName, isLock, roomLeader, maxPlayer);

        NetworkManager.OnPlayerEntered += UpdateRoomUI;
        NetworkManager.OnPlayerLeft += UpdateRoomUI;
    }

    void OnDisable()
    {
        NetworkManager.OnPlayerEntered -= UpdateRoomUI;
        NetworkManager.OnPlayerLeft -= UpdateRoomUI;
        Debug.Log("Action 해제 완료");
    }

    /// <summary>
    /// 방 접속시 초기 UI 세팅
    /// </summary>
    public void SetUI(string roomName, bool isLock, string roomLeader, int maxPlayer)
    {
        _roomName.text = roomName;

        if (isLock)
        {
            _lock.gameObject.SetActive(true);
        }
        else
        {
            _lock.gameObject.SetActive(false);
        }

        _roomLeader.text = roomLeader;

        InitConnectPlayerUI();
    }

    void UpdateRoomUI()
    {
        UpdateConnectPlayerUI();
    }

    /// <summary>
    /// 접속중인 플레이어 수 업데이트
    /// </summary>
    void UpdateConnectPlayerCountText()
    {
        Room data = _netManager.GetCurrentRoomData();

        string text = "( ";
        text += data.PlayerCount;
        text += " / ";
        text += data.MaxPlayers;
        text += " )";

        _playerText.text = text;
    }

    void InitConnectPlayerUI()
    {
        // 접속중인 유저들의 목록을 받아옴
        Room data = _netManager.GetCurrentRoomData();
        Dictionary<int, Photon.Realtime.Player> dict = new Dictionary<int, Photon.Realtime.Player>(data.Players);

        // 닉네임 세팅 반복문 시작
        foreach (GameObject connectPlayer in _connectPlayerUI)
        {
            // 세팅할 player가 존재하지 않으면 반복문 종료
            if (0 >= dict.Count) break;

            TextMeshProUGUI nickText = connectPlayer.GetComponentInChildren<TextMeshProUGUI>();

            // 닉네임이 공백으로 설정돼있으면(비어있는 칸이면) 닉네임 세팅
            if (string.IsNullOrEmpty(nickText.text))
            {
                // 열거자를 사용하여 Dictionary의 첫번째 요소를 가져와서 사용
                var enumerator = dict.GetEnumerator();
                enumerator.MoveNext();
                nickText.text = enumerator.Current.Value.NickName;
                // 사용한 값 Dictionary에서 제거
                dict.Remove(enumerator.Current.Key);
            }
        }
        UpdateConnectPlayerCountText();
    }

    /// <summary>
    /// 접속중인 플레이어 닉네임 업데이트
    /// </summary>
    void UpdateConnectPlayerUI()
    {
        // 접속중인 유저들의 목록을 받아옴
        Room data = _netManager.GetCurrentRoomData();
        Dictionary<int, Photon.Realtime.Player> dict = new Dictionary<int, Photon.Realtime.Player>(data.Players);

        // _connectPlayerUI 배열을 List로 복사(플레이어 확인용)
        List<GameObject> connectPlayerList = new List<GameObject>(_connectPlayerUI);

        // 1.PlayerUI를 순회하며 닉네임 확인
        foreach (var player in dict)
        {
            string nickname = player.Value.NickName;
            bool isFound = false;

            // 복사한 리스트 순회
            foreach (GameObject connectPlayer in connectPlayerList)
            {
                TextMeshProUGUI nickText = connectPlayer.GetComponentInChildren<TextMeshProUGUI>();

                // 동일 닉네임을 찾으면 해당 UI를 리스트에서 제거(중복닉 오류 방지)
                if (nickText != null && nickText.text == nickname)
                {
                    connectPlayerList.Remove(connectPlayer);
                    isFound = true;
                    break;
                }
            }

            // 2.닉네임이 UI에 존재하지 않을경우 새로 입장한 유저이므로 UI에 추가
            if (!isFound)
            {
                foreach (GameObject connectPlayer in connectPlayerList)
                {
                    TextMeshProUGUI nickText = connectPlayer.GetComponentInChildren<TextMeshProUGUI>();

                    if (nickText != null && string.IsNullOrEmpty(nickText.text))
                    {
                        nickText.text = nickname;
                        connectPlayerList.Remove(connectPlayer);
                        break;
                    }
                }
            }

            // 3. 이후 남은 connectPlayerUI는 공백처리(나간 유저 처리)
            foreach (GameObject connectPlayer in connectPlayerList)
            {
                TextMeshProUGUI nickText = connectPlayer.GetComponentInChildren<TextMeshProUGUI>();

                if (nickText != null && !string.IsNullOrEmpty(nickText.text))
                {
                    nickText.text = "";
                }
            }
        }
        UpdateConnectPlayerCountText();
    }

    /// <summary>
    /// 접속중인 플레이어의 게임준비 신호 업데이트
    /// </summary>
    public void UpdateConnectPlayerReadyState()
    {

    }

    void OnClickGameStart()
    {
        Room data = _netManager.GetCurrentRoomData();
        Debug.Log($"[GameStartBtn]: {data}");
        foreach (var p in data.Players)
        {
            Debug.Log($"[CurrentRoom.Players]: {p.Value}");
        }

        Dictionary<int, Photon.Realtime.Player> player = PlayerDict;
        if (player != null)
        {
            foreach (var p in player)
            {
                Debug.Log($"[GameStartBtn]: player - {p.Value}");
            }
        }
    }

    void OnClickExit()
    {
        _netManager.LeaveRoom();
    }
}
