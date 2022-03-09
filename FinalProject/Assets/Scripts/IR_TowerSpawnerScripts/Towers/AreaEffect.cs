using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffect : TowerTargeting
{
    private const float AOE_RADIUS = 5.0f;
    private const float AOE_COOLDOWN = 1.0f;

    void Start()
    {
        _targetingRadius = AOE_RADIUS;
        _cooldownTime = AOE_COOLDOWN;
    }

    void Update()
    {
        SearchForTargets(AOE_COOLDOWN);
    }

    // Area-Effect Tower behavior: Apply appropriate effects to all Players/Enemies
    // in range
    public override void ApplyEffects(Collider[] collisions)
    {
        foreach (Collider c in collisions) 
        {
            GameObject obj = c.gameObject;

            if (obj.CompareTag("Player"))
            {
                Debug.Log("Applying Player effect to " + obj.name);
            }
            else if (obj.CompareTag("Enemy"))
            {
                Debug.Log("Applying Enemy effect to " + obj.name);
            }
        }
    }
}
