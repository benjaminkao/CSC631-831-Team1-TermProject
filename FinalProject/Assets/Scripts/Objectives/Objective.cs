using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Objective : NetworkBehaviour, ITargetable
{
    public static event Action<Objective> OnObjectiveEnabled;
    public static event Action<Objective> OnObjectiveDisabled;
    public static event Action<Objective> OnObjectiveDestroyed;


    public bool Destroyed
    {
        get { return this._isDestroyed; }
    }



    [SerializeField] protected Health health;

    private bool _isDestroyed;
    


    void OnEnable()
    {
        this._isDestroyed = false;

        this.RegisterTargetable();
        OnObjectiveEnabled?.Invoke(this);
    }

    void OnDisable()
    {
        this.DeregisterTargetable();
        OnObjectiveDisabled?.Invoke(this);
    }


    public virtual void Damage(float damage)
    {
        health.alterHealth(-damage);

        RpcUpdateHealth(health.HealthValue);


        if(health.Died)
        {
            this._isDestroyed = true;
            OnObjectiveDestroyed?.Invoke(this);
        }
    }


    [ClientRpc]
    void RpcUpdateHealth(float healthValue)
    {
        this.health.SetHealth(healthValue);
    }



    public GameObject GetTargetPosition()
    {
        return this.gameObject;
    }

}
