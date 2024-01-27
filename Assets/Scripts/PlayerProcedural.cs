using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerProcedural : MonoBehaviour
{
    public ProceduralGeneration trackGeneration;
    private int currentCollision = -1;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Track")
        {
            TrackCollision(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Track")
        {
            TrackCollision(other.gameObject);
        }
    }

    private void TrackCollision(GameObject track)
    {
        if (track.tag == "Track")
        {
            // get the track id of the collided track
            int trackId = track.GetComponent<FloorTrackObject>().trackID;

            // test if the track collision is for a new track
            if (trackId == currentCollision) return;
            currentCollision = trackId;

            // if entered a new track, update the procedural generation
            
            trackGeneration.CollidedWithTrack(trackId);
        }
    }
}
