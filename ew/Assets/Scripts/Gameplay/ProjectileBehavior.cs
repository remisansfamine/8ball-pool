using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    MomentumBehavior mb = null;
    TrajectoryComponent tc = null;

    [SerializeField] private Vector3 initialForce;

    // Start is called before the first frame update
    void Awake()
    {
        mb = GetComponent<MomentumBehavior>();
        tc = GetComponent<TrajectoryComponent>();
    }

    private void Start()
    {
        mb.AddForce(initialForce);
        tc.ComputeTrajectory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
