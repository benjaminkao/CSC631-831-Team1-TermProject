using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class TowerTargeting: MonoBehaviour, ITargetable
{
    protected float _targetingRadius;
    protected float _cooldownTime;
    private bool _canShoot = true;


    public GameObject TargetPosition
    {
        get { return this._targetPosition; }
    }

    [Tooltip("The position that enemies will use to reference how far away they are from this tower.")]
    [SerializeField] private GameObject _targetPosition;


    [SerializeField] private Health health;



    public ContainmentPlayer Owner
    {
        get
        {
            return this._owner;
        }

        set
        {
            this._owner = value;
        }
    }


    [SerializeField] private ContainmentPlayer _owner;

    // TowerTargeting sub-classes will implement the reaction to collisions
    // for players, enemies, or both.
    public abstract void ApplyEffects(Collider[] collisions);


    private void OnEnable()
    {
        this.RegisterTargetable();
    }

    private void OnDisable()
    {
        this.DeregisterTargetable();
    }



    // Update function for TowerTargeting sub-classes
    public void SearchForTargets()
    {
        if (_canShoot)
        {
            ApplyEffects(GetEntitiesInRange());
            _canShoot = false;
            StartCoroutine(ShotCooldown());
        }
    }

    private Collider[] GetEntitiesInRange()
    {
        // possible optimization: add all entities that can be affected by towers
        // into a separate layer, that way Physics.OverlapSphere can ignore irrelevant
        // collisions. 
        return Physics.OverlapSphere(transform.position, _targetingRadius /* , layerMask*/);
    }

    private IEnumerator ShotCooldown()
    {
        yield return new WaitForSecondsRealtime(_cooldownTime);
        _canShoot = true;
    }

    public void Damage(float damage)
    {
        health.alterHealth(-damage);
        

        if(health.Died)
        {
            Destroy(this);
        }
    }

    public GameObject GetTargetPosition()
    {
        return this._targetPosition;
    }
}