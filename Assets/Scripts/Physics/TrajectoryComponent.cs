using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryComponent : MonoBehaviour
{
    MomentumBehavior mb = null;
    LineRenderer lr = null;

    [SerializeField] private int pointCount;
    [SerializeField] private float timeStep;

    // Start is called before the first frame update
    void Awake()
    {
        mb = GetComponent<MomentumBehavior>();
        lr = GetComponent<LineRenderer>();
    }

    public void ComputeTrajectory()
    {
        lr.positionCount = pointCount;

        Vector3 initialForce = mb.force;
        Vector3 acceleration = initialForce / mb.mass;
        Vector3 velocity = acceleration * Time.fixedDeltaTime;
        Vector3 position = transform.position + velocity * Time.fixedDeltaTime;

        for (int i = 0; i < pointCount; i++)
        {
            Vector3 gravity = 9.81f * mb.mass * Vector3.down;
            Vector3 friction = - mb.frictionCoeff * velocity;
            Vector3 force = gravity + friction;

            velocity += Time.fixedDeltaTime / mb.mass * force;
            position += velocity * Time.fixedDeltaTime;

            lr.SetPosition(i, position);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
