using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class MathsUtils
{
    static public Vector3 Reflect(in this Vector3 toReflect, in Vector3 normal)
    {
        return toReflect - 2f * Vector3.Dot(toReflect, normal) * normal;
    }
}
