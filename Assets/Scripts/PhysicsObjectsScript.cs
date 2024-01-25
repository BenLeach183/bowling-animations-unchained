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

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        force = GetComponent<ConstantForce>();
        force.force = transform.parent.rotation * Vector3.up * -9.8f;
        BasePos = transform.localPosition;
        BaseRot = transform.rotation;
    }

    public void Replace()
    {
        rigidbody.velocity = Vector3.zero;
        transform.localPosition = BasePos;
        transform.rotation = BaseRot;
    }
}
