using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RDG;

public class BreakObjectParticle : MonoBehaviour
{
    private ParticleSystem particles;
    private BoxCollider thisCollision;
    MeshRenderer meshRenderer;
    Color transparent;
    private bool destroyed;

    public float lifespan = 0.5f;
    public bool pushPlayerBack = true;
    public float pushBackForce = -50f;

    private float t = 0;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        thisCollision = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        transparent = new Color(0, 0, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vibration.Vibrate(200);
            particles.Play();
            meshRenderer.material.color = transparent;
            thisCollision.enabled = false;
            Destroy(this.gameObject, lifespan);

            if (pushPlayerBack)
            {
                collision.gameObject.GetComponent<PlayerController>().ExtraForce += collision.gameObject.GetComponent<Rigidbody>().velocity.normalized * collision.gameObject.GetComponent<PlayerController>().speed * pushBackForce;
            }
        }
    }
}
