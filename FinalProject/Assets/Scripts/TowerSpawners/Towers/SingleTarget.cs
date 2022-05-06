using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SingleTarget : TowerTargeting
{

    [SerializeField] private float _stRadius = 7.0f;
    [SerializeField] private float _stCooldown = 0.5f;
    [SerializeField] private int _stDamage = 25;

    [SerializeField] private GameObject shootPosition;

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

        RpcShootVFX(enemyLocation.position, _turretBarrel.transform.rotation);
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
                Enemy enemy = c.GetComponent<Enemy>();

                if(enemy == null || enemy.HasDied)
                {
                    continue;
                }

                GameObject enemyGO = enemy.gameObject;
                Vector3 enemyPos = enemyGO.transform.position;

                float distance = Vector3.Distance(transform.position, enemyPos);

                if (distance < minDistance) // enemy is closer than current closest 
                {
                    minDistance = distance;
                    nearest = enemyGO;
                }
            }
        }
        // apply damage if enemy exists 
        if (nearest)
        {
            Enemy enemy = nearest.GetComponent<Enemy>();
            SnapToEnemy(enemy.targetablePosition.transform);
            

            enemy.Damage(this.Owner, this._stDamage);
            //  Debug.Log(string.Format("Applying {0} damage to [{1}] Total Health: {2}",
            //    _stDamage, nearest.name, enemy.Health.HealthValue)
            //);
        }
    }

    [ClientRpc]
    void RpcShootVFX(Vector3 enemyPos, Quaternion towerRotation)
    {
        if(_turretBarrel.transform.rotation != towerRotation)
        {
            _turretBarrel.transform.rotation = towerRotation;
        }

        towerAudio.PlayShootAudio();

        TrailRenderer trail = Instantiate(towerFx.bulletTrail, shootPosition.transform.position, Quaternion.identity);

        StartCoroutine(SpawnTrail(trail, enemyPos));

        
    }



    IEnumerator SpawnTrail(TrailRenderer trail, Vector3 enemyPos)
    {
        float time = 0;

        Vector3 startPosition = trail.transform.position;
        trail.gameObject.transform.LookAt(enemyPos);

        while(time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, enemyPos, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = enemyPos;

        Instantiate(towerFx.bulletImpact, enemyPos, Quaternion.identity);

        Destroy(trail.gameObject, trail.time);
    }
}
