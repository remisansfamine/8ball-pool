using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngularMomentumBehavior : MonoBehaviour
{
    [SerializeField] private float mass = 1f;
    [SerializeField] private float momentOfInertia = 1f;

    [SerializeField] private Vector3 force = Vector3.zero;
    [SerializeField] private Vector3 acceleration = Vector3.zero;
    [SerializeField] private Vector3 speed = Vector3.zero;
    [SerializeField] private Vector3 torque = Vector3.zero;
    [SerializeField] private Vector3 angularMomentum = Vector3.zero;
    [SerializeField] private Vector3 angularAcceleration = Vector3.zero;
    [SerializeField] private Vector3 angularVelocity = Vector3.zero;
    [SerializeField] private Vector3 orientation = Vector3.zero;

    [SerializeField] private Vector3 startForce;
    [SerializeField] private Vector3 startForcePosition;

    void AddForce(Vector3 _force)
    {
        force += _force;
    }

    void AddTorque(Vector3 _force, Vector3 _position)
    {
        torque += Vector3.Cross(_position - transform.position, _force);
    }

    private void Start()
    {
        AddTorque(startForce, transform.position + startForcePosition);
    }

    private void Update()
    {
        Debug.DrawRay(transform.position + startForcePosition, startForce, Color.red);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        angularMomentum = torque * Time.fixedDeltaTime;
        torque = Vector3.zero;

        angularAcceleration = angularMomentum / momentOfInertia;

        angularVelocity += angularAcceleration * Time.fixedDeltaTime;

        acceleration = force / mass;
        force = Vector3.zero;

        speed += acceleration * Time.fixedDeltaTime;

        transform.position += acceleration * Time.fixedDeltaTime;

        //transform.eulerAngles += angularVelocity * Time.fixedDeltaTime;

        //transform.rotation = new Quaternion
        /* orientation += Time.fixedDeltaTime * angularVelocity;
         Vector3 halfAngle = 0.5f * orientation;

         float l = halfAngle.magnitude;

         if (l > 0f)
         {
             halfAngle *= Mathf.Sin(l) / l;
             transform.rotation = new Quaternion(halfAngle.x, halfAngle.y, halfAngle.z, Mathf.Cos(l));
         }
         else 
             transform.rotation = new Quaternion(halfAngle.x, halfAngle.y, halfAngle.z, 1f);*/

        transform.rotation *= Quaternion.Euler(Time.fixedDeltaTime * angularVelocity);
    }

    private void OnGUI()
    {
        GUIContent content = new GUIContent();
        content.text += "angularMomentum = " + angularMomentum + '\n';
        content.text += "angularAcceleration = " + angularAcceleration + '\n';
        content.text += "angularVelocity = " + angularVelocity + '\n';
        content.text += "acceleration = " + acceleration + '\n';
        content.text += "speed = " + speed + '\n';
        content.text += "transform.rotation = " + transform.rotation + '\n';
        GUI.Label(new Rect(10, 10, 5000, 5000), content);
    }
}
