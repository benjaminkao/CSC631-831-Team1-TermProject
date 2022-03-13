using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        _healthBarUI.gameObject.SetActive(true);
    }


}
