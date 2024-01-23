using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBoardScript : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            other.transform.GetComponent<PlayerController>().ExtraForce += other.transform.GetComponent<PlayerController>().CurrentUpDirection * 500 * other.transform.GetComponent<PlayerController>().speed;
            animator.SetBool("Triggered", true);
            Destroy(GetComponent<BoxCollider>());
        }
    }
}
