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

    private Vector3 force = Vector3.zero;

    private Vector3 acceleration = Vector3.zero;
    [SerializeField] private Vector3 averageAcceleration = Vector3.zero;

    private Vector3 lastSpeed = Vector3.zero;

    private Vector3 momentum = Vector3.zero;

    [SerializeField] private Vector2 GUIOffset;

    public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = initialVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isStatic || !collision.gameObject.TryGetComponent(out MomentumBehavior other))
            return;

        ContactPoint[] contacts = collision.contacts;
        Vector3 averageNormal = new Vector3(contacts.Average(contact => contact.normal.x),
                                            contacts.Average(contact => contact.normal.y),
                                            contacts.Average(contact => contact.normal.z));


        Bounce(averageNormal, other);
        // Vector3 newAverageAcceleration = direction * averageAcceleration.magnitude;
        // force += (newAverageAcceleration + other.averageAcceleration) * massSum;
    }

    private void Bounce(Vector3 normal, MomentumBehavior other)
    {
        Vector3 direction = MathsUtils.Reflect(rb.velocity.normalized, normal);
        Vector3 newSpeed = direction * rb.velocity.magnitude;

        Vector3 newMomentum = rb.velocity * mass;

        Vector3 momentumSum = newMomentum + other.momentum;

        float massSum = mass + other.mass;

        rb.velocity = momentumSum / massSum;
    }

    private void FixedUpdate()
    {
        if (isStatic)
            return;

        if (hasGravity)
            force += -9.81f * mass * Vector3.up;

        acceleration = force / mass;
        force = Vector3.zero;

        lastSpeed = rb.velocity;
        rb.velocity += acceleration * Time.fixedDeltaTime;

        averageAcceleration = (rb.velocity + lastSpeed) / Time.fixedDeltaTime;

        momentum = mass * rb.velocity;

    }

    private void OnGUI()
    {
        if (!debug)
            return;

        GUIContent content = new GUIContent();
        content.text += "Speed = " + rb.velocity + '\n';
        content.text += "Momentum = " + momentum + '\n';
        GUI.Label(new Rect(10 + GUIOffset.x, 10 + GUIOffset.y, 150, 100), content);
    }
}
