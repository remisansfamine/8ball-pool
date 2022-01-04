using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumBehavior : MonoBehaviour
{
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
        if (!collision.gameObject.TryGetComponent(out MomentumBehavior other))
            return;

        Vector3 direction = transform.position - other.transform.position;

        Vector3 momentumSum = momentum + other.momentum;
        float massSum = mass + other.mass;
        float speedLenght = momentumSum.magnitude / massSum;

        if (speedLenght == 0f)
            speed = Vector3.zero;
        else 
            speed += speedLenght * direction.normalized;
    }

    private void FixedUpdate()
    {
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
        GUIContent content = new GUIContent();
        content.text += "Speed = " + speed + '\n';
        content.text += "Momentum = " + momentum + '\n';
        GUI.Label(new Rect(10 + GUIOffset.x, 10 + GUIOffset.y, 150, 100), content);
    }
}
