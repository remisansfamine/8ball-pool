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

    private Vector3 force = Vector3.zero;

    private Vector3 acceleration = Vector3.zero;
    [SerializeField] private Vector3 averageAcceleration = Vector3.zero;

    [SerializeField] private Vector3 speed = Vector3.zero;
    private Vector3 lastSpeed = Vector3.zero;

    private Vector3 momentum = Vector3.zero;

    [SerializeField] private Vector2 GUIOffset;

    private void OnCollisionEnter(Collision collision)
    {
        if (isStatic || !collision.gameObject.TryGetComponent(out MomentumBehavior other))
            return;

        ContactPoint[] contacts = collision.contacts;
        Vector3 averageNormal = new Vector3(contacts.Average(contact => contact.normal.x),
                                            contacts.Average(contact => contact.normal.y),
                                            contacts.Average(contact => contact.normal.z));

        Vector3 direction = Vector3.Reflect(speed.normalized, averageNormal);

        Vector3 newSpeed = direction * speed.magnitude;

        Vector3 newMomentum = newSpeed * mass;

        Vector3 momentumSum = newMomentum + other.momentum;

        float massSum = mass + other.mass;

        speed = momentumSum / massSum;

        // Vector3 newAverageAcceleration = direction * averageAcceleration.magnitude;
        // force += (newAverageAcceleration + other.averageAcceleration) * massSum;
    }

    private void FixedUpdate()
    {
        if (isStatic)
            return;

        if (hasGravity)
            force += -9.81f * mass * Vector3.up;

        acceleration = force / mass;
        force = Vector3.zero;

        lastSpeed = speed;
        speed += acceleration * Time.fixedDeltaTime;

        averageAcceleration = (speed + lastSpeed) / Time.fixedDeltaTime;

        momentum = mass * speed;

        transform.position += speed * Time.fixedDeltaTime;
    }

    private void OnGUI()
    {
        if (!debug)
            return;

        GUIContent content = new GUIContent();
        content.text += "Speed = " + speed + '\n';
        content.text += "Momentum = " + momentum + '\n';
        GUI.Label(new Rect(10 + GUIOffset.x, 10 + GUIOffset.y, 150, 100), content);
    }
}
