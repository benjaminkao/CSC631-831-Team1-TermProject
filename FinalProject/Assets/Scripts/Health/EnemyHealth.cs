using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{

    public float HealthValue
    {
        get { return _healthValue;  }

        set
        {
            _healthValue = value;
            _healthBarUI.SetHealth(value);
            Debug.Log("got here");
            HandleShowHealthBar();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        _healthBarUI.gameObject.SetActive(false);
    }


    public override void alterHealth(float value)
    {
        base.alterHealth(value);

        HandleShowHealthBar();
    }

    public override void SetHealth(float healthValue)
    {
        base.SetHealth(healthValue);

        HandleShowHealthBar();
    }

    private void HandleShowHealthBar()
    {
        
        if (atMaxHealth())
        {
            _healthBarUI.gameObject.SetActive(false);
            return;
        }

        //Debug.Log("Should show health bar");
        _healthBarUI.gameObject.SetActive(!Died());
        
    }

    


    public bool Died()
    {
        return atZeroHealth();
    }
    
}
