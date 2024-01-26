using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TurningVolumeScript : MonoBehaviour
{
    public List<Transform> PlayerMarkers, CameraMarkers;
    GameObject Player;
    Vector3 TargetDirection = Vector3.forward, TargetUpDirection = Vector3.up;
    int LastClosest = 0, NextClosest = 0;
    float BaseSpeed = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Player = other.gameObject;
            BaseSpeed = Player.GetComponent<PlayerController>().MaxSpeed;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {


            if (PlayerMarkers.Count > 1)
            {


                int templ = 0;
                for (int i = 0; i < PlayerMarkers.Count; i++)
                {
                    if (Vector3.Distance(Player.transform.position, PlayerMarkers[i].position) < Vector3.Distance(Player.transform.position, PlayerMarkers[templ].position))
                    {
                        templ = i;
                    }
                }
                int tempn = -1;
                for (int i = 0; i < PlayerMarkers.Count; i++)
                {
                    if (i != templ)
                    {
                        if (tempn == -1)
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
                //Debug.Log("" + LastClosest + " :: " + NextClosest);


                float TotalDistance = Vector3.Distance(Player.transform.position, PlayerMarkers[LastClosest].position) + Vector3.Distance(Player.transform.position, PlayerMarkers[NextClosest].position);

                if (Vector3.Distance(Player.transform.position, PlayerMarkers[LastClosest].position) > Vector3.Distance(PlayerMarkers[LastClosest].position, PlayerMarkers[NextClosest].position))
                {
                    TargetDirection = Vector3.Slerp(PlayerMarkers[LastClosest].forward, PlayerMarkers[NextClosest].forward, 1);
                    TargetUpDirection = Vector3.Slerp(PlayerMarkers[LastClosest].up, PlayerMarkers[NextClosest].up, 1);

                }
                else
                {
                    TargetDirection = Vector3.Slerp(PlayerMarkers[LastClosest].forward, PlayerMarkers[NextClosest].forward, Vector3.Distance(Player.transform.position, PlayerMarkers[LastClosest].position) / TotalDistance);
                    TargetUpDirection = Vector3.Slerp(PlayerMarkers[LastClosest].up, PlayerMarkers[NextClosest].up, Vector3.Distance(Player.transform.position, PlayerMarkers[LastClosest].position) / TotalDistance);
                }
                /*
                Vector3 Cross1 = Vector3.Cross(TargetDirection.normalized, TargetUpDirection.normalized).normalized;
                Vector3 Cross2 = Cross1 * -1;
                Vector3 d = Player.GetComponent<Rigidbody>().velocity.normalized;

                if (Vector3.Dot(d, Cross1) < 0)
                {

                    TargetDirection = d - (2 * Vector3.Dot(d, Cross1) * Cross1);
                }
                else
                {
                    TargetDirection = d - (2 * Vector3.Dot(d, Cross2) * Cross2);
                }
                */
                Player.GetComponent<PlayerController>().speed = Mathf.Lerp(BaseSpeed / 5, BaseSpeed, (Vector3.Dot(Player.GetComponent<Rigidbody>().velocity.normalized, TargetDirection) + 1) / 2);

                Vector3 PlayerVel = Player.GetComponent<Rigidbody>().velocity.normalized;
                float PlayerSpeed = Player.GetComponent<Rigidbody>().velocity.magnitude;

                if ((TargetDirection + (PlayerVel * -1)).magnitude > 1.25f)
                {
                    Player.GetComponent<Rigidbody>().AddForce((TargetDirection + (PlayerVel * -1)) * PlayerSpeed * 2);
                }


                Player.GetComponent<PlayerController>().TargetMoveDirection = TargetDirection;
                Player.GetComponent<PlayerController>().TargetUpDirection = TargetUpDirection;
            }
            else
            {
                Player.GetComponent<PlayerController>().speed = Mathf.Lerp(BaseSpeed / 5, BaseSpeed, (Vector3.Dot(Player.GetComponent<Rigidbody>().velocity.normalized, TargetDirection) + 1) / 2);

                Vector3 PlayerVel = Player.GetComponent<Rigidbody>().velocity.normalized;
                float PlayerSpeed = Player.GetComponent<Rigidbody>().velocity.magnitude;

                if ((TargetDirection + (PlayerVel * -1)).magnitude > 1.25f)
                {
                    Player.GetComponent<Rigidbody>().AddForce((TargetDirection + (PlayerVel * -1)) * PlayerSpeed * 2);
                }
                Player.GetComponent<PlayerController>().TargetMoveDirection = PlayerMarkers[0].forward;
                Player.GetComponent<PlayerController>().TargetUpDirection = PlayerMarkers[0].up;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Player.GetComponent<PlayerController>().speed = BaseSpeed;
            Player.GetComponent<PlayerController>().TargetMoveDirection = PlayerMarkers[PlayerMarkers.Count - 1].forward;
            Player.GetComponent<PlayerController>().TargetUpDirection = PlayerMarkers[PlayerMarkers.Count - 1].up;
        }

    }


}
