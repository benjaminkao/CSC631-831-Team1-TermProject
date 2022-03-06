using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolItem
{
    [SerializeField]
    private Poolable prefab;
    [SerializeField]
    private int numToPool;

    public Poolable Prefab
    {
        get
        {
            return this.prefab;
        }
    }

    public int NumToPool
    {
        get
        {
            return this.numToPool;
        }
    }


}
