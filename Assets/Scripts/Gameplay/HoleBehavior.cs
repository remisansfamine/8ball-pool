using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleBehavior : MonoBehaviour
{
    [SerializeField] PointCounterScript pointCounter;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        pointCounter?.AddPoint();
    }
}
