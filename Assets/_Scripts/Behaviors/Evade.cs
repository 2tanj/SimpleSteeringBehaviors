using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evade : SteeringObject
{
    [SerializeField]
    private SteeringObject _target;

    [SerializeField]
    private float _maxPrediction = 1f;

    [Header("Flee atributes")]
    // flee parameters
    [SerializeField]
    private float _panicDist = 3.5f;
    [SerializeField]
    private bool _decelerateOnStop = true;
    [SerializeField]
    private float _maxAcceleration = 10f;
    [SerializeField]
    private float _timeToTarget = .1f;

    public override Vector3 GetSteering()
    {
        // calculate the distance to the target
        var displacement = _target.transform.position - transform.position;
        float distance = displacement.magnitude;

        // get the targets speed
        float speed = _target.Rb.velocity.magnitude;

        //calculate the prediction time
        float prediction;
        if (speed <= distance / _maxPrediction)
            prediction = _maxPrediction;
        else
        {
            prediction = distance / speed;
            // place the predicted position a little before the target reaches the character
            prediction *= .9f;
        }

        var explicitTarget = _target.transform.position + _target.Rb.velocity * prediction;

        return GetFleeSteering(explicitTarget, _decelerateOnStop, _panicDist, _timeToTarget, _maxAcceleration); ;
    }
}
