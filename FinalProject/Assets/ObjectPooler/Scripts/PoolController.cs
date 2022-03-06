using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolController : ScriptableObject
{
    private Queue<Poolable> pool = new Queue<Poolable>();

    private Poolable _prefab;

    public Poolable Prefab
    {
        get
        {
            return this._prefab;
        }
        set
        {
            if (this._prefab != null)
            {
                Debug.LogError("Error: Cannot change prefab of Pool.");
                return;
            }

            this._prefab = value;
        }
    }

    public void InitializePool(int numToPool)
    {
        if (this._prefab == null)
        {
            Debug.LogError("Error: No Poolable Prefab set. Could not initialize Pool with empty Prefab.");
            return;
        }

        for (int i = 0; i < numToPool; i++)
        {
            CreatePooledObject();
        }
    }

    public void InitializePool(int numToPool, Poolable prefab)
    {
        this._prefab = prefab;

        InitializePool(numToPool);
    }

    public Poolable GetObject()
    {
        Poolable poolableObject = GetPooledObject();

        if (poolableObject == null)
        {
            poolableObject = CreatePooledObject();
        }

        return poolableObject;
    }

    public Poolable GetObject(Vector3 position, Quaternion rotation)
    {
        Poolable poolableObject = GetObject();

        poolableObject.gameObject.transform.position = position;
        poolableObject.gameObject.transform.rotation = rotation;

        return poolableObject;
    }

    Poolable GetPooledObject()
    {
        return pool.Count > 0 ? pool.Dequeue() : null;
    }

    Poolable CreatePooledObject()
    {
        if (this._prefab == null)
        {
            Debug.LogError("Error: No Poolable Prefab set. Could not create new pooled object with empty Prefab.");
            return null;
        }

        // Create copy of Poolable prefab and set it to inactive
        Poolable pooledObject = Instantiate(this._prefab.gameObject).GetComponent<Poolable>();
        pooledObject.PoolIndex = this._prefab.PoolIndex;
        pooledObject.gameObject.SetActive(false);

        // Add the new copy of Poolable prefab to the pool
        pool.Enqueue(pooledObject);

        return pooledObject;
    }


    public void ReturnPooledObject(Poolable poolableObject)
    {
        poolableObject.Despawn();

        // Add poolableObject back to the pool
        pool.Enqueue(poolableObject);
    }

    public bool ComparePoolablePrefab(Poolable poolableObject)
    {
        if(_prefab == null)
        {
            Debug.LogError("Pool Controller does not have a Poolable prefab.");
            return false;
        }

        return _prefab.Equals(poolableObject);
    }

}
