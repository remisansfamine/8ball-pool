using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class MathsUtils
{
    static public Vector2 Reflect(in this Vector2 toReflect, in Vector2 normal)
    {
        return toReflect - 2f * Vector2.Dot(toReflect, normal) * normal;
    }
}
