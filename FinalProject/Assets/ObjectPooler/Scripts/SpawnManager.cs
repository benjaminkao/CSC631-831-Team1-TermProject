using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private SpawnPoolManager spawnPoolManager;

    [SerializeField]
    private List<PoolItem> poolItems;


    [SerializeField]
    private int spawnRange;

    // Start is called before the first frame update
    void Start()
    {
        foreach(PoolItem poolItem in poolItems)
        {
            spawnPoolManager.AddPool(poolItem);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnRandomShape();
        } 
    }

    void SpawnRandomShape()
    {
        Poolable poolableObject = spawnPoolManager.Get(GetRandomShapePrefab());

        poolableObject.gameObject.transform.position = GetRandomPosition(spawnRange);
        poolableObject.gameObject.transform.rotation = Random.rotation;
        poolableObject.Spawn();

        StartCoroutine(DespawnTimer(poolableObject));
    }

    IEnumerator DespawnTimer(Poolable poolableObject)
    {
        yield return new WaitForSeconds(2f);

        spawnPoolManager.ReturnToPool(poolableObject);
    }



    Poolable GetRandomShapePrefab()
    {
        int shapeIndex = Random.Range(0, poolItems.Count);

        Poolable poolableObject = poolItems[shapeIndex].Prefab;

        return poolableObject;
    }

    Vector3 GetRandomPosition(float bounds)
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        Vector3 randomPos = new Vector3(x, y, z) * bounds;

        return randomPos;
    }
}
