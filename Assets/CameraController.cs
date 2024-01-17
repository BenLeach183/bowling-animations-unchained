using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;
    void Update()
    {
        transform.position = Player.transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(Player.GetComponent<Rigidbody>().velocity.normalized, Player.GetComponent<PlayerController>().CurrentUpDirection),Time.deltaTime * Player.GetComponent<PlayerController>().speed);
    }
}
