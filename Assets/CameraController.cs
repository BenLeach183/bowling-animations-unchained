using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;
    void Update()
    {
        transform.position = Player.transform.position;
        transform.rotation = Quaternion.LookRotation(Player.GetComponent<PlayerController>().CurrentMoveDirection, Player.GetComponent<PlayerController>().CurrentUpDirection);
    }
}
