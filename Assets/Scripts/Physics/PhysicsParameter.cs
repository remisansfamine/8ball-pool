using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsParameter : MonoBehaviour
{
    [SerializeField] protected float momentOfInertia = 0.1f;

    [SerializeField] protected Vector3 velocity;
    public Vector3 Velocity => velocity;

    [SerializeField] protected Vector3 angularVelocity;
    [SerializeField] protected float frictionCoef = 1f;
    [SerializeField] protected float angularFrictionCoef = 1f;
    [SerializeField] protected float jkrtse = ;

    [SerializeField] protected float mass = 1f;
    public float Mass => mass;

    [SerializeField] protected float radius = 0.5f;
}
