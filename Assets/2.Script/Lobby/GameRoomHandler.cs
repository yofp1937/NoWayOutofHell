using System.Collections;
using System.Collections.Generic;
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

        UpdateConnectPlayerText();
    }

    /// <summary>
    /// 접속중인 플레이어 수 업데이트
    /// </summary>
    public void UpdateConnectPlayerText()
    {
        string text = "( ";
        text += _netManager.GetCurrentConnectPlayerCountInRoom();
        text += " / ";
        text += _maxPlayer;
        text += " )";

        _playerText.text = text;
    }

    /// <summary>
    /// 접속중인 플레이어 닉네임 업데이트
    /// </summary>
    public void UpdateConnectPlayerUI()
    {

    }

    /// <summary>
    /// 접속중인 플레이어의 게임준비 신호 업데이트
    /// </summary>
    public void UpdateConnectPlayerReadyState()
    {

    }

    public void OnClickExit()
    {
        LobbyManager.Instance.ChangeState(LobbyState.MainMenu);
    }
}
