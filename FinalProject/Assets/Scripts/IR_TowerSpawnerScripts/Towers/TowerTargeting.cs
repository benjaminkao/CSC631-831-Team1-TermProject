using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class TowerTargeting: NetworkBehaviour
{
    protected float _targetingRadius;
    protected float _cooldownTime;
    private bool _canShoot = true;

    // TowerTargeting sub-classes will implement the reaction to collisions
    // for players, enemies, or both.
    public abstract void ApplyEffects(Collider[] collisions);

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
}