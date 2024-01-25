using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricCableSpline : MonoBehaviour
{
    public Transform startPoint, endPoint;
    private Vector3 a, b, c;
    private LineRenderer lineRenderer;
    int segments = 6;
    private List<Vector3> points = new List<Vector3>();

    bool Initialised = false;

    void Start(){
        Initialise();
    }

    void Initialise()
    {
        if (Initialised) return;
        lineRenderer = GetComponent<LineRenderer>();
        a = startPoint.position;
        c = endPoint.position;

        Vector3 droop = (a + ((c - a) / 2)) - (Vector3.up * (Vector3.Distance(a, c) / 10));
        b = droop;

        RasterizeBezier();

        lineRenderer.positionCount = segments + 1;
        lineRenderer.SetPositions(points.ToArray());
        Initialised = true;

    }

    public void ReusedUpdate()
    {
        if(!Initialised){Initialise();}
        //Debug.Log("test");
        lineRenderer = GetComponent<LineRenderer>();
        a = startPoint.position;
        c = endPoint.position;

        Vector3 droop = (a + ((c - a) / 2)) - (Vector3.up * (Vector3.Distance(a, c) / 10));
        b = droop;

        RasterizeBezier();

        lineRenderer.positionCount = segments + 1;
        lineRenderer.SetPositions(points.ToArray());
    }

    private void RasterizeBezier()
    {
        for (int i = 0; i <= segments; i++)
        {
            points.Add(GetValue((float)i / segments));
        }
    }

    Vector3 GetValue(float t)
    {
        Vector3 ab, bc;

        ab = GetT(a, b, t);
        bc = GetT(b, c, t);
        return GetT(ab, bc, t);
    }

    Vector3 GetT(Vector3 a, Vector3 b, float t)
    {
        return a + ((b - a) * t);
    }
}
