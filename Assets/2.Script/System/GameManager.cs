using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("# Reference Data")]
    public Transform InteractableT;
    public bool DebugMode;
}