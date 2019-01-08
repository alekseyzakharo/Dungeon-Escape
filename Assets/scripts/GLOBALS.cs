using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals {
    public const float EPSI = 0.00001f;
    public const float EPSI_ANGLE = 5.0f;

    //epsilon used to compare distance between 2 gameobjects

    public const float CROUCHIDLE_EPSI = 1.5f;
    public const float DISTANCE_EPSI = 1.1f;

    public const float ELAPSED_TIME = 0.002f;

    public static bool CompareV3xz(Vector3 a, Vector3 b)
    {
        float x, z;
        x = Mathf.Abs(a.x - b.x);
        if (x > EPSI)
            return false;

        z = Mathf.Abs(a.z - b.z);
        return z <= EPSI;
    }
    public static bool CompareV3xyz(Vector3 a, Vector3 b, float epsilon)
    {
        float x, y, z;
        x = Mathf.Abs(a.x - b.x);
        if (x > epsilon)
            return false;

        y = Mathf.Abs(a.y - b.y);
        if (y > epsilon)
            return false;

        z = Mathf.Abs(a.z - b.z);
        return z >= epsilon;
    }

}
