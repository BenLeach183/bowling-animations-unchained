using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 CurrentMoveDirection = Vector3.forward, CurrentUpDirection = Vector3.up , RightMoveDirection = Vector3.right, InputDirection = Vector3.zero;
    public Vector3 TargetMoveDirection = Vector3.forward, TargetUpDirection = Vector3.up;

    Vector2 InputVector = Vector2.zero;

    ConstantForce Force;
    Rigidbody Rigidbody;

    private void Start()
    {
        Force = GetComponent<ConstantForce>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RightMoveDirection = Vector3.Cross(CurrentUpDirection, CurrentMoveDirection);

        InputDirection = ((InputVector.x * RightMoveDirection) + (InputVector.y * CurrentMoveDirection)).normalized;

        Rigidbody.velocity = (CurrentUpDirection * -9.8f) + (CurrentMoveDirection * 3) + (InputDirection * 2);

        InputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        CurrentMoveDirection = Vector3.Lerp(CurrentMoveDirection, TargetMoveDirection, Time.deltaTime);
        CurrentUpDirection = Vector3.Lerp(CurrentUpDirection, TargetUpDirection, Time.deltaTime);

    }
}
