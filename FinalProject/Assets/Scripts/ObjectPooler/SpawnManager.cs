using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private SpawnPoolManager spawnPoolManager;

    [SerializeField]
    private List<PoolItem> poolItems;

    // Start is called before the first frame update
    void Start()
    {
        foreach(PoolItem poolItem in poolItems)
        {
            spawnPoolManager.AddPool(poolItem);
        }
    }


    public Poolable Get(Poolable prefab, Vector3 worldPosition)
    {

        Poolable objectToSpawn = null;

        foreach(PoolItem poolItem in poolItems)
        {
            if(poolItem.Prefab.Equals(prefab))
            {
                objectToSpawn = poolItem.Prefab;
            }
        }

        if(objectToSpawn == null)
        {
            // SpawnManager does not have the specified GameObject to spawn
            return null;
        }


        Poolable spawnedObject = spawnPoolManager.Get(objectToSpawn);

        spawnedObject.gameObject.transform.position = worldPosition;


        return spawnedObject;
    }
    

}
