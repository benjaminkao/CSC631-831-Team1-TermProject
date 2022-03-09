using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffect : TowerTargeting
{
    private const float AOE_RADIUS = 5.0f;
    private const float AOE_COOLDOWN = 1.0f;
    private const int AOE_DAMAGE = 15;
    private const int AOE_HEAL = 5;

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
                Health playerHP = obj.GetComponent<Health>();
                playerHP.alterHealth(AOE_HEAL);
                Debug.Log(string.Format("Adding {0} health to [{1}] Total health: {2}",
                    AOE_HEAL, obj.name, playerHP.HealthValue));
            }
            else if (obj.CompareTag("Enemy"))
            {
                Health enemyHP = obj.GetComponent<Health>();
                enemyHP.alterHealth(-AOE_DAMAGE);
                Debug.Log(string.Format("Applying {0} damage to [{1}] Total Health: {2}",
                    AOE_DAMAGE, obj.name, enemyHP.HealthValue)
                );
            }
        }
    }
}
