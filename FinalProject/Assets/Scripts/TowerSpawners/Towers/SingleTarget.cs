using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTarget : TowerTargeting
{
    [SerializeField] private float _stRadius = 7.0f;
    [SerializeField] private float _stCooldown = 0.5f;
    [SerializeField] private int _stDamage = 25;

    void Start()
    {
        _targetingRadius = _stRadius;
        _cooldownTime = _stCooldown;
    }

    void Update()
    {

        SearchForTargets();
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
        if (nearest)
        {
            Enemy enemy = nearest.GetComponent<Enemy>();

            enemy.Damage(this.Owner, this._stDamage);
            
            Debug.Log(string.Format("Applying {0} damage to [{1}] Total Health: {2}",
                _stDamage, nearest.name, enemy.Health.HealthValue)
            );
        }
    }
}
