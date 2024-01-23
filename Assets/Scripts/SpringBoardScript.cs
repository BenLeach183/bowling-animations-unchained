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
            other.transform.GetComponent<Rigidbody>().AddForce(other.transform.GetComponent<PlayerController>().CurrentUpDirection * 25000, ForceMode.Force);
            animator.SetBool("Triggered", true);
            Destroy(GetComponent<BoxCollider>());
        }
    }
}
