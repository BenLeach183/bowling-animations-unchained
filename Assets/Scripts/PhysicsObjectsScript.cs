using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PhysicsObjectsScript : MonoBehaviour
{
    ConstantForce force;
    Vector3 BasePos;
    Quaternion BaseRot;

    Rigidbody rigidbody;

    bool initialised = false;

    void Start(){
        Initialise();
    }
    public void Initialise(){
        if(initialised) return;

        rigidbody = GetComponent<Rigidbody>();
        force = GetComponent<ConstantForce>();
        force.force = transform.parent.up * -9.8f;
        BasePos = transform.localPosition;
        BaseRot = transform.rotation;
        initialised = true;

    }

    public void Replace()
    {
        if (!initialised) { Initialise(); }
        force.force = transform.parent.up * -9.8f;
        rigidbody.velocity = Vector3.zero;
        transform.localPosition = BasePos;
        transform.rotation = BaseRot;
    }
}
