using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTarget : TowerTargeting
{
    [SerializeField] private float _stRadius = 7.0f;
    [SerializeField] private float _stCooldown = 0.5f;
    [SerializeField] private int _stDamage = 25;

    private GameObject _turretBarrel;

    void Start()
    {
        _targetingRadius = _stRadius;
        _cooldownTime = _stCooldown;

        // moving section of the tower will be the first child of the root game object
        Transform turretBarrelTransform = transform.GetChild(0);
        _turretBarrel = turretBarrelTransform.gameObject;
    }

    void Update()
    {
        SearchForTargets();
    }

    // Handles the rotation of the tower to aim at the targeted enemy
    private void SnapToEnemy(Transform enemyLocation)
    {
        float barrelTurnRateDegrees = 180f;

        // Turret look movement: rotate on all axes to point directly at enemy
        Vector3 barrelAimDirection = enemyLocation.position - _turretBarrel.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(barrelAimDirection);

        targetRotation.x = Mathf.Clamp(targetRotation.x, -0.10f, 0.20f);

        _turretBarrel.transform.rotation = Quaternion.RotateTowards(
            _turretBarrel.transform.rotation,
            targetRotation,
            barrelTurnRateDegrees
        );
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
            SnapToEnemy(nearest.transform);
            Enemy enemy = nearest.GetComponent<Enemy>();

            enemy.Damage(this.Owner, this._stDamage);
            //  Debug.Log(string.Format("Applying {0} damage to [{1}] Total Health: {2}",
            //    _stDamage, nearest.name, enemy.Health.HealthValue)
            //);
        }
    }
}
