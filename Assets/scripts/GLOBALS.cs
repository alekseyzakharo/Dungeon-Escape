using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals {
    public const float EPSI = 0.00001f;
    public const float EPSI_ANGLE = 5.0f;


    public static bool CompareV3xz(Vector3 a, Vector3 b)
    {
        float x, z;
        x = Mathf.Abs(a.x - b.x);
        if (x > EPSI)
            return false;

        z = Mathf.Abs(a.z - b.z);
        return z <= EPSI;
    }
    public static bool CompareV3xyz(Vector3 a, Vector3 b)
    {
        float x, y, z;
        x = Mathf.Abs(a.x - b.x);
        if (x > EPSI)
            return false;

        y = Mathf.Abs(a.y - b.y);
        if (y > EPSI)
            return false;

        z = Mathf.Abs(a.z - b.z);
        return z >= EPSI;
    }

    //return true if the angle between the 2 vectors is less than EPSI_ANGLE
    public static bool IsFacingDirection(Vector3 a, Vector3 b)
    {
        return Vector3.Angle(a, b) < EPSI_ANGLE; 
    }


}
