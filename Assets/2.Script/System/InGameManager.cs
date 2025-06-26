using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoSingleton<InGameManager>
{
    [Header("# Reference Data")]
    public Transform InteractableT;
    public bool DebugMode;
}