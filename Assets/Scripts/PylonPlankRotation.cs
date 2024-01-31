using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonPlankRotation : MonoBehaviour
{
    public GameObject pylonPlanks;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //in the jumping hoops prefab, there are some planks which go across the electric pylon,
            //this script rotates them -90 degrees once the player passes them, so when the player
            //comes back through the pylon on the other angle, they're aligned.
            pylonPlanks.transform.Rotate(new Vector3(0, -90, 0));
        }
    }

    public void Reset()     //This is for the prefab to reset the rotation as we use object pooling.
    {
        pylonPlanks.transform.localRotation = Quaternion.identity;
    }
}
