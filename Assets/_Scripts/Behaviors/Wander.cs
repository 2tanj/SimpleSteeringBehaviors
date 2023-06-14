using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : SteeringObject
{
    // The forward offset of the wander square
    [SerializeField]
    private float _wanderDistance = 2.0f;
    // The radius of the wander square
    [SerializeField]
    private float _wanderRadius = 1.2f;
    // Maximum amount of random displacement a second
    [SerializeField]
    private float _wanderJitter = 15f;


    private Vector3 _wanderTarget;

    private void Awake()
    {
        // initializing the target position on the wander circle
        float theta = Random.value * 2 * Mathf.PI;
        _wanderTarget = new Vector3(_wanderRadius * Mathf.Cos(theta), 0f, _wanderRadius * Mathf.Sin(theta));
    }

    public override Vector3 GetSteering()
    {
        // get the jitter for this time frame
        float jitter = _wanderJitter * Time.deltaTime;

        // add a small random vector to the targets position
        _wanderTarget += new Vector3(Random.Range(-1f, 1f) * jitter, 0f, Random.Range(-1f, 1f) * jitter);
        _wanderTarget.Normalize();
        _wanderTarget *= _wanderRadius;

        // move the target in front of the charater
        Vector3 targetPos = transform.position + transform.right * _wanderDistance + _wanderTarget;

        return GetSeekSteering(targetPos, 0f);
    }

    // Returns a random number between -1 and 1. Values around zero are more likely.
    private float RandomBinomial()
    {
        return Random.value - Random.value;
    }
}
