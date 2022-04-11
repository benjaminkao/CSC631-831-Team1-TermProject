using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBeacon : Objective
{
    [SerializeField] GameObject beaconSphere;
    Renderer beaconRenderer;

    [Range(0, 1)] [SerializeField] private float _startPower;


    [Header("IsoSphere Rotation")]
    [SerializeField] private float rotationSpeed;



    private void Start()
    {



        if(this.beaconSphere == null)
        {
            Debug.LogWarning("Shield Beacon Sphere not set. Shield Beacon will not change how it looks as health goes down.");
        } else
        {
            
            this.beaconRenderer = beaconSphere.GetComponent<Renderer>();

            this.beaconRenderer.material.SetFloat("power", Mathf.Lerp(0f, 5f, 1 - _startPower));
        }

        

        
    }


    private void Update()
    {
        beaconSphere.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

    }



    public override void Damage(float damage)
    {
        base.Damage(damage);


        float powerPercentage = 1f - (this.health.HealthValue / this.health.MaxHealthValue);

        //Debug.Log(powerPercentage);

        this.beaconRenderer.material.SetFloat("power", Mathf.Lerp(0f, 5f, powerPercentage));
    }

}
