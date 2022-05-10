using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectNetworking : TowerTargetingNetworking
{
    [SerializeField] private float _aoeRadius = 5.0f;
    [SerializeField] private float _aoeCooldown = 1.0f;
    [SerializeField] private int _aoeDamage = 15;
    private const int AOE_HEAL = 5;

    void Start()
    {
        _targetingRadius = _aoeRadius;
        _cooldownTime = _aoeCooldown;
    }

    void Update()
    {
        if (!isServer)
        {
            return;
        }

        SearchForTargets();
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
                enemyHP.alterHealth(-_aoeDamage);
                Debug.Log(string.Format("Applying {0} damage to [{1}] Total Health: {2}",
                    _aoeDamage, obj.name, enemyHP.HealthValue)
                );
            }
        }
    }
}
