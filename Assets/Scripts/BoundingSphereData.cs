using System;
using UnityEditorInternal;
using UnityEngine;

[Serializable]
public class BoundingSphereData
{
    [SerializeField]
    private Vector3 position;

    [SerializeField]
    private float radius;

    public BoundingSphereData()
    {
        this.position = new Vector3(0, 0, 0);
        this.radius = 1;
    }

    public BoundingSphereData(Vector3 position, float radius)
    {
        this.position = position;
        this.radius = radius;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public float GetRadius()
    {
        return radius;
    }
}
