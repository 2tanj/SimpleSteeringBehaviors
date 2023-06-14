using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : SteeringObject
{
    [SerializeField]
    private Transform _targetPosition;

    public override Vector3 GetSteering()
    {
        return GetArriveSteering(_targetPosition.position);
    }
}
