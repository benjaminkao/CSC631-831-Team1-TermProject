using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour, ITargetable
{
    public static event Action<Objective> OnObjectiveEnabled;
    public static event Action<Objective> OnObjectiveDisabled;
    public static event Action<Objective> OnObjectiveDestroyed;


    public bool Destroyed
    {
        get { return this._isDestroyed; }
    }



    [SerializeField] Health health;

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


    public void Damage(float damage)
    {
        health.alterHealth(-damage);

        if(health.Died)
        {
            this._isDestroyed = true;
            OnObjectiveDestroyed?.Invoke(this);
        }
    }

    public GameObject GetTargetPosition()
    {
        return this.gameObject;
    }

}
