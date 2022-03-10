using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonFollow : MonoBehaviour
{
    public GameObject player; 
    private Vector3 offset = new Vector3(0,4,-9);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate(){
        transform.position = player.transform.position + offset; 
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = player.transform.position + new Vector3(0, 4, -9);  
    }
}
