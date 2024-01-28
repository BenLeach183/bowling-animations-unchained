using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public int maxBackTrackLength = 1;
    public int maxForwardTrackLength = 4;

    private int maxTotalTrackLength;

    // a track prefab that is only spawned once at the start
    public GameObject spawnTrack;

    // list of all types of track
    public GameObject[] proceduralElements;

    // pooled track objects and script references
    private GameObject[] pooledTracks;
    public FloorTrackObject[] pooledTrackScripts;

    // number of pooled objects for each type
    private byte numPooledObject = 3;

    // list of tracks in scene
    private List<int> currentTracks = new List<int>();
    // list of available tracks
    private List<int> availableTracks = new List<int>();

    public int onTrackID = 0;

    // Start is called before the first frame update
    void Start()
    {
        // store the maxmimum number of tracks
        maxTotalTrackLength = maxBackTrackLength + maxForwardTrackLength;

        // set size of pooled tracks, nPooledObjects instances per track type, + 1 for spawn track
        pooledTracks = new GameObject[(proceduralElements.Length * numPooledObject) + 1];
        pooledTrackScripts = new FloorTrackObject[(proceduralElements.Length * numPooledObject) + 1];

        // set the first element of array to the spawn prefab
        pooledTracks[0] = Instantiate(spawnTrack, Vector3.zero, Quaternion.identity);
        pooledTrackScripts[0] = pooledTracks[0].GetComponent<FloorTrackObject>();

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

                // calculate id for track (position in the array), + 1 for spawn prefab
                int trackID = (i * numPooledObject) + j + 1;

                // store reference to the track and the script
                pooledTracks[trackID] = track;
                pooledTrackScripts[trackID] = script;

                // set the id for the track
                script.trackID = trackID;

                // add track to available tracks
                availableTracks.Add(trackID);

                // initialise the track
                script.InitialiseTrack();
            }
        }

        // by default enable the first track (spawn track) in the list
        pooledTrackScripts[0].InitialiseTrack();
        pooledTrackScripts[0].EnableTrack();
        currentTracks.Add(0);
    }

    private void Update()
    {
        // check if the total number of tracks is less than max total
        if(currentTracks.Count < maxTotalTrackLength)
        {
            AddTrack();
        }
    }


    private void AddTrack()
    {
        // if no tracks available return
        if (availableTracks.Count <= 0) return;

        // get the id of the track at the end
        int endTrackID = currentTracks[currentTracks.Count - 1];

        // pick a random track
        int trackID = availableTracks[Random.Range(0, availableTracks.Count)];

        // connect the new track to end of current tracks
        pooledTrackScripts[trackID].ConnectToPoint(pooledTrackScripts[endTrackID].endPos, pooledTrackScripts[endTrackID].endRot);

        bool result = CheckOverlap(trackID);

        // if unable to place track return
        if (result)
        {
            pooledTrackScripts[trackID].Despawn();
            return;
        }

        // if track is further ahead than 1 disable turning volume
        if(currentTracks.Count >= maxBackTrackLength + 1)
        {
            pooledTrackScripts[trackID].TurningVolumeActive(false);
        }

        // remove the track from available and add to current
        availableTracks.Remove(trackID);
        currentTracks.Add(trackID);
    }

    private bool CheckOverlap(int trackID)
    {
        return CheckBigOverlap(trackID);
    }

    private bool CheckBigOverlap(int trackID)
    {
        // get the bounding sphere and position of track
        BoundingSphereData boundingSphere = pooledTrackScripts[trackID].totalBoundingSphere;
        Vector3 offset = pooledTrackScripts[trackID].transform.position;

        // loop over all active tracks
        for (int i = 0; i < currentTracks.Count; i++)
        {
            // get the track ID of other track
            int otherTrackID = currentTracks[i];

            // if the track being checked is current track continue
            if (otherTrackID == trackID) continue;

            // get the bounding sphere and position of other track
            BoundingSphereData otherBoundingSphere = pooledTrackScripts[otherTrackID].totalBoundingSphere;
            Vector3 otherOffset = pooledTrackScripts[otherTrackID].transform.position;

            // check if the bounding spheres overlap
            bool overlap = CheckSphereOverlap(boundingSphere, offset, otherBoundingSphere, otherOffset);

            // if no overlap continue the loop
            if (!overlap) continue;

            // if overlap check if the smaller bounding spheres overlap
            overlap = CheckDetailedOverlap(trackID, otherTrackID);

            // if overlapping return true
            if (overlap) return true;
        }

        // if no overlap return false
        return false;
    }

    private bool CheckDetailedOverlap(int trackID, int otherTrackID)
    {
        // get the list of detailed bounding spheres
        List<BoundingSphereData> boundingSpheres = pooledTrackScripts[trackID].boundingSpheres;
        List<BoundingSphereData> otherBoundingSpheres = pooledTrackScripts[otherTrackID].boundingSpheres;

        // get the positions of tracks
        Vector3 offset = pooledTrackScripts[trackID].transform.position;
        Vector3 otherOffset = pooledTrackScripts[otherTrackID].transform.position;

        // loop over bounding spheres in first list
        for(int i = 0; i < boundingSpheres.Count; i++)
        {
            // loop over bounding spheres in second list
            for(int j = 0; j < otherBoundingSpheres.Count; j++)
            {
                // check if the spheres intersect
                bool overlap = CheckSphereOverlap(boundingSpheres[i], offset, otherBoundingSpheres[j], otherOffset);

                // if overlap is detected return true
                if (overlap) return true;
            }
        }

        // if no overlap return false
        return false;
    }

    private bool CheckSphereOverlap(BoundingSphereData sphereA, Vector3 offsetA, BoundingSphereData sphereB, Vector3 offsetB)
    {
        // get difference between the global positioned spheres
        Vector3 difference = (sphereB.GetPosition() + offsetB) - (sphereA.GetPosition() + offsetA);
        float distanceSquared = difference.sqrMagnitude;

        float radiiSquared = (sphereA.GetRadius() + sphereB.GetRadius()) * (sphereA.GetRadius() + sphereB.GetRadius());

        return (distanceSquared <= radiiSquared);
    }


    private void ReleaseFirstTrack(int trackID)
    {
        if(!currentTracks.Contains(trackID)) { return; }

        // index in the list of track collided with
        int collisionTrackIndex = currentTracks.IndexOf(trackID);

        // remove the backTrackLength from the index
        collisionTrackIndex -= maxBackTrackLength;

        // if the index is greater than 0, meaning theres at least maxBackTrackLength+1 tracks before it
        // remove the tracks until theres only maxBackTrackLength tracks
        if (collisionTrackIndex > 0)
        {
            // loop through tracks to remove
            for(int i = 0; i < collisionTrackIndex; i++)
            {
                int removeTrackID = currentTracks[i];

                pooledTrackScripts[removeTrackID].Despawn();
                currentTracks.Remove(removeTrackID);

                // if the track isn't the spawn track add it to available tracks
                if(removeTrackID != 0) availableTracks.Add(removeTrackID);
            }
        }
    }

    public void CollidedWithTrack(int trackID)
    {
        int index = currentTracks.IndexOf(trackID);

        // if the track collider being entered isn't the next track, ignore
        if (index > maxBackTrackLength+1) return;

        // set the on track id to this track
        onTrackID = trackID;

        // enable turning volume for this track
        pooledTrackScripts[currentTracks[index]].TurningVolumeActive(true);

        // if it is, enable the turning volume for the next track
        if (currentTracks.Count > index+1)
        {
            pooledTrackScripts[currentTracks[index + 1]].TurningVolumeActive(true);
        }

        ReleaseFirstTrack(trackID);
        AddTrack();
    }
}
