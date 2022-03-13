using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class Poolable : MonoBehaviour, IEquatable<Poolable>
{
    private int poolIndex;


    public static event Action<Poolable> OnPoolableDespawn;


    public int PoolIndex
    {
        get
        {
            return this.poolIndex;
        }

        set
        {
            this.poolIndex = value;
        }
    }




    public virtual GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public virtual void Spawn()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void Despawn()
    {
        this.gameObject.SetActive(false);
        OnPoolableDespawn?.Invoke(this);
    }


    // This abstract method must be implemented by all child classes of Poolable.
    // It defines the behavior of what the user wants to happen to the GameObject when it is "destroyed"/inactive
    protected abstract void OnDisable();




    // Override Equals() and GetHashCode() for Dictionary hashing
    public override bool Equals(object obj)
    {
        return Equals(obj as Poolable);
    }

    public bool Equals(Poolable other)
    {
        //Debug.Log(other != null);
        //Debug.Log(tag == other.tag);
        //Debug.Log(poolIndex == other.poolIndex);

        return other != null &&
               //base.Equals(other) &&
               tag == other.tag &&
               poolIndex == other.poolIndex;
    }

    public override int GetHashCode()
    {
        int hashCode = -287426838;
        //hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(tag);
        hashCode = hashCode * -1521134295 + poolIndex.GetHashCode();

        return hashCode;
    }
}
