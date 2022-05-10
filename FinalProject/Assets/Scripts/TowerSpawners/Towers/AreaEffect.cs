using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AreaEffect : TowerTargeting
{
    
    [SerializeField] private GameObject shootPosition;

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
                Enemy enemy = obj.GetComponent<Enemy>();

                if(enemy == null || enemy.HasDied)
                {
                    continue;
                }

                RpcShootVFX(enemy.targetablePosition.transform.position);

                Health enemyHP = obj.GetComponent<Health>();
                enemyHP.alterHealth(-_aoeDamage);
                Debug.Log(string.Format("Applying {0} damage to [{1}] Total Health: {2}",
                    _aoeDamage, obj.name, enemyHP.HealthValue)
                );
            }
        }
    }


    [ClientRpc]
    void RpcShootVFX(Vector3 enemyPos)
    {
        towerAudio.PlayShootAudio();

        TrailRenderer trail = Instantiate(towerFx.bulletTrail, shootPosition.transform.position, Quaternion.identity);

        StartCoroutine(SpawnTrail(trail, enemyPos));
        

    }

    IEnumerator SpawnTrail(TrailRenderer trail, Vector3 enemyPos)
    {
        float time = 0;

        Vector3 startPosition = trail.transform.position;
        trail.gameObject.transform.LookAt(enemyPos);

        while (time < 1)
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
