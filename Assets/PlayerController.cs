using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 CurrentMoveDirection = Vector3.forward, CurrentUpDirection = Vector3.up , RightMoveDirection = Vector3.right, InputDirection = Vector3.zero;
    public Vector3 TargetMoveDirection = Vector3.forward, TargetUpDirection = Vector3.up;

    public float speed;

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

        Vector3 RelativeUpVel = Vector3.Scale(Rigidbody.velocity,CurrentUpDirection);
        RelativeUpVel = (CurrentUpDirection * -9.8f * speed * 30 * Time.deltaTime);// RelativeUpVel + (CurrentUpDirection * -9.8f);

        Force.force = RelativeUpVel + (CurrentMoveDirection * speed * 5) + (InputDirection * 2);

        Rigidbody.velocity = Vector3.ClampMagnitude(Rigidbody.velocity, speed );
        InputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        CurrentMoveDirection = Vector3.Lerp(CurrentMoveDirection, TargetMoveDirection, Time.deltaTime * speed);
        CurrentUpDirection = Vector3.Lerp(CurrentUpDirection, TargetUpDirection, Time.deltaTime * speed);

    }
}
