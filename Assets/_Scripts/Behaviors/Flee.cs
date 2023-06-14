using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : SteeringObject
{
    [field: SerializeField]
    public Transform Target { get; private set; }

    [field: SerializeField]
    public float PanicDist { get; private set; } = 3.5f;
    [field: SerializeField]
    public bool DecelerateOnStop { get; private set; } = true;
    [field: SerializeField]
    public float MaxAcceleration { get; private set; } = 10f;
    [field: SerializeField]
    public float TimeToTarget { get; private set; } = .1f;

    public override Vector3 GetSteering()
    {
        return GetFleeSteering(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PanicDist);
    }

    public void SetTarget(Vector3 pos) => Target.position = pos;
}
