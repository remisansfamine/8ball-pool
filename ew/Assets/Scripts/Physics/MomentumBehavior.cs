using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MomentumBehavior : MonoBehaviour
{
    [SerializeField] private bool debug = false;

    [SerializeField] private bool isStatic = false;

    [SerializeField] private bool hasGravity = true;

    public float mass = 1f;

    public Vector3 velocity;

    public Vector3 force { get; private set; } = Vector3.zero;

    public Vector3 acceleration { get; private set; } = Vector3.zero;

    public float frictionCoeff = 0.05f;

    Vector3 lastPositionCollision = Vector3.zero;

    Vector3 nextPosition => transform.position + velocity * Time.fixedDeltaTime;

    private Vector3 momentum = Vector3.zero;

    [SerializeField] private Vector2 GUIOffset;

    private void Bounce(Vector3 normal, MomentumBehavior other)
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
                if (!hit.collider.TryGetComponent(out MomentumBehavior other))
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

                if (!collider.TryGetComponent(out MomentumBehavior other))
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

    public void AddForce(in Vector3 inForce) => force += inForce;

    private void ApplyNewtonianLaws()
    {
        acceleration = force / mass;
        force = Vector3.zero;

        velocity += acceleration * Time.fixedDeltaTime;

        transform.position = nextPosition;
    }
    
    private void FixedUpdate()
    {
        if (isStatic)
            return;

        ComputeCollisions();

        if (hasGravity)
            AddForce(9.81f * mass * Vector3.down);

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
        GUI.Label(new Rect(10 + GUIOffset.x, 10 + GUIOffset.y, 150, 100), content);

    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Vector4(1,0,0,0.5f);
        Gizmos.DrawSphere(lastPositionCollision, 0.5f);  
    }
}