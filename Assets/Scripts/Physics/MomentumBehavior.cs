using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MomentumBehavior : MonoBehaviour
{
    [SerializeField] private bool debug = false;

    [SerializeField] private bool isStatic = false;

    [SerializeField] private bool hasGravity = true;

    [SerializeField] private float mass = 1f;

    public Vector3 velocity;

    private Vector3 force = Vector3.zero;

    private Vector3 acceleration = Vector3.zero;

    [SerializeField] private float frictionCoeff = 0.05f;

    Vector3 nextPosition => transform.position + velocity * Time.fixedDeltaTime;

    private Vector3 momentum = Vector3.zero;

    [SerializeField] private Vector2 GUIOffset;

    private void Bounce(Vector3 normal, MomentumBehavior other)
    {
        if (isStatic || !other || other == this)
            return;

        Vector3 direction = MathsUtils.Reflect(velocity.normalized, normal);

        Vector3 newSpeed = direction * velocity.magnitude;
        Vector3 newMomentum = newSpeed * mass;

        Vector3 momentumSum = newMomentum + other.momentum;
        float massSum = mass + other.mass;

        Vector3 finalSpeed = momentumSum / massSum;

        velocity = finalSpeed;
    }

    void ComputeCollisions()
    {
        float distance = Vector3.Distance(transform.position, nextPosition);
        bool hasHit = Physics.SphereCast(transform.position, 0.5f, velocity, out RaycastHit hit, distance);

        int iterationMax = 10;
        for (int i = 0; i < iterationMax && hasHit; i++)
        {
            if (!hit.collider.TryGetComponent(out MomentumBehavior otherMomentum))
                break;

            Bounce(hit.normal, otherMomentum);
            otherMomentum.Bounce(-hit.normal, this);

            distance = Vector3.Distance(transform.position, nextPosition);
            hasHit = Physics.SphereCast(transform.position, 0.5f, velocity, out hit, distance);
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
        GUI.Label(new Rect(10 + GUIOffset.x, 10 + GUIOffset.y, 150, 100), content);
    }
}