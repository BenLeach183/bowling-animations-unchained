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

    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshCollider meshCollider;

    Vector3 a, b, c, d;

    Vector3 CurrentGameUp;

    public List<Vector3> Positions;
    List<Quaternion> Rotations;

    int Segments = 20;

    List<GameObject> pointers;
    //List<GameObject> platforms;

    Mesh mesh;

    public bool initalised = false;

    Vector3[] Vertices;
    Vector2[] UVs;
    int[] Triangles;

    public void ReuseUpdate()
    {
        if (!initalised) { Initialise(); }

        a = Point1.transform.localPosition;
        b = Point2.transform.localPosition;
        c = Point3.transform.localPosition;
        d = Point4.transform.localPosition;

        c = c + (Vector3.right * Random.Range(-3, 3)) + (Vector3.up * Random.Range(-3, 3)) + (Vector3.forward * Random.Range(-3, 3));

        RasterizeBezier();
        UpdateMesh();
        UpdateConstrictionPoints();

        int boundaryIndex = 0;
        floorTrackObject.ClearBoundingSpheres();

        for (int i = 0; i <= Segments; i++)
        {
            pointers[i].transform.localPosition = Positions[i] + ((Rotations[i] * Vector3.up) / 10);
            pointers[i].transform.localRotation = Rotations[i];

            if (i <= 1 || i >= Segments-1) continue;
            if(i % 2 != 0) continue; 


            floorTrackObject.UpdateBoundingSpheres((Positions[i]), 3.5f, boundaryIndex);
            boundaryIndex++;
        }
    }

    private void Initialise()
    {
        if (initalised) return;

        // initialize variables
        pointers = new List<GameObject>();
        mesh = new Mesh();

        Vertices = new Vector3[(Segments + 1) * 4];//4];
        UVs = new Vector2[(Segments + 1) * 2];//4];
        Triangles = new int[(Segments) * 24];//12];

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        Transform pointerParent = new GameObject("Volume Pointers").transform;
        pointerParent.parent = this.transform;

        for (int i = 0; i <= Segments; i++)
        {
            pointers.Add(Instantiate(pointer, pointerParent));
            turningVolumeScript.PlayerMarkers.Add(pointers[i].transform);
        }

        initalised = true;
    }

    void UpdateConstrictionPoints()
    {
        floorTrackObject.UpdateEndPoint(Rotations[Rotations.Count - 1], Positions[Positions.Count - 1]);
        floorTrackObject.UpdateStartPoint(Rotations[0], Positions[0]); 
    }

    void UpdateMesh()
    {
        mesh.Clear();

        for (int i = 0; i <= Segments; i++)// * 2; i++)
        {
            Vector3 left = (Positions[i] - (Rotations[i] * Vector3.left * 3.5f));
            Vector3 right = (Positions[i] - (Rotations[i] * Vector3.right * 3.5f));

            Vector3 down = ((Rotations[i] * Vector3.down * 0.18f));
            Vector3 up = ((Rotations[i] * Vector3.up * 0.18f));

            Vertices[i * 4] = left + up;
            Vertices[(i * 4) + 1] = right + up;

            Vertices[(i * 4) + 2] = left + down;
            Vertices[(i * 4) + 3] = right + down;

            if (i < Segments)
            {
                // top
                Triangles[(i * 24)] = (i * 4);
                Triangles[(i * 24) + 1] = (i * 4) + 1;
                Triangles[(i * 24) + 2] = ((i+1) * 4);

                Triangles[(i * 24) + 3] = (i * 4) + 1;
                Triangles[(i * 24) + 4] = ((i+1) * 4) + 1;
                Triangles[(i * 24) + 5] = ((i+1) * 4);

                // bottom
                Triangles[(i * 24) + 6] = (i * 4) + 2;
                Triangles[(i * 24) + 8] = (i * 4) + 3;
                Triangles[(i * 24) + 7] = ((i + 1) * 4) + 2;

                Triangles[(i * 24) + 9] = (i * 4) + 3;
                Triangles[(i * 24) + 11] = ((i + 1) * 4) + 3;
                Triangles[(i * 24) + 10] = ((i + 1) * 4) + 2;

                // left
                Triangles[(i * 24) + 12] = (i * 4) + 2;
                Triangles[(i * 24) + 13] = (i * 4);
                Triangles[(i * 24) + 14] = ((i + 1) * 4);

                Triangles[(i * 24) + 15] = (i * 4) + 2;
                Triangles[(i * 24) + 16] = ((i + 1) * 4);
                Triangles[(i * 24) + 17] = ((i + 1) * 4) + 2;

                // right
                Triangles[(i * 24) + 18] = (i * 4) + 3;
                Triangles[(i * 24) + 20] = (i * 4) + 1;
                Triangles[(i * 24) + 19] = ((i + 1) * 4) + 1;

                Triangles[(i * 24) + 21] = (i * 4) + 3;
                Triangles[(i * 24) + 23] = ((i + 1) * 4) + 1;
                Triangles[(i * 24) + 22] = ((i + 1) * 4) + 3;

            }
        }
        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
    }

    void RasterizeBezier()
    {
        Positions = new List<Vector3>();
        Rotations = new List<Quaternion>();

        Rotations.Add(Quaternion.identity);

        for (int i = 0; i <= Segments; i++)
        {
            Positions.Add(GetValue((float)i / Segments));
            if (i != 0)
            {
                Rotations.Add(Quaternion.LookRotation((Positions[i] - Positions[i - 1]).normalized, Rotations[i - 1] * Vector3.up));
            }
        }
        
        if(Segments >= 1)
        {
            Rotations[0] = Quaternion.LookRotation((Positions[1] - Positions[0]).normalized, Rotations[0] * Vector3.up);
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
