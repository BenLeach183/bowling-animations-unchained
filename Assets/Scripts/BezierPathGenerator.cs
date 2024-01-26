using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private bool initalised = false;

    private float width = 7.0f;
    private float depth = 0.34f;

    private Vector3 minimumVertex;
    private Vector3 maximumVertex;

    Vector3[] Vertices;
    Vector2[] UVs;
    int[] Triangles;

    public void ReuseUpdate()
    {
        // intialise this class if it hasnt been already
        if (!initalised) { Initialise(); }

        // get the position of the points used for bezier curve
        a = Point1.transform.localPosition;
        b = Point2.transform.localPosition;
        c = Point3.transform.localPosition;
        d = Point4.transform.localPosition;

        // add a random position to the c point
        c = c + (Vector3.right * Random.Range(-3, 3)) + (Vector3.up * Random.Range(-3, 3)) + (Vector3.forward * Random.Range(-3, 3));

        // update the bezier track
        RasterizeBezier();
        UpdateMesh();
        UpdateConstrictionPoints();
        floorTrackObject.UpdateBoundary(minimumVertex, maximumVertex);

        // update the bounding spheres (used for detecting track overlap)
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

        Vertices = new Vector3[(Segments + 1) * 16];//4];
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

    // find the bounding corners of the track
    void UpdateMinMaxBounds(Vector3 left, Vector3 right, Vector3 down, Vector3 up)
    {
        float minX = Mathf.Min(minimumVertex.x, left.x, right.x, down.x, up.x);
        float minY = Mathf.Min(minimumVertex.y, left.y, right.y, down.y, up.y);
        float minZ = Mathf.Min(minimumVertex.z, left.z, right.z, down.z, up.z);

        float maxX = Mathf.Max(maximumVertex.x, left.x, right.x, down.x, up.x);
        float maxY = Mathf.Max(maximumVertex.y, left.y, right.y, down.y, up.y);
        float maxZ = Mathf.Max(maximumVertex.z, left.z, right.z, down.z, up.z);

        minimumVertex = new Vector3(minX, minY, minZ);
        maximumVertex = new Vector3(maxX, maxY, maxZ); 
    }

    void UpdateMesh()
    {
        mesh.Clear();

        // get the platform vertices of the first point
        Vector3 left = (Positions[0] - (Rotations[0] * Vector3.left * width/2));
        Vector3 right = (Positions[0] - (Rotations[0] * Vector3.right * width/2));

        Vector3 down = ((Rotations[0] * Vector3.down * depth/2));
        Vector3 up = ((Rotations[0] * Vector3.up * depth / 2));

        // update the bounds
        minimumVertex = left;
        maximumVertex = left;
        UpdateMinMaxBounds(left, right, down, up);

        for (int i = 0; i < Segments; i++)
        {
            int vertexIndex = i * 16;

            // generate platform vertices for current point
            Vertices[vertexIndex] = left + up;  //
            Vertices[vertexIndex + 1] = right + up; //

            Vertices[vertexIndex + 2] = left + down;    //
            Vertices[vertexIndex + 3] = right + down;   //

            Vertices[vertexIndex + 4] = left + up;  //
            Vertices[vertexIndex + 5] = right + up;

            Vertices[vertexIndex + 6] = left + down;    //
            Vertices[vertexIndex + 7] = right + down;

            // get the platform vertices of the next point
            left = (Positions[i+1] - (Rotations[i + 1] * Vector3.left * width / 2));
            right = (Positions[i + 1] - (Rotations[i + 1] * Vector3.right * width / 2));

            down = ((Rotations[i + 1] * Vector3.down * depth / 2));
            up = ((Rotations[i + 1] * Vector3.up * depth / 2));

            // update the bounds
            UpdateMinMaxBounds(left, right, down, up);

            // generate platform vertices for the next point
            Vertices[vertexIndex + 8] = left + up;  //
            Vertices[vertexIndex + 9] = right + up; //

            Vertices[vertexIndex + 10] = left + down;   //
            Vertices[vertexIndex + 11] = right + down;  //

            Vertices[vertexIndex + 12] = left + up; //
            Vertices[vertexIndex + 13] = right + up;

            Vertices[vertexIndex + 14] = left + down;
            Vertices[vertexIndex + 15] = right + down;

            // create triangles
            // top
            Triangles[(i * 24)] = vertexIndex;
            Triangles[(i * 24) + 1] = vertexIndex + 1;
            Triangles[(i * 24) + 2] = vertexIndex + 8;

            Triangles[(i * 24) + 3] = vertexIndex + 1;
            Triangles[(i * 24) + 4] = vertexIndex + 9;
            Triangles[(i * 24) + 5] = vertexIndex + 8;

            // bottom
            Triangles[(i * 24) + 6] = vertexIndex + 2;
            Triangles[(i * 24) + 8] = vertexIndex + 3;
            Triangles[(i * 24) + 7] = vertexIndex + 10;

            Triangles[(i * 24) + 9] = vertexIndex + 3;
            Triangles[(i * 24) + 11] = vertexIndex + 11;
            Triangles[(i * 24) + 10] = vertexIndex + 10;

            // left
            Triangles[(i * 24) + 12] = vertexIndex + 6;
            Triangles[(i * 24) + 13] = vertexIndex + 4;
            Triangles[(i * 24) + 14] = vertexIndex + 12;

            Triangles[(i * 24) + 15] = vertexIndex + 6;
            Triangles[(i * 24) + 16] = vertexIndex + 12;
            Triangles[(i * 24) + 17] = vertexIndex + 14;

            // right
            Triangles[(i * 24) + 18] = vertexIndex + 7;
            Triangles[(i * 24) + 20] = vertexIndex + 5;
            Triangles[(i * 24) + 19] = vertexIndex + 13;

            Triangles[(i * 24) + 21] = vertexIndex + 7;
            Triangles[(i * 24) + 23] = vertexIndex + 13;
            Triangles[(i * 24) + 22] = vertexIndex + 15;
        }
        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
        mesh.RecalculateNormals();
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
