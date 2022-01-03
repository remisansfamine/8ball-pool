using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehavior : MonoBehaviour
{
    [SerializeField] private float mass = 1f;

    private Vector3 lastSpeed = Vector3.zero;
    [SerializeField] private Vector3 speed = Vector3.zero;
    [SerializeField] private Vector3 acceleration = Vector3.zero;
    private Vector3 averageAcceleration = Vector3.zero;

    private Vector3 momentum = Vector3.zero;
    private Vector3 force = Vector3.zero;

    [SerializeField] private Vector2 GUIOffset;


    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out MovementBehavior other))
            return;

        ContactPoint contact = collision.contacts[0];
        Vector3 difference = other.transform.position - transform.position;

        Vector3 momentumSum = momentum + other.momentum;
        float massSum = mass + other.mass;
        speed = momentumSum.magnitude / massSum * difference.normalized;

        Debug.Log("Entered");
    }

    private void FixedUpdate()
    {
        acceleration += force / mass;
        force = Vector3.zero;

        lastSpeed = speed;
        speed += acceleration * Time.fixedDeltaTime;

        averageAcceleration = (speed + lastSpeed) / Time.fixedDeltaTime;

        momentum = mass * speed;

        transform.position += speed * Time.fixedDeltaTime;
    }

    private void OnGUI()
    {
        GUIContent content = new GUIContent();
        content.text += "Speed = " + speed + '\n';
        content.text += "Momentum = " + momentum + '\n';
        GUI.Label(new Rect(10 + GUIOffset.x, 10 + GUIOffset.y, 150, 100), content);
    }
}
