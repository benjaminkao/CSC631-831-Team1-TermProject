using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

[RequireComponent(typeof(NavMeshAgent), typeof(Health), typeof(EnemyPoolable))]
public class Enemy : NetworkBehaviour
{
    public EnemyAnimator enemyAnimator;

    public static event Action OnEnemyDied;
    public static event Action<ContainmentPlayer, int> OnEnemyDiedPoints;


    public Health Health {
        get { return health; }
       }


    [Header("Movement")]
    public float walkingSpeed;
    public bool shouldRunAtPlayer;
    [Tooltip("The distance the zombie should be before it starts running/chasing the player.")]
    public float distanceFromPlayerToRun;
    public float runningSpeed;

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
    [SerializeField] private GameObject particleBloodSpawnPosition;

    [SerializeField] private GameObject target;
    private ITargetable targetable;

    [SerializeField] private int _pointsForDeath = 100;

    private NavMeshAgent agent;
    private Health health;

    private EnemyPoolable enemyPoolable;

    private bool _shouldUpdateTarget;
    private bool _canAttack;
    private bool _canWalk;
    private bool _hasDied;
    private float _sqrAttackRange;
    private float _sqrRunRange;


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
        _sqrRunRange = distanceFromPlayerToRun * distanceFromPlayerToRun;
        _canAttack = true;
        _canWalk = true;
        _hasDied = false;
    }




    private void Update()
    {
        if(!isServer)
        {
            return;
        }

        if(_hasDied)
        {
            return;
        }

        if(_shouldUpdateTarget)
        {
            StartCoroutine(UpdateTarget());
        }


        if(target == null)
        {
            return;
        }

        float sqrDistanceFromTarget = (targetable.GetTargetPosition().gameObject.transform.position - transform.position).sqrMagnitude;


        if (!_hasDied)
        {
            if (sqrDistanceFromTarget <= _sqrAttackRange)
            {
                if (_canAttack)
                {
                    StartCoroutine(AttackTarget());
                }
            }
            else
            {
                if (shouldRunAtPlayer)
                {
                    if (sqrDistanceFromTarget <= _sqrRunRange)
                    {
                        agent.speed = runningSpeed;
                    }
                    else
                    {
                        agent.speed = walkingSpeed;
                    }
                }


                enemyAnimator.HandleMovementAnimation(agent.velocity.sqrMagnitude);
                if (_canWalk)
                {
                    agent.SetDestination(targetable.GetTargetPosition().gameObject.transform.position);
                }
            }
        }
    }


    public void Damage(ContainmentPlayer player, float damage)
    {
        if(!isServer)
        {
            return;
        }

        //Debug.Log("Enemy hit");

        


        health.alterHealth(-damage);

        this.RpcUpdateHealth(this.health.HealthValue);

        

        if (health.Died && !this._hasDied)
        {
            this._hasDied = true;
            OnEnemyDied?.Invoke();
            OnEnemyDiedPoints?.Invoke(player, this._pointsForDeath);
            RpcDied();
        }
    }


    [ClientRpc]
    public void RpcUpdateHealth(float healthValue)
    {
         Destroy(Instantiate(enemyType.bloodSprayPrefab, particleBloodSpawnPosition.transform.position, Quaternion.identity), 3);

        health.SetHealth(healthValue);
        //Debug.Log(healthValue);
        //if(!isServer && health.Died)
        //{
        //    //OnEnemyDied?.Invoke(player, this._pointsForDeath);
        //    enemyPoolable.Despawn();
        //}
    }



    public void Died()
    {
        Instantiate(enemyType.bloodSprayPrefab, bloodSpawnPosition.transform.position, Quaternion.identity);
        Instantiate(enemyType.bloodPrefab, bloodSpawnPosition.transform.position, Quaternion.identity);
        enemyPoolable.Despawn();
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
        targetable = target.GetComponent<ITargetable>();
        

        yield return new WaitForSeconds(TimeBetweenUpdatingTargets);

        _shouldUpdateTarget = true;
    }

    IEnumerator AttackTarget()
    {
        _canAttack = false;
        _canWalk = false;


        // Attack Target
        ITargetable targetable = target.GetComponent<ITargetable>();

        RpcAttack();
        

        targetable.Damage(AttackDamage);


        yield return new WaitForSeconds(TimeBetweenAttacks);

        _canAttack = true;
        _canWalk = true;
    }

    [ClientRpc]
    private void RpcAttack()
    {
        enemyAnimator.HandleAttackAnimation();
    }

    [ClientRpc]
    private void RpcDied()
    {
        enemyAnimator.HandleDeathAnimation();
    }
}
