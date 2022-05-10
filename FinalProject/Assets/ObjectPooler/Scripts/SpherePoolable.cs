using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePoolable : Poolable
{

    protected override void OnDisable()
    {
        Debug.Log("Disabled");
    }
}
