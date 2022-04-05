using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    GameObject gameObject { get; }

    void Damage(float damage);

    GameObject GetTargetPosition();

}

public static class TargetableManager
{
    public static Action<ITargetable> OnTargetableEnabled;
    public static Action<ITargetable> OnTargetableDisabled;

    // Extension Method:
    // Allows any/all classes implementing the ITargetable interface to have
    // access to this RegisterTargetable() class.
    public static void RegisterTargetable(this ITargetable targetable)
    {
        OnTargetableEnabled?.Invoke(targetable);
    }

    public static void DeregisterTargetable(this ITargetable targetable)
    {
        OnTargetableDisabled?.Invoke(targetable);
    }
}
