using UnityEngine;
using UnityEngine.AI;
using Mirror;

[RequireComponent(typeof(NavMeshAgent), typeof(Health), typeof(EnemyPoolable))]
public class Enemy : NetworkBehaviour
{

    public Health Health {
        get { return health; }
       }


    [SerializeField] private EnemyType enemyType;
    [SerializeField] private GameObject bloodSpawnPosition;

    [SerializeField] private GameObject player;

    private NavMeshAgent agent;
    private Health health;

    private EnemyPoolable enemyPoolable;

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

    }

    private void OnEnable()
    {
        if(GameManager.Instance.Players.Count <= 0)
        {
            return;
        } 

        player = GameManager.Instance.Players[0].gameObject;
    }




    private void Update()
    {
        if(player == null)
        {
            return;
        }

        agent.SetDestination(player.transform.position);
    }


    public void Damage(float damage)
    {
        if(!isServer)
        {
            return;
        }


        Debug.Log("Enemy hit");

        health.alterHealth(-damage);

        if(health.Died)
        {
            enemyPoolable.Despawn();
        }
    }

    [ClientRpc]
    public void RpcUpdateHealth(float healthValue)
    {
        if(isServer)
        {
            return;
        }

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

}
