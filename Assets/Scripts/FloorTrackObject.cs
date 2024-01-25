using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
  Each track object has this script
  it stores the position and rotation
  of the track, and will connect to other
  tracks.
 */

public class FloorTrackObject : MonoBehaviour
{
    [SerializeField]
    // one sphere that covers the entire object (for optimization)
    public BoundingSphereData totalBoundingSphere = new BoundingSphereData();

    // array if smaller spheres to detect if track collides with nearby tracks within bigger sphere
    [SerializeField]
    public List<BoundingSphereData> boundingSpheres;

    // whether the track has been connected to the next track
    public bool connected = false;

    // whether the track is enabled
    public bool trackEnabled = false;

    // store the ID of the track
    public int trackID = 0;

    // store the local end and start pos/rot of track
    private Vector3 localStartPos;
    private Vector3 localEndPos;

    private Quaternion localStartRot;
    private Quaternion localEndRot;

    // store the actual end pos/rot of track
    public Vector3 endPos;
    public Quaternion endRot;

    // store the current rotation of the object
    private Quaternion currentRotation = Quaternion.identity;

    public UnityEvent trackAdded;

    // update the default values
    public void Awake()
    {
        this.gameObject.SetActive(false);

        localStartPos = transform.Find("procedural_start").position;
        localStartRot = transform.Find("procedural_start").rotation;

        localEndPos = transform.Find("procedural_end").position;
        localEndRot = transform.Find("procedural_end").rotation;

        endPos = localEndPos;
        endRot = localEndRot;

    }

    public void UpdateBoundingSpheres(Vector3 position, float radius, int index){
        if(index >= boundingSpheres.Count)
        {
            boundingSpheres.Add(new BoundingSphereData());
            boundingSpheres[boundingSpheres.Count - 1].SetPosition(position);
            boundingSpheres[boundingSpheres.Count - 1].SetRadius(radius);
        }
        else
        {
            boundingSpheres[index].SetPosition(position);
            boundingSpheres[index].SetRadius(radius);
        }
    }

    public void UpdateStartPoint(Quaternion rotation, Vector3 position)
    {
        localStartPos = position;
        localStartRot = rotation;

        transform.Find("procedural_start").localPosition = position;
        transform.Find("procedural_start").localRotation = rotation;
    }

    public void UpdateEndPoint(Quaternion rotation, Vector3 position)
    {
        localEndPos = position;
        localEndRot = rotation;


        transform.Find("procedural_end").localPosition = position;
        transform.Find("procedural_end").localRotation = rotation;
    }

    public bool ConnectToPoint(Vector3 position, Quaternion rotation)
    {
        // reset the bounding sphere's rotation
        Quaternion inverseCurrent = Quaternion.Inverse(currentRotation);
        totalBoundingSphere.Rotate(inverseCurrent);
        for (int i = 0; i < boundingSpheres.Count; i++)
        {
            boundingSpheres[i].Rotate(inverseCurrent);
        }

        // invoke the track added event
        trackAdded.Invoke();

        // find the rotation ( multiplying by inverse subtracts rotation)
        Quaternion rotateTo = rotation * Quaternion.Inverse(localStartRot);

        // store the current rotation
        currentRotation = rotateTo;

        // set the bounding sphere's rotation
        totalBoundingSphere.Rotate(currentRotation);
        for (int i = 0; i < boundingSpheres.Count; i++)
        {
            boundingSpheres[i].Rotate(currentRotation);
        }

        // update the rotation
        this.transform.rotation = rotateTo;

        // calculate position to move to
        // the local start pos is the offset from objects origin
        // rotate the local pos
        Vector3 newPos = position - (rotateTo*localStartPos);
        this.transform.position = newPos;

        // update the new end position and rotation
        endPos = newPos + (rotateTo*localEndPos);
        endRot = rotateTo*localEndRot;

        // activate the object
        this.gameObject.SetActive(true);
        trackEnabled = true;



        // return true if succesful
        return true;
    }

    public void EnableTrack()
    {
        trackEnabled = true;
        this.gameObject.SetActive(true);
    }

    public void Despawn()
    {
        connected = false;
        trackEnabled = false;
        this.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.3f, 1.0f, 0.0f, 1.0f);
        Gizmos.DrawWireSphere(transform.position + totalBoundingSphere.GetPosition(), totalBoundingSphere.GetRadius());


        Gizmos.color = new Color(1.0f, 0.2f, 0.0f, 0.5f);

        for (int i = 0; i < boundingSpheres.Count; i++)
        {
            Gizmos.DrawSphere(transform.position + boundingSpheres[i].GetPosition(), boundingSpheres[i].GetRadius());
        }
    }
}
