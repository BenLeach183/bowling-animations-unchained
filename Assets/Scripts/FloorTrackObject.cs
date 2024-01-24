using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    private BoundingSphereData totalBoundingSphere = new BoundingSphereData();

    // array if smaller spheres to detect if track collides with nearby tracks within bigger sphere
    [SerializeField]
    private BoundingSphereData[] boundingSpheres;


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

    //public BoxCollider overlapBoundary;

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

    public void UpdateStartPoint(Quaternion rotation, Vector3 position)
    {
        localStartPos = position;
        localStartRot = rotation;
    }

    public void UpdateEndPoint(Quaternion rotation, Vector3 position)
    {
        localEndPos = position;
        localEndRot = rotation;

        endPos = localEndPos;
        endRot = localEndRot;
    }

    public bool ConnectToPoint(Vector3 position, Quaternion rotation)
    {
        // find the rotation ( multiplying by inverse subtracts rotation)
        Quaternion rotateTo = rotation * Quaternion.Inverse(localStartRot);
        
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

        // check whether the track overlaps exisiting track
        if (CheckForOverlap()) return false;

        // if no overlap continue

        // activate the object
        this.gameObject.SetActive(true);
        trackEnabled = true;

        // return true if succesful
        return true;
    }

    // checks whether the track overlaps an exisiting track
    private bool CheckForOverlap()
    {
        LayerMask trackMask = LayerMask.GetMask("Track");
        return false;
        //return Physics.CheckBox(overlapBoundary.bounds.center, overlapBoundary.bounds.extents, transform.rotation, trackMask, QueryTriggerInteraction.Ignore);
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

        for (int i = 0; i < boundingSpheres.Length; i++)
        {
            Gizmos.DrawSphere(transform.position + boundingSpheres[i].GetPosition(), boundingSpheres[i].GetRadius());
        }
    }
}
