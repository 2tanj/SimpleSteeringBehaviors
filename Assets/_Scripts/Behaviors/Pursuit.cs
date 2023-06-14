using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuit : SteeringObject
{
    [SerializeField]
    private SteeringObject _target;

    // Maximum prediction time the pursue will predict in the future
    [SerializeField]
    private float _maxPrediction = 1f;

    public override Vector3 GetSteering()
    {
        // calculate the distance to the target
        var displacement = _target.transform.position - transform.position;
        float distance = displacement.magnitude;

        // get the agents speed
        float speed = Rb.velocity.magnitude;

        float prediction;
        if (speed <= distance / _maxPrediction)
            prediction = _maxPrediction;
        else
            prediction = distance / speed;

        // put the target together based on where we think the target will be
        var explicitTarget = _target.transform.position + _target.Rb.velocity * prediction;

        return GetSeekSteering(explicitTarget, 0f);
    }
}
