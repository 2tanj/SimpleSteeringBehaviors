using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "SteeringBehaviour", order = 1)]
public class MovementDataSO : ScriptableObject
{
    [Header("Basic Information")]

    [Range(1, 10)]
    public float MaxVelocity;
    [Range(1, 50)]
    public float MaxAcceleration;
    [Range(1, 100)]
    public float TurnSpeed;

    [Header("Target Inforation")]

    // The radius from the target which signals that we are close enough and have arrived
    [Range(0, 1)]
    public float TargetRadius;
    // The radius from the target where we start to slow down
    [Range(0, 3)]
    public float SlowRadius;
    // The time in which we want to achieve the TargetSpeed
    [Range(0, 3)]
    public float TimeToTarget;
}
