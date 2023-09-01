using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //creating the wheel colliders that are connected to each one of the four wheels
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider backLeft;
    [SerializeField] WheelCollider backRight;

    //creating the transform that allows for the wheels to be rotated in real-time
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform backLeftTransform;
    [SerializeField] Transform backRightTransform;

    //establishing set values for both the speed and the force the car brakes at (these values can be edited in Unity)
    public float acceleration = 500f;
    public float brakingForce = 300f;
    public float maxTurningRadius = 15f;

    //values that are determined based on the current key input by the player, starting value as 0 since nothing is pressed from the start
    private float currentSpeed = 0f;
    private float currentBrakeForce = 0f;
    private float currentTurningRadius = 0f;

    private void FixedUpdate()
    {
        //gets forward and backward user inputs when the "W" and "S" keys are pressed
        currentSpeed = acceleration * Input.GetAxis("Vertical");

        //once the "Spacebar" is pressed by the player, the brake will be applied to the car
        if(Input.GetKey(KeyCode.Space))
        {
            currentBrakeForce = brakingForce;
        }

        //car will maintain it's current speed if the brake is not pressed
        else
        {
            currentBrakeForce = 0f;
        }

        //acceleration will only be applied to the front wheels of the car (not four-wheel drive)
        frontLeft.motorTorque = currentSpeed;
        frontRight.motorTorque = currentSpeed;

        //brakes will be applied to all wheels of the car
        frontLeft.brakeTorque = currentBrakeForce;
        frontRight.brakeTorque = currentBrakeForce;
        backLeft.brakeTorque = currentBrakeForce;
        backRight.brakeTorque = currentBrakeForce;

        //gets left and right user inputs when the "A" and "D" keys are pressed
        currentTurningRadius = maxTurningRadius * Input.GetAxis("Horizontal");
        
        //turning will only be applied to the front wheels
        frontLeft.steerAngle = currentTurningRadius;
        frontRight.steerAngle = currentTurningRadius;

        //updating the wheel meshes on the car to reflect the rotation/position
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
        UpdateWheel(backRight, backRightTransform);
    }

    void UpdateWheel(WheelCollider col, Transform trans)
    {
        //gets the current state of the wheel and whether or not it has turned
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        //setting the wheel transformation
        trans.position = position;
        trans.rotation = rotation;
    }
}
