using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBoxColor : MonoBehaviour
{
    Material m_Material; 
    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Material from the Renderer of the GameObject
        m_Material = GetComponent<Renderer>().material;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            m_Material.color = Color.blue; 
        }
    }
}
