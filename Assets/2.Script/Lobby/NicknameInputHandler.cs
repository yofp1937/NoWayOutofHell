using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NicknameInputHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] Button _button;
    [SerializeField] TextMeshProUGUI _text;

    void Awake()
    {
        _button.onClick.AddListener(OnClickButton);
        _text.text = "";
    }

    /// <summary>
    /// 닉네임을 읽어와서 공백이면 return
    /// </summary>
    void OnClickButton()
    {
        string nickname = _inputField.text.Trim();

        // 공백이면 return
        if (string.IsNullOrEmpty(nickname))
        {
            _text.text = "닉네임을 입력하세요!";
            _text.color = Color.red;
            return;
        }
        Debug.Log($"닉네임 입력: {nickname}");

        // 메인화면으로 이동 및 네트워크 연결
        LobbyManager.Instance.ChangeState(LobbyState.MainMenu);
        GameManager.Instance.NetworkManager.SetNickname(nickname);
        GameManager.Instance.NetworkManager.ConncetToServer();
    }
}