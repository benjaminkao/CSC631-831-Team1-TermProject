using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerTargeting: MonoBehaviour
{
    protected float _targetingRadius;
    protected float _cooldownTime;

    // TowerTargeting sub-classes will implement the reaction to collisions
    // for players, enemies, or both.
    public abstract void ApplyEffects(Collider[] collisions);

    // Update function for TowerTargeting sub-classes
    public void SearchForTargets(float coolDown)
    {
        if (_cooldownTime <= 0.0f)
        {
            ApplyEffects(GetEntitiesInRange());
            ResetCooldown(coolDown);
        }
        else
        {
            WaitForNextShot();
        }
    }

    public Collider[] GetEntitiesInRange()
    {
        // possible optimization: add all entities that can be affected by towers
        // into a separate layer, that way Physics.OverlapSphere can ignore irrelevant
        // collisions. 
        return Physics.OverlapSphere(transform.position, _targetingRadius /* , layerMask*/);
    }

    public void WaitForNextShot() { _cooldownTime -= Time.deltaTime; }
    public void ResetCooldown(float newTime) { _cooldownTime = newTime; }
}