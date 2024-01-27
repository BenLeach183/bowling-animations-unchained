using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;

    void Update()
    {
        transform.position = Player.transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(Player.GetComponent<PlayerController>().CurrentMoveDirection, Player.GetComponent<PlayerController>().CurrentUpDirection),Time.deltaTime * Player.GetComponent<PlayerController>().speed);

        // get the aspcet ratio of the screen
        float aspect = Screen.width / Screen.height;

        // if the screen width is less than height increase fov
        if(aspect < 1)
        {
            
            Camera.main.fieldOfView = 100;
        } else
        {
            Camera.main.fieldOfView = 60;
        }
    }
}
