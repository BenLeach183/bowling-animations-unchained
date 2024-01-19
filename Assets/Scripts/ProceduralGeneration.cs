using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    // list of all types of track
    public GameObject[] proceduralElements;

    // pooled track objects and script references
    private GameObject[] pooledTracks;
    private FloorTrackObject[] pooledTrackScripts;

    // number of pooled objects for each type
    private byte numPooledObject = 3;

    // list of tracks in scene
    private List<int> currentTracks = new List<int>();
    // list of available tracks
    private List<int> availableTracks = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        // set size of pooled tracks, 2 instances per track type
        pooledTracks = new GameObject[proceduralElements.Length * numPooledObject];
        pooledTrackScripts = new FloorTrackObject[proceduralElements.Length * numPooledObject];

        // loop over element type
        for (int i = 0; i < proceduralElements.Length; i++)
        {
            // loop over number of objects to create
            for(int j = 0; j < numPooledObject; j++)
            {
                // instantiate the track
                GameObject track = Instantiate(proceduralElements[i], Vector3.zero, Quaternion.identity);
                
                // get the track object script
                FloorTrackObject script = track.GetComponent<FloorTrackObject>();

                // calculate id for track (position in the array)
                int trackID = (i * numPooledObject) + j;

                // store reference to the track and the script
                pooledTracks[trackID] = track;
                pooledTrackScripts[trackID] = script;

                // set the id for the track
                script.trackID = trackID;

                // add track to available tracks
                availableTracks.Add(trackID);
            }
        }

        // by default enable the first track in the list
        pooledTrackScripts[3].EnableTrack();
        availableTracks.Remove(3);
        currentTracks.Add(3);

        AddTrack();
        AddTrack();
        AddTrack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddTrack()
    {
        // if no tracks available return
        if (availableTracks.Count <= 0) return;

        // get the id of the track at the end
        int endTrackID = currentTracks[currentTracks.Count - 1];

        // pick a random track
        int trackID = availableTracks[Random.Range(0, availableTracks.Count)];

        // remove the track from available and add to current
        availableTracks.Remove(trackID);
        currentTracks.Add(trackID);

        // connect the new track to end of current tracks
        pooledTrackScripts[trackID].ConnectToPoint(pooledTrackScripts[endTrackID].endPos, pooledTrackScripts[endTrackID].endRot);
    }

    private void ReleaseFirstTrack(int trackID)
    {
        // index in the list of track collided with
        int collisionTrackIndex = currentTracks.IndexOf(trackID);

        // if the index is greater than 1, meaning theres at least 3 tracks before it
        // remove the tracks until theres only 2
        if(collisionTrackIndex > 1)
        {
            collisionTrackIndex -= 2;

            // loop through tracks to remove
            for(int i = 0; i < collisionTrackIndex; i++)
            {
                int removeTrackID = currentTracks[i];

                pooledTrackScripts[removeTrackID].Despawn();
                currentTracks.Remove(removeTrackID);
                availableTracks.Add(removeTrackID);
            }
        }
    }

    public void CollidedWithTrack(int trackID)
    {
        ReleaseFirstTrack(trackID);
        AddTrack();
    }
}
