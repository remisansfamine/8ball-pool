using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MixedMomentumBehavior : MonoBehaviour
{
    [SerializeField] private bool debug = false;

    [SerializeField] private bool isStatic = false;

    [SerializeField] private bool hasGravity = true;

    [SerializeField] private float mass = 1f;

    public Vector3 velocity;

    private Vector3 force = Vector3.zero;

    private Vector3 acceleration = Vector3.zero;

    [SerializeField] private float frictionCoeff = 0.05f;

    Vector3 lastPositionCollision = Vector3.zero;

    Vector3 nextPosition => transform.position + velocity * Time.fixedDeltaTime;
    Quaternion nextRotation => transform.rotation * Quaternion.Euler(Time.fixedDeltaTime * angularVelocity);

    private Vector3 momentum = Vector3.zero;

    [SerializeField] private Vector2 GUIOffset;

    [SerializeField] private float momentOfInertia = 1f;

    [SerializeField] private Vector3 torque = Vector3.zero;
    [SerializeField] private Vector3 angularMomentum = Vector3.zero;
    [SerializeField] private Vector3 angularAcceleration = Vector3.zero;
    [SerializeField] private Vector3 angularVelocity = Vector3.zero;
    [SerializeField] private Vector3 orientation = Vector3.zero;


    void AddForce(Vector3 _force)
    {
        force += _force;
    }

    void AddTorque(Vector3 _force, Vector3 _position)
    {
        torque += Vector3.Cross(_position - transform.position, _force);
    }
    public void AddForceTorque(Vector3 _force, Vector3 _position)
    {
        AddForce(_force);
        AddTorque(_force, _position);
    }

    private void Bounce(Vector3 normal, MixedMomentumBehavior other)
    {
        if (isStatic || !other || other == this)
            return;

        Vector3 deltaV = velocity - other.velocity;

        float massSum = mass + other.mass;
        Vector3 u1 = other.mass / massSum * deltaV;
        Vector3 u2 = mass / massSum * deltaV;

        Vector3 direction = normal;
        direction = MathsUtils.Reflect(deltaV.normalized, normal);

        velocity = u1.magnitude * direction;

        if (!other.isStatic)
            other.velocity = -u2.magnitude * direction;
    }

    bool CheckIsZero(float epsilon) => velocity.sqrMagnitude < epsilon;


    void ComputeCollisions()
    {

        {
            float distance = Vector3.Distance(transform.position, nextPosition);
            bool hasHit = Physics.SphereCast(transform.position, 0.5f, velocity.normalized, out RaycastHit hit, distance);

            int iterationMax = 10;
            for (int i = 0; i < iterationMax && hasHit; i++)
            {
                if (!hit.collider.TryGetComponent(out MixedMomentumBehavior other))
                    break;

                Bounce(hit.normal, other);

                lastPositionCollision = nextPosition;

                distance = Vector3.Distance(transform.position, nextPosition);
                hasHit = Physics.SphereCast(transform.position, 0.5f, velocity.normalized, out hit, distance);
            }
        }

        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject == gameObject)
                    continue;

                if (!collider.TryGetComponent(out MixedMomentumBehavior other))
                    break;

                if (Physics.Raycast(transform.position, collider.ClosestPoint(transform.position) - transform.position, out RaycastHit hit))
                {
                    Debug.Log(other.name);

                    Bounce(hit.normal, other);

                    lastPositionCollision = collider.ClosestPoint(transform.position);
                    Debug.DrawRay(collider.ClosestPoint(transform.position), hit.normal, Color.red, 10);
                }
            }
        }
    }

    private void ApplyNewtonianLaws()
    {
        angularMomentum = torque * Time.fixedDeltaTime;
        torque = Vector3.zero;

        angularAcceleration = angularMomentum / momentOfInertia;

        angularVelocity += angularAcceleration * Time.fixedDeltaTime;

        acceleration = force / mass;
        force = Vector3.zero;

        velocity += acceleration * Time.fixedDeltaTime;

        transform.position = nextPosition;
        transform.rotation = nextRotation;
    }

    private void FixedUpdate()
    {
        if (isStatic)
            return;

        ComputeCollisions();

        if (hasGravity)
            AddForce(-9.81f * mass * Vector3.up);

        AddForce(-frictionCoeff * velocity);

        ApplyNewtonianLaws();

        momentum = mass * velocity;
    }

    private void OnGUI()
    {
        if (!debug)
            return;

        GUIContent content = new GUIContent();
        content.text += "Speed = " + velocity + '\n';
        content.text += "Momentum = " + momentum + '\n';
        content.text += "angularMomentum = " + angularMomentum + '\n';
        content.text += "angularAcceleration = " + angularAcceleration + '\n';
        content.text += "angularVelocity = " + angularVelocity + '\n';
        GUI.Label(new Rect(10 + GUIOffset.x, 10 + GUIOffset.y, 150, 100), content);

    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Vector4(1,0,0,0.5f);
        Gizmos.DrawSphere(lastPositionCollision, 0.5f);  
    }
}