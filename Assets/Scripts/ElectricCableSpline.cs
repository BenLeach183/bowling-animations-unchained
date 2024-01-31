using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricCableSpline : MonoBehaviour
{
    public Transform startPoint, endPoint;
    //a - start b - middle c - end
    private Vector3 a, b, c;
    private LineRenderer lineRenderer;
    //6 segments fits the art style
    int segments = 6;
    private List<Vector3> points = new List<Vector3>();

    bool Initialised = false;

    void Start()
    {
        Initialise();
    }

    void Initialise()
    {
        if (Initialised) return;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        a = startPoint.position;
        c =  endPoint.position;

        //Debug.Log(c);

        //Droop is the part in the middle of the cable that is closer to the ground, this equation makes the b point lower if the a and c are further apart
        Vector3 droop = (a + ((c - a) / 2)) - (transform.root.up * (Vector3.Distance(a, c) * 0.1f));
        b = droop;

        segments = (int)Mathf.Round(Vector3.Distance(a, c));

        //reusing function from beziar curve as it is very similar
        RasterizeBezier();

        //loop through positions
        lineRenderer.positionCount = segments + 1;
        lineRenderer.SetPositions(points.ToArray());
        Initialised = true;

    }

    void Update(){
        ReusedUpdate();
    }

    public void ReusedUpdate()  //reused update is in here so the prefabs can use this update as the normal update was not working with procedural generation
    {
        if (!Initialised) { Initialise(); }
        lineRenderer = GetComponent<LineRenderer>();
        a = startPoint.position;
        c =  endPoint.position;

        Vector3 droop = (a + ((c - a) / 2)) - (transform.root.up * (Vector3.Distance(a, c) * 0.1f));
        b = droop;

        RasterizeBezier();

        lineRenderer.SetPositions(points.ToArray());
    }

    private void RasterizeBezier()
    {
        points = new List<Vector3>();
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
