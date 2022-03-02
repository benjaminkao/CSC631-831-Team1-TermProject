using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    [SerializeField] private EnemyType enemyType;
    [SerializeField] private GameObject bloodSpawnPosition;

    void OnEnable()
    {
        if(enemyType == null)
        {
            Debug.LogError("Please attach the correct EnemyType ScriptableObject");
        }

        if(bloodSpawnPosition == null)
        {
            Debug.LogError("Please specify the spawn position of the blood effects");
        }
    }



    public void Died()
    {
        Instantiate(enemyType.bloodSprayPrefab, bloodSpawnPosition.transform.position, Quaternion.identity);
        Instantiate(enemyType.bloodPrefab, bloodSpawnPosition.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
