using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public Transform cam;

    private void Awake()
    {
        cam = Camera.main.gameObject.transform;
    }


    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
