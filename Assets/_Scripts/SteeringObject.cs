using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public abstract class SteeringObject : MonoBehaviour
{
    [field: SerializeField]
    public MovementDataSO MovementData { get; set; }

    public Rigidbody Rb { get; private set; }
    private CapsuleCollider _collider;

    protected SteeringBehaviours _steering;
    public abstract Vector3 GetSteering();

    private void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        Rb = GetComponent<Rigidbody>();
        InitRb();

        _steering = new SteeringBehaviours(this);
    }

    private void FixedUpdate()
    {
        _steering.Steer();
        _steering.LookAtHeadingDirection();
    }


    private void InitRb()
    {
        Rb.interpolation = new RigidbodyInterpolation();
        Rb.freezeRotation = true;
    }

    public Vector3 GroundVector(Vector3 vect)
    {
        vect.y = 0;
        return vect;
    }

    protected Vector3 GetSeekSteering(Vector3 targetPos, float dist)
    {
        var acc = GroundVector(targetPos - transform.position);
        // if were inside the lowest distance of the target, stop
        if (acc.magnitude <= dist)
            return Vector3.zero;

        acc.Normalize();

        acc *= MovementData.MaxAcceleration;

        return acc;
    }

    protected Vector3 GetArriveSteering(Vector3 targetPos)
    {
        Debug.DrawLine(transform.position, targetPos, Color.green, 0f, false);

        targetPos = GroundVector(targetPos);

        // get the direction for the linear accel
        var targetVelo = targetPos - Rb.position;
        // get the distance to the target
        float distance = targetVelo.magnitude;

        // We stop if were within the stopping radius
        if (distance < MovementData.TargetRadius)
        {
            Rb.velocity = Vector3.zero;
            return Vector3.zero;
        }

        // Calculate the target speed depending on the distance
        float targetSpeed;
        if (distance > MovementData.SlowRadius)
            targetSpeed = MovementData.MaxVelocity;
        else
            targetSpeed = MovementData.MaxVelocity * (distance / MovementData.SlowRadius);

        // Give our velocity the adjusted speed
        targetVelo.Normalize();
        targetVelo *= targetSpeed;

        // Calculate the linear acceleration
        var acceleration = targetVelo - Rb.velocity;
        /* Rather than accelerate the character to the correct speed in 1 second, 
        * accelerate so we reach the desired speed in timeToTarget seconds
        * (if we were to actually accelerate for the full timeToTarget seconds). */
        acceleration *= 1 / MovementData.TimeToTarget;

        // Make sure to not pass the max acc
        if (acceleration.magnitude > MovementData.MaxAcceleration)
        {
            acceleration.Normalize();
            acceleration *= MovementData.MaxAcceleration;
        }

        return acceleration;
    }

    // TODO: Clean this up!!
    protected Vector3 GetFleeSteering(Flee flee)
    {
        var acceleration = transform.position - flee.Target.position;

        // if the target is far then dont flee
        if (acceleration.magnitude > flee.PanicDist)
        {
            // slow down if we should decelerate on stop
            if (flee.DecelerateOnStop && Rb.velocity.magnitude > .001f)
            {
                // decelerato to zero velocity in time to target amount of time
                acceleration = -Rb.velocity / flee.TimeToTarget;

                if (acceleration.magnitude > flee.MaxAcceleration)
                    acceleration = GiveMaxAccel(acceleration, flee.MaxAcceleration);

                return acceleration;
            }
            else
            {
                Rb.velocity = Vector3.zero;
                return Vector3.zero;
            }
        }

        return GiveMaxAccel(acceleration, flee.MaxAcceleration);
    }
    
    protected Vector3 GetFleeSteering(Vector3 target, bool decelerate, float panicDist, float time, float maxAccel)
    {
        var acceleration = transform.position - target;

        // if the target is far then dont flee
        if (acceleration.magnitude > panicDist)
        {
            // slow down if we should decelerate on stop
            if (decelerate && Rb.velocity.magnitude > .001f)
            {
                // decelerato to zero velocity in time to target amount of time
                acceleration = -Rb.velocity / time;

                if (acceleration.magnitude > maxAccel)
                    acceleration = GiveMaxAccel(acceleration, maxAccel);

                return acceleration;
            }
            else
            {
                Rb.velocity = Vector3.zero;
                return Vector3.zero;
            }
        }

        return GiveMaxAccel(acceleration, maxAccel);
    }

    private Vector3 GiveMaxAccel(Vector3 vect, float maxAccel)
    {
        vect.Normalize();
        vect *= maxAccel;
        return vect;
    }
}
