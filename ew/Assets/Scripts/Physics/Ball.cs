using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : PhysicsParameter
{
    bool Approximately(Vector3 a, float epsilon) { return a.sqrMagnitude < epsilon; }
    bool CheckCollision(out RaycastHit hit)
    {
        {
            Vector3 nextPosition = transform.position + velocity * Time.fixedDeltaTime;

            float distance = Vector3.Distance(transform.position, nextPosition);

            Vector3 velDir = velocity.normalized;

            bool hasHit = Physics.SphereCast(transform.position, radius, velDir, out hit, distance + 0.1f);
            
            if (hasHit)
            {
                if (hit.collider.gameObject != gameObject)
                {
                    Debug.Log("CAAAAAAAAAAAAAASSSSSSSSSSSSSSSSSSSSSSSSSSSST !111");
                    return true;
                }
                else
                {
                    Debug.LogWarning("Il y a un probleme car c'est moi qui collide 11!!!");
                }
            }
        }

        //{
        //    Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        //
        //    foreach (Collider collider in colliders)
        //    {
        //        if (collider.gameObject == gameObject)
        //            continue;
        //
        //        Vector3 dir = collider.ClosestPoint(transform.position) - transform.position;
        //        Ray ray = new Ray(transform.position, dir);
        //        if (Physics.Raycast(ray, out hit, 0.5f))
        //        {
        //            Debug.Log("OVERLQQQQAAAp !111");
        //            return true;
        //        }
        //    }
        //
        //    hit = new RaycastHit();
        //    return false;
        //}

        return false;
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

        Vector3 vel = u1.magnitude * direction;
        Vector3 force = mass / Time.fixedDeltaTime * (vel - velocity);

        AddForce(force);
        AddTorque(force, hit.point);

        Vector3 otherVel = -u2.magnitude * direction;
        Vector3 otherForce = other.Mass / Time.fixedDeltaTime * (otherVel - other.velocity);
        
        other.AddForce(otherForce);
        other.AddTorque(otherForce, hit.point);
    }

    private void FixedUpdate()
    {
        if (CheckCollision(out RaycastHit hit))
            OnCollide(hit);

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
        //Simulate Friction
        AddForce(-frictionCoef * velocity);
        AddTorque(-angularFrictionCoef  * angularVelocity);

        //Simulate Spin
        AddLocalTorque(-velocity, Vector3.down);

        //Simulate gravity
        AddForce(Physics.gravity * Mass);

    }

}
