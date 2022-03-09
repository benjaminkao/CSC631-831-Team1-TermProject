using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTarget : TowerTargeting
{
    private const float ST_RADIUS = 7.0f;
    private const float ST_COOLDOWN = 0.5f;

    void Start()
    {
        _targetingRadius = ST_RADIUS;
        _cooldownTime = ST_COOLDOWN;
    }

    void Update()
    {
        SearchForTargets(ST_COOLDOWN);
    }

    // Single-Target Tower behavior: Find closest enemy and apply damage to them
    public override void ApplyEffects(Collider[] collisions)
    {
        GameObject nearest = null;
        float minDistance = float.PositiveInfinity;

        foreach (Collider c in collisions)
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                GameObject enemy = c.gameObject;
                Vector3 enemyPos = enemy.transform.position;

                float distance = Vector3.Distance(transform.position, enemyPos);

                if (distance < minDistance) // enemy is closer than current closest 
                {
                    minDistance = distance;
                    nearest = enemy;
                }
            }
        }
        // apply damage if enemy exists 
        if (nearest) Debug.Log("Applying damage to " + nearest.name);
    }
}
