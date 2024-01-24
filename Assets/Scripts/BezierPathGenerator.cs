using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPathGenerator : MonoBehaviour
{
    public GameObject Point1, Point2, Point3, Point4;

    public FloorTrackObject floorTrackObject;
    public TurningVolumeScript turningVolumeScript;
    public GameObject pointer;
    public GameObject platform;
    Vector3 a, b, c, d;

    Vector3 CurrentGameUp;

    public List<Vector3> Positions;
    List<Quaternion> Rotations;

    int Segments = 20;

    List<GameObject> pointers;
    //List<GameObject> platforms;

    Mesh mesh;



    Vector3[] Vertices;
    Vector2[] UVs;
    int[] Triangles;

    void Start()
    {
        Positions = new List<Vector3>();
        Rotations = new List<Quaternion>();
        pointers = new List<GameObject>();
        mesh = new Mesh();

        Vertices = new Vector3[(Segments + 1) * 2];//4];
        UVs = new Vector2[(Segments + 1) * 2];//4];
        Triangles = new int[(Segments) * 6];//12];
        //platforms = new List<GameObject>();

        

        a = Point1.transform.position;
        b = Point2.transform.position;
        c = Point3.transform.position;
        d = Point4.transform.position;

        c = c + (Vector3.right * Random.Range(-3, 3)) + (Vector3.up * Random.Range(-3, 3)) + (Vector3.forward * Random.Range(-3, 3));

        RasterizeBezier();
        UpadateMesh();
        UpdateConstrictionPoints();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        if (pointers.Count <= 0)
        {
            for (int i = 0; i <= Segments; i++)
            {
                //platforms.Add(Instantiate(platform, Positions[i], Rotations[i]));
                pointers.Add(Instantiate(pointer, Positions[i] + ((Rotations[i] * Vector3.up) / 10), Rotations[i]));
                turningVolumeScript.PlayerMarkers.Add(pointers[i].transform);
            }
        }
        else
        {
            for (int i = 0; i <= Segments; i++)
            {
                pointers[i].transform.position = Positions[i] + ((Rotations[i] * Vector3.up) / 10);
                pointers[i].transform.rotation = Rotations[i];
            }
        }

    }

    void Update()
    {
        a = Point1.transform.position;
        b = Point2.transform.position;
        c = Point3.transform.position;
        d = Point4.transform.position;

        RasterizeBezier();
        UpadateMesh();
        UpdateConstrictionPoints();
        for (int i = 0; i <= Segments; i++)
        {
            pointers[i].transform.position = Positions[i] + ((Rotations[i] * Vector3.up) / 10);
            pointers[i].transform.rotation = Rotations[i];
        }
        
    }
    void UpdateConstrictionPoints()
    {
        floorTrackObject.UpdateEndPoint(Rotations[Rotations.Count - 1], Positions[Positions.Count - 1]);
    }

    void UpadateMesh()
    {
        for (int i = 0; i <= Segments; i++)// * 2; i++)
        {
            Vertices[i * 2] = transform.parent.worldToLocalMatrix * ((Positions[i] - transform.position) - (Rotations[i] * -Vector3.right * 3));
            Vertices[(i * 2) + 1] = transform.parent.worldToLocalMatrix * ((Positions[i] - transform.position) - (Rotations[i] * Vector3.right * 3));

            if (i < Segments)
            {
                Triangles[(i * 6)] = (i * 2);
                Triangles[(i * 6) + 1] = (i * 2) + 1;
                Triangles[(i * 6) + 2] = (i * 2) + 2;
                Triangles[(i * 6) + 3] = (i * 2) + 1;
                Triangles[(i * 6) + 4] = (i * 2) + 3;
                Triangles[(i * 6) + 5] = (i * 2) + 2;
            }
        }
        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
    }

    void RasterizeBezier()
    {
        Positions = new List<Vector3>();
        Rotations = new List<Quaternion>();
        for (int i = 0; i <= Segments; i++)
        {
            Positions.Add(GetValue((float)i / Segments));
            if (i != 0)
            {
                Rotations.Add(Quaternion.LookRotation((Positions[i] - Positions[i - 1]).normalized, Rotations[i - 1] * Vector3.up));
            }
            else
            {
                Rotations.Add(Point1.transform.parent.rotation);
            }
        }
    }

    Vector3 GetValue(float t)
    {
        Vector3 ab, bc, cd, abc, bcd;

        ab = GetT(a, b, t);
        bc = GetT(b, c, t);
        cd = GetT(c, d, t);
        abc = GetT(ab, bc, t);
        bcd = GetT(bc, cd, t);
        return GetT(abc, bcd, t);
    }

    Vector3 GetT(Vector3 a, Vector3 b, float t)
    {
        return a + ((b - a) * t);
    }

    void OnDestroy()
    {
        foreach (GameObject item in pointers)
        {
            Destroy(item);
        }
    }
}
