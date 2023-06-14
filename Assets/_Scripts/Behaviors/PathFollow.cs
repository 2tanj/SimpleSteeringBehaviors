using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : SteeringObject
{
    [SerializeField]
    private Path _path;

    [SerializeField]
    private float _stopRadius = .05f;
    [SerializeField]
    private float _pathOffset = .1f;
    [SerializeField]
    private float _pathDirection = 1f;

    [SerializeField]
    private bool _pathLoop = false;
    [SerializeField]
    private bool _reversePath = false;

    private Vector3 _targetPosition;

    private void FixedUpdate()
    {
        if (_reversePath && IsAtEndOfPath(_path))
            _path.ReversePath();

        _steering.Steer();
        _steering.LookAtHeadingDirection();
    }

    public override Vector3 GetSteering()
    {
        // if the path has only one ndoe then just go to that position
        if (_path.NodesPos.Length == 1)
            _targetPosition = _path.NodesPos[0];
        // else find the closest spot on the path to the character and go to that instead
        else
        {
            float param = _path.GetParam(transform.position);

            if (!_pathLoop)
            {
                Vector3 finalDestination;

                if (IsAtEndOfPath(_path, param, out finalDestination))
                {
                    _targetPosition = finalDestination;

                    Rb.velocity = Vector3.zero;
                    return Vector3.zero;
                }
            }

            // move down the path
            param += _pathDirection * _pathOffset;
            // set the target position
            _targetPosition = _path.GetPosition(param, _pathLoop);
        }

        return GetArriveSteering(_targetPosition);
    }

    public bool IsAtEndOfPath(Path path)
    {
        // if the path has only one node then just check the distance to that node
        if (path.NodesPos.Length == 1)
        {
            Vector3 endPos = GroundVector(path.NodesPos[0]);
            return Vector3.Distance(transform.position, endPos) < _stopRadius;
        }
        else
        {
            Vector3 finalDestination;
            // get the param for the closest position point on the path given the characters position
            float param = path.GetParam(transform.position);

            return IsAtEndOfPath(path, param, out finalDestination);
        }
    }

    private bool IsAtEndOfPath(Path path, float param, out Vector3 finalDestination)
    {
        bool result;

        finalDestination = (_pathDirection > 0) ? path.NodesPos[path.NodesPos.Length - 1] : path.NodesPos[0];
        finalDestination = GroundVector(finalDestination);

        if (param >= path.Distances[path.NodesPos.Length - 2])
            result = Vector3.Distance(Rb.position, finalDestination) < _stopRadius;
        else
            result = false;

        return result;
    }
}
