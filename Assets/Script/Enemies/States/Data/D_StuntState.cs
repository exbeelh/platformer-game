using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newStuntStateData", menuName = "Data/State Data/Stunt State")]
public class D_StuntState : ScriptableObject
{
    public float stuntTime = 3f;

    public float stuntKnocbackTime = 0.2f;
    public float stuntKnockbackSpeed = 20f;
    public Vector2 stuntKnockbackAngle;
}
