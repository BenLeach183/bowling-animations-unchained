using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PhysicsObjectsScript : MonoBehaviour
{
    ConstantForce force;
    Vector3 BasePos;
    Quaternion BaseRot;

    private void Start()
    {
        force = GetComponent<ConstantForce>();
        force.force = transform.rotation * Vector3.forward * -9.8f;
        BasePos = transform.position;
        BaseRot = transform.rotation;
    }

    public void Replace()
    {
        transform.position = BasePos;
        transform.rotation = BaseRot;
    }
}
