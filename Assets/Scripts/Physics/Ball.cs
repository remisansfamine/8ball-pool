using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float momentOfInertia = 0.1f;

    [SerializeField] private Vector3 velocity;
    public Vector3 Velocity => velocity;

    [SerializeField] private Vector3 angularVelocity;
    [SerializeField] private float frictionCoef = 1f;
    [SerializeField] private float angularFrictionCoef = 1f;

    [SerializeField] private float mass = 1f;

    [SerializeField] private float radius = 0.5f;

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
        
    }

    private void FixedUpdate()
    {
        if (CheckCollision(out RaycastHit hit))
            OnCollide(hit);

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

        //Vector3 coef = Vector3.Cross(velocity, angularVelocity).normalized;
        //velocity += velocity * Vector3.Dot(-velocity, coef);
        //Debug.Log("Coef : " + coef + " Angle : " + angle);
        
        Move();
    }

    public void AddForce(Vector3 force)
    {
        velocity += Time.fixedDeltaTime / mass * force;
    }

    public void AddTorque(Vector3 torque) => angularVelocity += Time.fixedDeltaTime / momentOfInertia * torque;
    public void AddTorque(Vector3 force, Vector3 position) => AddTorque(force, position - transform.position);
    public void AddLocalTorque(Vector3 force, Vector3 position) => AddTorque(Vector3.Cross(position, force));
}
