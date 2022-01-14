using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleBehavior : MonoBehaviour
{
    [SerializeField] GameManagerScript gameManager;

    private void OnTriggerEnter(Collider other)
    {
        gameManager?.HoleCallback(other.gameObject.GetComponent<MomentumBehavior>());
        Destroy(other.gameObject);
    }
}
