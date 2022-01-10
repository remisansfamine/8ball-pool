using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public int score;

    [SerializeField] public MomentumBehavior whiteBall;

    [SerializeField] private float force = 1f;
    [SerializeField] private float maxSpeed = 1f;

    Vector3 pushOrigin;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
                pushOrigin = hit.point;
        }

        if (Input.GetButtonUp("Shoot"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;

            Vector3 direction = pushOrigin - hit.point;
            Vector3 forceVec = new Vector3(direction.x * force, 0f, direction.z * force);

            whiteBall.rb.velocity = Vector3.ClampMagnitude(forceVec, maxSpeed);
        }
    }
}
