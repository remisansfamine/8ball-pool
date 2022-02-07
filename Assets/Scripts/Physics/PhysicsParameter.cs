using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsParameter : MonoBehaviour
{
    [SerializeField] private bool isStatic;

    [SerializeField] protected float momentOfInertia = 0.1f;

    [SerializeField] public Vector3 velocity;
    public Vector3 Velocity => velocity;

    [SerializeField] protected Vector3 angularVelocity;
    [SerializeField] protected float frictionCoef = 1f;
    [SerializeField] protected float angularFrictionCoef = 1f;

    [SerializeField, Range(0.00001f, Mathf.Infinity)] protected float mass = 1f;
    public float Mass => mass;

    [SerializeField] protected float radius = 0.5f;

    public void AddForce(Vector3 force)
    {
        if (!isStatic)
            velocity += Time.fixedDeltaTime / mass * force;
        //AddLocalTorque(-velocity, Vector3.down);
    }

    public void AddTorque(Vector3 torque)
    { 
        if (!isStatic)
            angularVelocity += Time.fixedDeltaTime / momentOfInertia * torque;
        //AddForce(Quaternion.Euler(angularVelocity * Time.fixedDeltaTime) * velocity * jkrtse);
        //velocity = Quaternion.Euler(angularVelocity * Time.fixedDeltaTime) * velocity;
        //Debug.Log("Velocity = " + velocity);
    }
    public void AddTorque(Vector3 force, Vector3 position) => AddTorque(Vector3.Cross(position - transform.position, force));
    public void AddLocalTorque(Vector3 force, Vector3 position) => AddTorque(Vector3.Cross(position, force));
}
