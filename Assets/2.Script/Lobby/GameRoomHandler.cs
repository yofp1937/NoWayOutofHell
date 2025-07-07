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
    int _currentPlayer;
    int _maxPlayer;

    [Header("# Connect Player")]
    [SerializeField] GameObject[] _connectPlayers;

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
        _currentPlayer = 0;

        string roomName = data.Name;
        bool isLock = data.CustomProperties.ContainsKey("needPassword");
        string roomLeader = data.CustomProperties.ContainsKey("host") ? data.CustomProperties["host"].ToString() : "Unknown";
        int maxPlayer = data.MaxPlayers;

        // 닉네임칸 초기화
        foreach (GameObject connectPlayer in _connectPlayers)
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
        _maxPlayer = maxPlayer;

        UpdateRoomUI(null);
    }

    void UpdateRoomUI(Photon.Realtime.Player player)
    {
        UpdateConnectPlayerUI(player);
    }

    /// <summary>
    /// 접속중인 플레이어 수 업데이트
    /// </summary>
    void UpdateConnectPlayerCountText()
    {
        string text = "( ";
        text += _currentPlayer;
        text += " / ";
        text += _maxPlayer;
        text += " )";

        _playerText.text = text;
    }

    /// <summary>
    /// 접속중인 플레이어 닉네임 업데이트
    /// </summary>
    void UpdateConnectPlayerUI(Photon.Realtime.Player player)
    {
        // 1. player가 null이면 초기 방생성이므로 접속중인 모든 플레이어를 표시해야함
        if (player == null)
        {
            // 접속중인 유저들의 목록을 받아옴
            Dictionary<int, Photon.Realtime.Player> dict = _netManager.GetPlayersInCurrentRoom();

            // 닉네임 세팅 반복문 시작
            foreach (GameObject connectPlayer in _connectPlayers)
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
                    _currentPlayer++;
                }
            }
        }
        // 2. player 값이 존재하면 해당 플레이어의 닉네임이 _connectPlayer에 존재하는지 확인
        //    동일한 닉네임 존재 - player가 방을 나간것
        //    동일한 닉네임 없음 - player가 방에 입장한것
        else
        {
            string nickname = player.NickName;
            bool nickExists = false;

            // 동일한 닉네임이 존재하는지 확인
            foreach (GameObject connectPlayer in _connectPlayers)
            {
                TextMeshProUGUI nickText = connectPlayer.GetComponentInChildren<TextMeshProUGUI>();

                // 동일한 닉네임 존재하면(Player가 나간것이므로 닉네임 공백으로 변경)
                if (nickText.text == nickname)
                {
                    nickText.text = "";
                    nickExists = true;
                    _currentPlayer--;
                    break;
                }
            }

            // 닉네임이 존재하지 않으면(Player가 입장한것이므로 닉네임 추가)
            if (!nickExists)
            {
                foreach (GameObject connectPlayer in _connectPlayers)
                {
                    TextMeshProUGUI nickText = connectPlayer.GetComponentInChildren<TextMeshProUGUI>();

                    // 빈칸에 닉네임 추가
                    if (string.IsNullOrEmpty(nickText.text))
                    {
                        nickText.text = nickname;
                        _currentPlayer++;
                        break;
                    }
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

    }

    void OnClickExit()
    {
        _netManager.LeaveRoom();
    }
}
