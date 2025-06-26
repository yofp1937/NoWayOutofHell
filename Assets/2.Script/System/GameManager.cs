using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public NetworkManager NetworkManager;

    void Awake()
    {
        NetworkManager = GetComponentInChildren<NetworkManager>();
    }
}
