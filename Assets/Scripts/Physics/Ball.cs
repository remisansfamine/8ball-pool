using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : PhysicsParameter
{
    bool CheckCollision(out RaycastHit hit)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == gameObject)
                continue;

            Vector3 dir = transform.position - collider.ClosestPoint(transform.position);
            Ray ray = new Ray(transform.position, dir);
            if (Physics.Raycast(ray, out hit, 0.5f))
            {
                return true;
            }
        }

        hit = new RaycastHit();
        return false;
    }

    private void OnCollide(RaycastHit hit)
    {
        //if (!hit.collider.TryGetComponent(out PhysicsParameter other))
        //    return;
        //
        //Vector3 deltaV = Velocity - other.Velocity;
        //float massSum = Mass + other.Mass;
        //
        //Vector3 u1 = other.Mass / massSum * deltaV;
        //Vector3 u2 = Mass / massSum * deltaV;
        //
        //Vector3 direction = MathsUtils.Reflect(deltaV.normalized, hit.normal);
        //Vector3 force = u1.magnitude * direction;
        //velocity = force;
        //AddTorque(force, hitPoint);
        //
        //if (!other.isStatic)
        //{
        //    Vector3 otherForce = -u2.magnitude * direction;
        //    other.velocity = otherForce;
        //    other.AddTorque(otherForce, hitPoint);
        //}
    }

    private void FixedUpdate()
    {
        if (CheckCollision(out RaycastHit hit))
            OnCollide(hit);

        SimulatePhysics();
    }

    private void Move()
    {
        transform.position += new Vector3(velocity.x, 0f, velocity.z) * Time.fixedDeltaTime;
        transform.rotation *= Quaternion.Euler(Time.fixedDeltaTime * angularVelocity);
    }

    private void SimulatePhysics() 
    {
        //Simulate Friction
        AddForce(-frictionCoef * velocity);
        AddTorque(-angularFrictionCoef  * angularVelocity);

        //AddForce(Quaternion.Euler(angularVelocity / Time.fixedDeltaTime) * velocity);

        //Simulate Spin
        //Vector3 radAngularVelocity = angularVelocity * Mathf.Deg2Rad;
        //
        //AddForce((radAngularVelocity * 0.5f - velocity) / Time.fixedDeltaTime);

        Move();
    }

    public void AddForce(Vector3 force)
    {
        velocity += Time.fixedDeltaTime / mass * force;
        //AddLocalTorque(-velocity, Vector3.down);
    }

    //xnzsn
    
    public void AddTorque(Vector3 torque)
    {
        angularVelocity += Time.fixedDeltaTime / momentOfInertia * torque;
        //AddForce(Quaternion.Euler(angularVelocity * Time.fixedDeltaTime) * velocity * jkrtse);
        velocity = Quaternion.Euler(angularVelocity * Time.fixedDeltaTime) * velocity;
        Debug.Log("Velocity = " + velocity);
    }
    public void AddTorque(Vector3 force, Vector3 position) => AddTorque(Vector3.Cross(position - transform.position, force));
    public void AddLocalTorque(Vector3 force, Vector3 position) => AddTorque(Vector3.Cross(position, force));
}
