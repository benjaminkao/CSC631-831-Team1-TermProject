using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChange : MonoBehaviour
{
    public CinemachineVirtualCamera ThirdPersonCam; 
    public CinemachineVirtualCamera FirstPersonCam;
    public bool toggle;  

    void Update()
    {
        if(Input.GetButtonDown("CamToggle")) {
            toggle = !toggle; 
            if(toggle){
                //switch to third person camera
                ThirdPersonCam.Priority = 1; 
                FirstPersonCam.Priority = 0; 
            }
            else 
            {
                //Switch to First Person Camera 
                ThirdPersonCam.Priority = 0; 
                FirstPersonCam.Priority = 1; 
            }
        }
    }
}
