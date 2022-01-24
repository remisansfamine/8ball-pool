using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private static float momentOfInertia = 0.1f;

    [SerializeField] private Vector3 velocity;
    public Vector3 Velocity => velocity;

    [SerializeField] private Vector3 angularVelocity;
    [SerializeField] private float frictionCoef = 1f;

    [SerializeField] private static float mass = 1f;

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
        Vector3 frictionForce = -frictionCoef * velocity;
        AddForce(frictionForce);

        float sign = Mathf.Sign(Vector3.Dot(angularVelocity, transform.forward));

        Vector3 angularFrictionTorque = sign * frictionCoef * angularVelocity;
        AddLocalTorque(angularFrictionTorque, Vector3.down);

        //Simulate Spin

        Move();
    }

    public void AddForce(Vector3 force)
    {
        velocity += Time.fixedDeltaTime / mass * force;
    }

    public void AddTorque(Vector3 force, Vector3 position)
    {
        Vector3 globalPos = position - transform.position;
        angularVelocity += Time.fixedDeltaTime / momentOfInertia * Vector3.Cross(globalPos, force);
    }

    public void AddLocalTorque(Vector3 force, Vector3 position)
    {
        angularVelocity += Time.fixedDeltaTime / momentOfInertia * Vector3.Cross(position, force);
    }

}
