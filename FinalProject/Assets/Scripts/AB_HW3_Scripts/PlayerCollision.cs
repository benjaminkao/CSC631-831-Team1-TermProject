using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    public GameObject bloodSpray;  
    public GameObject blood; 
    public GameObject bloodSpawnPosition; 

    void OnCollisionEnter (Collision col)
    {
        if(col.collider.name == "Vehicle"){
            Instantiate(bloodSpray, bloodSpawnPosition.transform.position + (Vector3.down * 1.1f), Quaternion.identity);
            Instantiate(blood, bloodSpawnPosition.transform.position + (Vector3.down * 1.1f), Quaternion.identity); 
            Destroy(gameObject);
        }
        
    }

}
