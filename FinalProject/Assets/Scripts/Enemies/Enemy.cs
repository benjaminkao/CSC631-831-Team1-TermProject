using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Health), typeof(EnemyPoolable))]
public class Enemy : MonoBehaviour
{


    public static event Action<ContainmentPlayer, int> OnEnemyDied;


    public Health Health {
        get { return health; }
       }

    [Header("Attack")]

    public float TimeBetweenUpdatingTargets = 1f;
    [Tooltip("The amount of damage the enemy does per attack.")]
    public float AttackDamage = 10f;
    [Tooltip("How far the enemy has to be from a target before it can attack.")]
    public float AttackRange = 1f;
    [Tooltip("The amount of time between attacks an enemy must wait before attacking again.")]
    public float TimeBetweenAttacks = 0.5f;


    [Header("Misc")]

    [SerializeField] private EnemyType enemyType;
    [SerializeField] private GameObject bloodSpawnPosition;

    [SerializeField] private GameObject target;

    [SerializeField] private int _pointsForDeath = 100;

    private NavMeshAgent agent;
    private Health health;

    private EnemyPoolable enemyPoolable;

    private bool _shouldUpdateTarget;
    private bool _canAttack;
    private float _sqrAttackRange;


    void Awake()
    {
        if(enemyType == null)
        {
            Debug.LogError("Please attach the correct EnemyType ScriptableObject");
        }

        if(bloodSpawnPosition == null)
        {
            Debug.LogError("Please specify the spawn position of the blood effects");
        }


        health = GetComponent<Health>();
        agent = GetComponent<NavMeshAgent>();
        enemyPoolable = GetComponent<EnemyPoolable>();
        _shouldUpdateTarget = true;

        _sqrAttackRange = AttackRange * AttackRange;
        _canAttack = true;
    }




    private void Update()
    {
        if(_shouldUpdateTarget)
        {
            StartCoroutine(UpdateTarget());
        }


        if(target == null)
        {
            return;
        }

        float sqrDistanceFromTarget = (target.transform.position - transform.position).sqrMagnitude;

        //Debug.Log(sqrDistanceFromTarget);
        //Debug.Log($"Attack Range: {_sqrAttackRange}");
        //Debug.Log(sqrDistanceFromTarget <= _sqrAttackRange);

        if (sqrDistanceFromTarget <= _sqrAttackRange)
        {
            if (_canAttack)
            {
                StartCoroutine(AttackTarget());
            }
        }
        else
        {


            agent.SetDestination(target.transform.position);
        }
    }


    public void Damage(ContainmentPlayer player, float damage)
    {


        Debug.Log("Enemy hit");

        health.alterHealth(-damage);

        if(health.Died)
        {
            OnEnemyDied?.Invoke(player, this._pointsForDeath);
            enemyPoolable.Despawn();
        }
    }



    public void RpcUpdateHealth(float healthValue)
    {

        health.SetHealth(healthValue);
        Debug.Log(healthValue);
        if(health.Died)
        {
            enemyPoolable.Despawn();
        }
    }



    public void Died()
    {
        Instantiate(enemyType.bloodSprayPrefab, bloodSpawnPosition.transform.position, Quaternion.identity);
        Instantiate(enemyType.bloodPrefab, bloodSpawnPosition.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private GameObject FindClosestTarget()
    {
        if(GameManager.Instance == null || GameManager.Instance.EnemyTargetables.Count <= 0)
        {
            return null;
        }

        List<GameObject> targets = GameManager.Instance.EnemyTargetables;

        if(targets.Count == 1)
        {
            return targets[0];
        }

        return targets.OrderBy(go => (this.gameObject.transform.position - go.gameObject.transform.position).sqrMagnitude).First();
    }


    IEnumerator UpdateTarget()
    {
        _shouldUpdateTarget = false;
        target = FindClosestTarget();

        yield return new WaitForSeconds(TimeBetweenUpdatingTargets);

        _shouldUpdateTarget = true;
    }

    IEnumerator AttackTarget()
    {
        _canAttack = false;


        // Attack Target
        ITargetable targetable = target.GetComponent<ITargetable>();

        targetable.Damage(AttackDamage);


        yield return new WaitForSeconds(TimeBetweenAttacks);

        _canAttack = true;
    }
}
