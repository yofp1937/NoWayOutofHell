using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayManager : Singleton<MultiplayManager>
{
    [Header("# Player Data")]
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] GameObject _playerUICanvasPrefab;

    void Awake()
    {
        GameObject playerObj = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
        GameObject canvasObj = Instantiate(_playerUICanvasPrefab);

        Player player = playerObj.GetComponent<Player>();
        PlayerUI playerUI = player.GetComponent<PlayerUI>();
        PlayerUICanvas canvas = canvasObj.GetComponent<PlayerUICanvas>();

        playerUI.ConnectComponents(canvas);
    }
}
