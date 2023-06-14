using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : SteeringObject
{
    [SerializeField]
    private Transform _targetTransform;

    [SerializeField]
    private float _lowestTargetDistance = 3.5f;

    private Vector3 _targetPosition;

    private void Awake()
    {
        _targetPosition = _targetTransform.position;
    }

    public override Vector3 GetSteering()
    {
        _targetPosition = _targetTransform.position;
        return GetSeekSteering(_targetPosition, _lowestTargetDistance);
    }

    public void SetTargetPos(Vector3 pos)
    {
        _targetPosition = pos;
    }
}
