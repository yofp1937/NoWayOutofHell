using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateGameHandler : MonoBehaviour
{
    int _maxPlayer = 4;

    [SerializeField] TMP_InputField _roomNameField;
    [SerializeField] TMP_InputField _passwordField;
    [SerializeField] TMP_Dropdown _dropdown;
    [SerializeField] Button _createBtn;
    [SerializeField] Button _cancleBtn;

    void Awake()
    {
        _dropdown.onValueChanged.AddListener(OnChangedDropdown);
        _createBtn.onClick.AddListener(OnClickCreateBtn);
        _cancleBtn.onClick.AddListener(OnClickCancleBtn);
    }

    public void OnChangedDropdown(int value)
    {
        string selected = _dropdown.options[value].text;

        if (int.TryParse(selected, out int result))
        {
            _maxPlayer = result;
        }
        Debug.Log($"maxPlayer = {_maxPlayer}");
    }

    public void OnClickCreateBtn()
    {
        NetworkManager netManager = GameManager.Instance.NetworkManager;


        string roomName = _roomNameField.text.Trim();
        if (roomName == "")
        {
            roomName = netManager.GetNickname() + "님의 방";
        }

        string password = _passwordField.text.Trim();

        netManager.CreateRoom(roomName, password, _maxPlayer);
    }

    public void OnClickCancleBtn()
    {
        _roomNameField.text = "";
        _passwordField.text = "";

        _dropdown.value = 0;

        LobbyManager.Instance.ChangeState(LobbyState.MainMenu);
    }
}