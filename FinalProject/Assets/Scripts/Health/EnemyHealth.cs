using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        _healthBarUI.gameObject.SetActive(false);
    }


    public override void alterHealth(float value)
    {
        base.alterHealth(value);

        if(atMaxHealth())
        {
            _healthBarUI.gameObject.SetActive(false);
        } else if(!atZeroHealth() || !atMaxHealth())
        {
            _healthBarUI.gameObject.SetActive(true);
        }
    }

    public bool Died()
    {
        return atZeroHealth();
    }
    
}
