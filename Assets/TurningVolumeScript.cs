using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TurningVolumeScript : MonoBehaviour
{
    public List<Transform> PlayerMarkers, CameraMarkers;
    GameObject Player;
    Vector3 TargetDirection = Vector3.forward;
    int LastClosest = 0, NextClosest = 0;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            Player = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        int templ = 0;
        for(int i = 0;i < PlayerMarkers.Count; i++)
        {
            if (Vector3.Distance(Player.transform.position,PlayerMarkers[i].position) < Vector3.Distance(Player.transform.position, PlayerMarkers[templ].position))
            {
                templ = i;
            }
        }
        int tempn = -1;
        for(int i = 0; i < PlayerMarkers.Count; i++)
        {
            if(i != templ)
            {
                if(tempn == -1)
                {
                    tempn = i;
                }
                if (Vector3.Distance(Player.transform.position, PlayerMarkers[i].position) < Vector3.Distance(Player.transform.position, PlayerMarkers[tempn].position))
                {
                    tempn = i;
                }
            }

        }
        LastClosest = templ;
        NextClosest = tempn;

        //Debug.Log("" + LastClosest + " : " + NextClosest);
        
        if (NextClosest < LastClosest)
        {
            int x = NextClosest;
            NextClosest = LastClosest;
            LastClosest = x;
            
        }
        Debug.Log("" + LastClosest + " :: " + NextClosest);
        

        float TotalDistance = Vector3.Distance(Player.transform.position, PlayerMarkers[LastClosest].position) + Vector3.Distance(Player.transform.position, PlayerMarkers[NextClosest].position);

        if(Vector3.Distance(Player.transform.position, PlayerMarkers[LastClosest].position) > Vector3.Distance(PlayerMarkers[LastClosest].position, PlayerMarkers[NextClosest].position))
        {
            TargetDirection = Vector3.Slerp(PlayerMarkers[LastClosest].forward, PlayerMarkers[NextClosest].forward, 1);

        }
        else
        {
            TargetDirection = Vector3.Slerp(PlayerMarkers[LastClosest].forward, PlayerMarkers[NextClosest].forward, Vector3.Distance(Player.transform.position, PlayerMarkers[LastClosest].position) / TotalDistance);
        }
        Player.GetComponent<PlayerController>().TargetMoveDirection = TargetDirection;
    }


}
