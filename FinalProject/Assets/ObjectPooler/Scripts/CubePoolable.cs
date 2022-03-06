using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePoolable : Poolable
{
    protected override void OnDisable()
    {
        Debug.Log("Disabled");
    }

}
