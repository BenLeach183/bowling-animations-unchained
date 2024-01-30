using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 CurrentMoveDirection = Vector3.forward, CurrentUpDirection = Vector3.up , RightMoveDirection = Vector3.right, InputDirection = Vector3.zero;
    public Vector3 TargetMoveDirection = Vector3.forward, TargetUpDirection = Vector3.up;

    public SaveManager saveManager;

    public Vector3 ExtraForce = Vector3.zero;

    public float speed;
    public float MaxSpeed;

    bool firstInput = true;
    public bool onTrack = true;

    public Vector2 InputVector = Vector2.zero;

    public Vector2 JoystickPivot;

    ConstantForce Force;
    Rigidbody Rigidbody;

    private void Start()
    {
        Force = GetComponent<ConstantForce>();
        Rigidbody = GetComponent<Rigidbody>();
        speed = MaxSpeed;

        CurrentMoveDirection = transform.forward;
        CurrentUpDirection = transform.up;
        RightMoveDirection = transform.right;

        TargetMoveDirection = transform.forward;
        TargetUpDirection = transform.up;
    }

    private void FixedUpdate()   
    {
        ExtraForce -= ExtraForce / 10f;

        RightMoveDirection = Vector3.Cross(CurrentUpDirection, CurrentMoveDirection);

        InputDirection = ((InputVector.x * RightMoveDirection) + (InputVector.y * CurrentMoveDirection)).normalized;

        Vector3 RelativeUpVel = Vector3.Scale(Rigidbody.velocity,CurrentUpDirection);
        RelativeUpVel = (CurrentUpDirection * -9.8f * speed * 1.5f);// RelativeUpVel + (CurrentUpDirection * -9.8f);

        Force.force = RelativeUpVel + (CurrentMoveDirection * speed * 50) + (InputDirection * 20 * speed) + ExtraForce;

        Rigidbody.velocity = Vector3.ClampMagnitude(Rigidbody.velocity, speed);

        CurrentMoveDirection = Vector3.Lerp(CurrentMoveDirection, TargetMoveDirection, 0.01f * speed);
        CurrentUpDirection = Vector3.Lerp(CurrentUpDirection, TargetUpDirection, 0.01f * speed);
    }

    private void Update()
    {

       

        InputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        int inputType = saveManager.playerSave.controllerModes;
        if(inputType == 0)
        {
            TapInput();
        }
        else if(inputType == 1)
        {
            JoystickInput();
        }
        else
        {
            TiltInput();
        }
    }

    private void JoystickInput(){
        if(Input.touchCount > 0){
            if(firstInput){
                JoystickPivot = Input.mousePosition/100;
                firstInput = false;
            }

            InputVector = Vector2.ClampMagnitude((Vector2)Input.mousePosition/100 - JoystickPivot, 1f);
        }
        else{
            firstInput = true;
            JoystickPivot = Vector2.zero;
        }
    }

    private void TiltInput()
    {
        InputVector = Vector2.ClampMagnitude((Vector2)Input.acceleration, 1f);
    }

    private void TapInput()
    {
        if (Input.touchCount > 0)
        {
            InputVector = Vector2.ClampMagnitude((Vector2)Input.mousePosition - new Vector2(Screen.width/2, Screen.height/2), 1f);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        onTrack = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        onTrack = false;
    }
}
