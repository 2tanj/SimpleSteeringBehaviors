using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviours : MonoBehaviour
{
    private SteeringObject _steeringObject;

    public SteeringBehaviours(SteeringObject steering)
                    => _steeringObject = steering;

    public void Steer()
    {
        var acc = _steeringObject.GetSteering();

        _steeringObject.Rb.velocity += acc * Time.deltaTime;
        if (_steeringObject.Rb.velocity.magnitude > _steeringObject.MovementData.MaxVelocity)
            _steeringObject.Rb.velocity = _steeringObject.Rb.velocity.normalized * _steeringObject.MovementData.MaxVelocity;
    }

    public void LookAtHeadingDirection()
    {
        var dir = _steeringObject.Rb.velocity;
        dir.Normalize();

        /* If we have a non-zero direction then look towards that direciton otherwise do nothing */
        if (dir.sqrMagnitude > 0.001f)
        {
            /* Mulitply by -1 because counter clockwise on the y-axis is in the negative direction */
            float toRotation = -1 * (Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg);
            float rotation = Mathf.LerpAngle(_steeringObject.Rb.rotation.eulerAngles.y, toRotation, Time.deltaTime * _steeringObject.MovementData.TurnSpeed);

            _steeringObject.Rb.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }

}
