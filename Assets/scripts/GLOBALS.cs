using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals {
    public const float EPSI = 0.00001f;
    public const float EPSI_ANGLE = 5.0f;

    //epsilon used to compare distance between 2 gameobjects

    public const float CROUCHIDLE_EPSI = 0.2f;
    public const float DISTANCE_EPSI = 1.1f;

    public const float ELAPSED_TIME = 0.002f;

    public const float navigationDelayTime = 0.2f;

    public static float DistanceV3xz(Vector3 a, Vector3 b)
    {
        float x = a.x - b.x;
        float z = a.z - b.z;
        return Mathf.Sqrt((x * x) + (z * z));
    }
}
