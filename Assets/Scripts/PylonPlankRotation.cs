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
            pylonPlanks.transform.Rotate(new Vector3(0, -90, 0));
        }
    }

    public void Reset()
    {
        pylonPlanks.transform.localRotation = Quaternion.identity;
    }
}
