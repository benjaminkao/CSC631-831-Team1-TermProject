using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour {

    public GameObject firePt;
    public List<GameObject> vfx = new List<GameObject> ();

    private GameObject effectToSpawn;   


    // Start is called before the first frame update
    void Start()
    {
        effectToSpawn = vfx[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            SpawnVFX();
        }
    }

    void SpawnVFX(){
        GameObject vfx;

        if(firePt != null){
            vfx = Instantiate (effectToSpawn, firePt.transform.position, Quaternion.identity);
        } else {
            Debug.Log("No fire points");
        }
    }
}
