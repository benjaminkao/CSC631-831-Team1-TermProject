using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
public class Enemy : Poolable
{

    [SerializeField] private EnemyType enemyType;
    [SerializeField] private GameObject bloodSpawnPosition;

    [SerializeField] private GameObject player;

    private NavMeshAgent agent;
    private Health health;

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
        health.alterHealth(-damage);

        if(health.Died)
        {
            base.Despawn();
        }
    }


    public void Died()
    {
        Instantiate(enemyType.bloodSprayPrefab, bloodSpawnPosition.transform.position, Quaternion.identity);
        Instantiate(enemyType.bloodPrefab, bloodSpawnPosition.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected override void OnDisable()
    {
        
    }
}
