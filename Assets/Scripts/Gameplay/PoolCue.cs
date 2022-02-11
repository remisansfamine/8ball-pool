using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCue : MonoBehaviour
{
    [SerializeField] private Vector2 sensivity;
    [SerializeField] private RangeAttribute rangeRotationX = new RangeAttribute(-75f, 0f);

    private Vector2 rotation;

    void RotateCue()
    {
        Vector2 axis;
        axis.x = Input.GetAxis("Mouse X");
        axis.y = Input.GetAxis("Mouse Y");

        rotation.y = rotation.y - axis.x * sensivity.y * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x - axis.y * sensivity.x * Time.deltaTime, rangeRotationX.min, rangeRotationX.max);
        transform.eulerAngles = new Vector3(rotation.x, rotation.y, transform.eulerAngles.z);
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
            RotateCue();
    }
}
