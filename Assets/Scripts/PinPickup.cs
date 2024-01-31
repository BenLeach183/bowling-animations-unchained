using RDG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinPickup : MonoBehaviour
{
    private GameManager gameManager;

    Vector3 LocalPosReset;
    Quaternion LocalRotReset;
    private bool Initialized = false;
    AudioManager audioManager;

    private bool Once = false;
    void Initialize(){
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        LocalPosReset = transform.localPosition;
        LocalRotReset = transform.localRotation;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        Initialized = true;
    }

    public void ReuseUpdate(){
        if(!Initialized){Initialize();}

        Once = false;
        transform.localPosition = LocalPosReset;
        transform.localRotation = LocalRotReset;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer("PinReady");
    }

    void OnCollisionEnter(Collision collision){
        if(collision.transform.tag == "Player" && !Once){
            audioManager.playOneSFX(2);
            Once = true;
            gameObject.layer = LayerMask.NameToLayer("PinUsed");
            gameManager.ScoreMultiplier += 1;
            Vibration.Vibrate(100);
        }
    }
}
