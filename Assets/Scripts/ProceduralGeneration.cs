using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    // list of all types of track
    public GameObject[] proceduralElements;

    // pooled track objects and script references
    private GameObject[] pooledTracks;
    private FloorTrackObject[] pooledTrackScripts;

    // number of pooled objects for each type
    private byte numPooledObject = 2;

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

                // store reference to the track and the script
                pooledTracks[(i * numPooledObject) + j] = track;
                pooledTrackScripts[(i * numPooledObject) + j] = track.GetComponent<FloorTrackObject>();
            }
        }

        pooledTrackScripts[2].EnableTrack();
        pooledTrackScripts[0].ConnectToPoint(pooledTrackScripts[2].endPos, pooledTrackScripts[2].endRot);
        pooledTrackScripts[3].ConnectToPoint(pooledTrackScripts[0].endPos, pooledTrackScripts[0].endRot);
        pooledTrackScripts[1].ConnectToPoint(pooledTrackScripts[3].endPos, pooledTrackScripts[3].endRot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
