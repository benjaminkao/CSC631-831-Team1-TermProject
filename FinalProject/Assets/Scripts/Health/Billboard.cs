using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public Transform cam;

    private void OnEnable()
    {
        cam = Camera.main.transform;
    }


    private void LateUpdate()
    {
        if(cam == null)
        {
            return;
        }
        transform.LookAt(transform.position + cam.forward);
    }
}
