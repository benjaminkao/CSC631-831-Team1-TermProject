using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   private float horizontalInput; 
    private float verticalInput; 
    private float steerAngle; 

    public WheelCollider wheelFlCollider; 
    public WheelCollider wheelFrCollider; 
    public WheelCollider wheelRlCollider; 
    public WheelCollider wheelRrCollider; 
    public Transform wheelFl;
    public Transform wheelFr;
    public Transform wheelRl;
    public Transform wheelRr;

    public float maxSteeringAngle = 30f;
    public float motorForce = 50f;
      
    

    private void FixedUpdate()
    {
        GetInput(); 
        HandleMotor();
        HandleSteering(); 
        UpdateWheels(); 
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

    }

    private void HandleSteering()
    {
        steerAngle = maxSteeringAngle * horizontalInput;
        wheelFlCollider.steerAngle = steerAngle; 
        wheelFrCollider.steerAngle = steerAngle;  
    }

    private void HandleMotor()
    {
        wheelFlCollider.motorTorque = verticalInput * motorForce;
        wheelFrCollider.motorTorque = verticalInput * motorForce; 
    }

        private void UpdateWheels()
    {
        UpdateWheelPos(wheelFlCollider, wheelFl);
        UpdateWheelPos(wheelFrCollider, wheelFr);
        UpdateWheelPos(wheelRlCollider, wheelRl);
        UpdateWheelPos(wheelRrCollider, wheelRr);
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }
}
