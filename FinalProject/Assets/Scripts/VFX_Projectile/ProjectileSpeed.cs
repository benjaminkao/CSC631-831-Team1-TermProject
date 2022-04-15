using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpeed : MonoBehaviour
{
    public float speed;
    public GameObject muzzlePrefab;
    public GameObject impactPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (muzzlePrefab != null){
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward;
            var psMuzzle = muzzleVFX.GetComponent<ParticleSystem>();
            if (psMuzzle != null)
                Destroy(muzzleVFX, psMuzzle.main.duration);
         else {
            var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
            Destroy (muzzleVFX, psChild.main.duration);
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (speed != 0 ){
            transform.position += transform.forward * (speed * Time.deltaTime);
        } else {
            Debug.Log("No speed");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        speed = 0;

        if (impactPrefab != null){
            var impactVFX = Instantiate(impactPrefab, transform.position, Quaternion.identity);
            impactVFX.transform.forward = gameObject.transform.forward;
            var psImpact = impactVFX.GetComponent<ParticleSystem>();
            if (psImpact != null)
                Destroy(impactVFX, psImpact.main.duration);
         else {
            var psChild = impactVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
            Destroy (impactVFX, psChild.main.duration);
            } 
        }

        Destroy(gameObject);
    }
}
