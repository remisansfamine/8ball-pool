using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : PhysicsParameter
{
    bool Approximately(Vector3 a, float epsilon) { return a.sqrMagnitude < epsilon; }
    RaycastHit[] CheckCollision()
    {
        Vector3 nextPosition = transform.position + velocity * Time.fixedDeltaTime;

        float distance = Vector3.Distance(transform.position, nextPosition);
        Vector3 velDir = velocity.normalized;

        return Physics.SphereCastAll(transform.position, radius, velDir, distance);
    }

    private void OnCollide(RaycastHit hit)
    {
        if (!hit.collider.TryGetComponent(out PhysicsParameter other))
            return;

        Vector3 deltaV = Velocity - other.Velocity;
        float massSum = Mass + other.Mass;

        Vector3 u1 = other.Mass / massSum * deltaV;
        Vector3 u2 = Mass / massSum * deltaV;

        Vector3 direction = MathsUtils.Reflect(deltaV.normalized, hit.normal);

        float averageBounciness = (Bounciness + other.Bounciness) * 0.5f;

        Vector3 vel = u1.magnitude * direction;
        Vector3 force = mass / Time.fixedDeltaTime * (vel - velocity);
         
        //Debug.LogWarning("Force : " + force);
        AddForce(force * averageBounciness);
        //AddTorque(vel, hit.point);

        Debug.DrawRay(hit.point, hit.normal * 5, Color.red, 10f);

        Vector3 otherVel = -u2.magnitude * direction;
        Vector3 otherForce = other.Mass / Time.fixedDeltaTime * (otherVel - other.Velocity);

        if (!isStatic)
        {
            other.AddForce(otherForce * averageBounciness);
            //other.AddTorque(otherForce, hit.point);
        }
    }

    private void FixedUpdate()
    {
        RaycastHit[] hits = CheckCollision();

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject != gameObject)
                OnCollide(hit);
        }

        Move();
        SimulatePhysics();
    }

    private void Move()
    {
        transform.position += velocity * Time.fixedDeltaTime;
        transform.rotation *= Quaternion.Euler(Time.fixedDeltaTime * angularVelocity);
    }

    private void SimulatePhysics() 
    {
        //Simulate gravity
        AddForce(Physics.gravity * Mass);

        //Simulate Friction
        AddForce(-frictionCoef * velocity);
        AddTorque(-angularFrictionCoef  * angularVelocity);

        //Simulate Spin
        AddLocalTorque(-velocity, Vector3.down);

    }

}
