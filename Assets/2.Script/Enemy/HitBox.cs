using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitPart
{
    Head,
    Body,
    Arm,
    Leg
}

public class HitBox : MonoBehaviour
{
    public HitPart HitPart;
    [HideInInspector] public float DamageMultiplier;

    void Start()
    {
        switch (HitPart)
        {
            case HitPart.Head:
                DamageMultiplier = 2f;
                break;
            case HitPart.Body:
                DamageMultiplier = 1f;
                break;
            case HitPart.Arm:
                DamageMultiplier = 0.5f;
                break;
            case HitPart.Leg:
                DamageMultiplier = 0.5f;
                break;
        }
    }
}
