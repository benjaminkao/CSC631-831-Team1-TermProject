using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoolManager : MonoBehaviour
{
    public SpawnPoolManager Instance { get; private set; }


    private List<PoolController> poolControllers = new List<PoolController>();

    private int poolCount = 0;


    private void Awake()
    {
        /// Note: If you want the SpawnPoolManager to transcend Scenes, you must add the appropriate DontDestroyOnLoad code and refactor this
        ///  accordingly to keep the Singleton pattern.

        if (Instance != null)
        {
            return;
        }

        Instance = this;
        poolCount = 0;
    }


    private void OnEnable()
    {
        Poolable.OnPoolableDespawn += ReturnToPool;
    }

    private void OnDisable()
    {
        Poolable.OnPoolableDespawn -= ReturnToPool;
    }


    public void AddPool(PoolItem poolItem)
    {
        // Check if a Pool already exists for the given poolableObject
        if (HasPool(poolItem.Prefab))
        {
            Debug.LogError("A pool of this PoolableObject has already been created.");
        }

        // If a Pool doesn't already exist, need to add one to the dictionary
        poolItem.Prefab.PoolIndex = poolCount;
        poolCount++;

        // Create a new Pool Controller for the Pool
        PoolController poolController = ScriptableObject.CreateInstance<PoolController>();

        poolController.InitializePool(poolItem.NumToPool, poolItem.Prefab);


        poolControllers.Add(poolController);
    }

    public bool RemovePool(Poolable poolableObject)
    {
        if(HasPool(poolableObject)) {
            poolControllers.RemoveAt(poolableObject.PoolIndex);
            return true;
        }

        return false;
    }

    public Poolable Get(Poolable poolableObject)
    {
        if(!HasPool(poolableObject))
        {
            return null;
        }

        PoolController poolController = poolControllers[poolableObject.PoolIndex];

        return poolController.GetObject();

    }

    public Poolable Get(Poolable poolableObject, Vector3 position, Quaternion rotation)
    {
        if(!HasPool(poolableObject))
        {
            return null;
        }

        PoolController poolController = poolControllers[poolableObject.PoolIndex];

        return poolController.GetObject(position, rotation);
    }

    public void ReturnToPool(Poolable poolableObject)
    {
        if(!HasPool(poolableObject))
        {
            Debug.LogError("Error: No pool found of this Poolable object.");
            return;
        }

        Debug.Log("Return to pool");

        PoolController poolController = poolControllers[poolableObject.PoolIndex];

        poolController.ReturnPooledObject(poolableObject);

    }

    private bool HasPool(Poolable poolableObject)
    {
        if (poolControllers.Count <= 0)
        {
            return false;
        }

        if(poolControllers.Count <= poolableObject.PoolIndex)
        {
            return false;
        }

        // Get the PoolController in charge of the pool that poolableObject corresponds to
        PoolController controller = poolControllers[poolableObject.PoolIndex];

        return controller.ComparePoolablePrefab(poolableObject);
    }
}
