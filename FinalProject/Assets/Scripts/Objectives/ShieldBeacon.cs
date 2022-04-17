using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBeacon : Objective
{

    public static event Action OnShieldBeaconDamaged;



    [SerializeField] GameObject beaconSphere;
    Renderer beaconRenderer;

    [Range(0, 1)] [SerializeField] private float _startPower;


    [Header("IsoSphere Rotation")]
    [SerializeField] private float rotationSpeed;


    [SerializeField] private float notifyDamageCooldown;
    private float _timeLastHit;


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


        this._timeLastHit = 0;
        
    }


    private void Update()
    {
        beaconSphere.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

    }



    public override void Damage(float damage)
    {
        base.Damage(damage);

        float currentTime = Time.time;

        if(currentTime - this._timeLastHit > notifyDamageCooldown)
        {
            // Shield Beacon has been damaged and it has been enough time to notify players
            OnShieldBeaconDamaged?.Invoke();
        }

        this._timeLastHit = currentTime;


        float powerPercentage = 1f - (this.health.HealthValue / this.health.MaxHealthValue);

        //Debug.Log(powerPercentage);

        this.beaconRenderer.material.SetFloat("power", Mathf.Lerp(0f, 5f, powerPercentage));
    }

}
