using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class randomBowl : MonoBehaviour
{
    public GameObject ball;
    public GameObject respawnBall;
    public GameObject ballSpotLight;
    public GameObject rollingSoundObject;
    public GameObject strikeSoundObject;

    public Vector3[] laneStartLocations;

    public float bowlSpeed = 10000.0f;

    private float t = 0;

    private Rigidbody rb;

    private SphereCollider ballCollider;
    private BoxCollider ballRespawnCollider;

    private bool ballRespawnTimer = false;

    AudioSource ballRolling;
    AudioSource strike;

    private void Start()
    {
        rb = ball.GetComponent<Rigidbody>();

        ballCollider = ball.GetComponent<SphereCollider>();

        ballRespawnCollider = respawnBall.GetComponent<BoxCollider>();

        ball.transform.position = laneStartLocations[Random.Range(0, 5)];
        ballSpotLight.transform.position = new Vector3(ball.transform.position.x, 2.6f, ball.transform.position.z);

        ballRolling = rollingSoundObject.GetComponent<AudioSource>();
        strike = strikeSoundObject.GetComponent<AudioSource>();

        ballRolling.Stop();
        strike.Stop();

        ballRolling.playOnAwake = false;
        strike.playOnAwake = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ballRespawnTimer)
        {
            if (t > 2.0f)
            {
                rb.velocity = Vector3.zero;
                ball.transform.position = laneStartLocations[Random.Range(0, 5)];
                ballSpotLight.SetActive(true);
                ballSpotLight.transform.position = new Vector3(ball.transform.position.x, 2.6f, ball.transform.position.z);
                ballRespawnTimer = false;
                t = 0;
            }
            else
            {
                t += Time.deltaTime;
            }
        }
        else
        {
            if (rb.velocity.z <= 0)
            {
                rb.AddForce(0, 0, bowlSpeed);
                ballRolling.Play();
            }
            if (ballCollider.bounds.Intersects(ballRespawnCollider.bounds))
            {
                ballSpotLight.SetActive(false);
                ballRespawnTimer = true;
                strike.Play();
                ballRolling.Stop();
            }
            ballSpotLight.transform.position = new Vector3(ball.transform.position.x, 2.6f, ball.transform.position.z);
        }
    }
}
