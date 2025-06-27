using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] Image _background;
    [SerializeField] GameObject _nicknamePanel;
    [SerializeField] GameObject _mainMenuPanel;
    [SerializeField] GameObject _createGamePanel;
    [SerializeField] GameObject _gameRoomPanel;
    [SerializeField] GameObject _roomSearchPanel;

    void Awake()
    {
        _background.color = Color.black;
        _nicknamePanel.SetActive(false);
        _mainMenuPanel.SetActive(false);
        _createGamePanel.SetActive(false);
        _gameRoomPanel.SetActive(false);
        _roomSearchPanel.SetActive(false);
    }

    /// <summary>
    /// Nickname Panel 활성화
    /// </summary>
    public void SetActiveNickname(bool active)
    {
        _nicknamePanel.SetActive(active);
    }

    /// <summary>
    /// 배경이미지 서서히 밝아지게 만들어줌
    /// </summary>
    public void BrightenBackground()
    {
        _background.DOColor(Color.white, 2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            SetActiveMainMenu(true);
        });
    }

    /// <summary>
    /// MainMenu Panel 활성화
    /// </summary>
    public void SetActiveMainMenu(bool active)
    {
        _mainMenuPanel.SetActive(active);
    }

    /// <summary>
    /// CreateGame Panel 활성화
    /// </summary>
    public void SetActiveCreateGame(bool active)
    {
        _createGamePanel.SetActive(active);
    }

    /// <summary>
    /// GameRoom Panel 활성화
    /// </summary>
    public void SetActiveGameRoom(bool active)
    {
        _gameRoomPanel.SetActive(active);
    }

    /// <summary>
    /// RoomSearch Panel 활성화
    /// </summary>
    public void SetActiveRoomSearch(bool active)
    {
        if (active)
        {
            var handler = _roomSearchPanel.GetComponent<RoomSearchHandler>();
            Debug.Log($"{handler}");
            handler.GetRoomListAndUpdateUI();
        }
        _roomSearchPanel.SetActive(active);
    }
}