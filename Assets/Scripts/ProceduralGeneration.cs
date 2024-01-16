using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    // list of all types of track
    public GameObject[] proceduralElements;

    // pooled track objects
    private GameObject[] pooledTracks;

    // number of pooled objects for each type
    private byte numPooledObject = 2;

    // Start is called before the first frame update
    void Start()
    {
        // set size of pooled tracks, 2 instances per track type
        pooledTracks = new GameObject[proceduralElements.Length * numPooledObject];

        // loop over element type
        for(int i = 0; i < proceduralElements.Length; i++)
        {
            // loop over number of objects to create
            for(int j = 0; j < numPooledObject; j++)
            {
                // instantiate the track
                pooledTracks[(i * numPooledObject) + j] = Instantiate(proceduralElements[i], Vector3.zero, Quaternion.identity);
            }
        }

        pooledTracks[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
