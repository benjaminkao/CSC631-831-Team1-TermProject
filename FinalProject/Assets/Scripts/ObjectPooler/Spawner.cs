using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Spawner : NetworkBehaviour
{

    private SpawnManager _spawnManager;


    [SerializeField]
    private int _spawnRange;

    [SerializeField]
    private SpawnerPreset _spawnerPreset;

    void Awake()
    {
        if(_spawnerPreset == null)
        {
            Debug.LogError("Spawner does not have a SpawnerPreset");
        }


        // Get the SpawnManager GameObject
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Is Server: " + isServer);
        // Anything that spawns should only be handled by the server
        if(!isServer)
        {
            return;
        }


        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SpawnRandomPoolableObject();
        }
    }


    void SpawnRandomPoolableObject()
    {
        Poolable poolableObject = _spawnManager.Get(GetRandomEnemy(), (this.transform.position + this.GetRandomPosition(_spawnRange)));

        poolableObject.Spawn();

        NetworkServer.Spawn(poolableObject.gameObject);

        //StartCoroutine(DespawnTimer(poolableObject));
    }

    //IEnumerator DespawnTimer(Poolable poolableObject)
    //{
    //    yield return new WaitForSeconds(2f);

    //    spawnPoolManager.ReturnToPool(poolableObject);
    //}

    Poolable GetRandomEnemy()
    {
        if(_spawnerPreset == null)
        {
            return null;
        }

        int n = _spawnerPreset.spawnablePrefabs.Count;

        int rand = Random.Range(0, n);

        return _spawnerPreset.spawnablePrefabs[rand];
    }


    Vector3 GetRandomPosition(float bounds)
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        Vector3 randomPos = new Vector3(x, 1, z) * bounds;

        return randomPos;
    }


}
