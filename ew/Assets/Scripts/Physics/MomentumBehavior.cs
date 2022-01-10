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

    [SerializeField] private Vector3 initialVelocity;
    public Vector3 velocity;

    private Vector3 force = Vector3.zero;

    private Vector3 acceleration = Vector3.zero;
    [SerializeField] private Vector3 averageAcceleration = Vector3.zero;
    Vector3 nextPosition => transform.position + velocity * Time.fixedDeltaTime;

    private Vector3 lastSpeed = Vector3.zero;

    private Vector3 momentum = Vector3.zero;

    [SerializeField] private Vector2 GUIOffset;

    public Rigidbody rb;

    private void Awake()
    {
        if (TryGetComponent(out rb))
            rb.velocity = initialVelocity;
    }

    private void Bounce(Vector3 normal, MomentumBehavior other)
    {
        if (!other || other == this)
            return;

        Debug.Log(other);

        Vector3 direction = MathsUtils.Reflect(velocity.normalized, normal);

        Vector3 newSpeed = direction * velocity.magnitude;
        // Vector3 newAverageAcceleration = direction * averageAcceleration.magnitude;
        // force += (newAverageAcceleration + other.averageAcceleration) * massSum;

        Vector3 newMomentum = newSpeed * mass;

        Vector3 momentumSum = newMomentum + other.momentum;

        float massSum = mass + other.mass;

        Vector3 finalSpeed = momentumSum / massSum;

        velocity = newSpeed;
        transform.position = nextPosition;

        if (!other.isStatic)
        {
            other.velocity = finalSpeed;
            other.transform.position = other.nextPosition;
        }
    }


    private void FixedUpdate()
    {
        if (isStatic)
            return;

        float distance = Vector3.Distance(transform.position, nextPosition);
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.5f, velocity, distance);

        foreach (RaycastHit hit in hits)
            Bounce(hit.normal, hit.collider.GetComponent<MomentumBehavior>());

        if (hasGravity)
            force += -9.81f * mass * Vector3.up;

        acceleration = force / mass;
        force = Vector3.zero;

        lastSpeed = velocity;
        velocity += acceleration * Time.fixedDeltaTime;

        averageAcceleration = (velocity + lastSpeed) / Time.fixedDeltaTime;

        momentum = mass * velocity;

        transform.position = nextPosition;


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