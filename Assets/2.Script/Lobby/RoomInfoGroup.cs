using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfoGroup : MonoBehaviour
{
    [Header("# Main Data")]
    public RoomInfo RoomInfo;

    [Header("# Button")]
    Image _btnImage;
    Button _btn;

    [Header("# Children Data")]
    [SerializeField] Image _lockImage;
    [SerializeField] TextMeshProUGUI _roomNameText;
    [SerializeField] TextMeshProUGUI _hostText;
    [SerializeField] TextMeshProUGUI _playerCountText;

    [Header("# Reference Data")]
    RoomSearchHandler _handler;

    void Awake()
    {
        _btnImage = GetComponent<Image>();
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClickBtn);
    }

    public void Init(RoomInfo roomInfo, RoomSearchHandler handler)
    {
        RoomInfo = roomInfo;
        _handler = handler;

        // 각종 데이터 추출
        string roomName = roomInfo.Name;
        string host = roomInfo.CustomProperties.ContainsKey("host") ? roomInfo.CustomProperties["host"].ToString() : "Unknown";
        bool hasPassword = roomInfo.CustomProperties.ContainsKey("needPassword");
        int playerCount = roomInfo.PlayerCount;
        int maxPlayer = roomInfo.MaxPlayers;

        // 데이터에 맞게 UI 세팅
        _roomNameText.text = roomName;
        _hostText.text = host;
        _lockImage.gameObject.SetActive(hasPassword);
        _playerCountText.text = $"{playerCount} / {maxPlayer}";

        SetImageEnable(false);
    }

    void OnClickBtn()
    {
        _handler.ChangeTarget(this);
        SetImageEnable(true);
    }

    public void SetImageEnable(bool active)
    {
        _btnImage.enabled = active;
    }
}
