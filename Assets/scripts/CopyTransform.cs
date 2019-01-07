using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform {

    private Vector3 Position;
    private Quaternion Rotation;
    private Vector3 Scale;

	public CopyTransform(Transform t)
    {
        Position = t.position;
        Rotation = t.rotation;
        Scale = t.localScale;
    }

    public Vector3 position()
    {
        return Position;
    }

    public Quaternion rotation()
    {
        return Rotation;
    }

    public Vector3 scale()
    {
        return Scale;
    }

    
}
