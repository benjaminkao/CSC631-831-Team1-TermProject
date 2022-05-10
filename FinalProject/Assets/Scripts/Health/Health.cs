using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected HealthBar _healthBarUI;

    [SerializeField] protected float _healthValue;


    public HealthBar HealthBarUI
    {
        get { return this._healthBarUI; }
        set
        {
            this._healthBarUI = value;
            this._healthBarUI.SetMaxHealth(_maxHealth);
            this._healthBarUI.SetHealth(_maxHealth);
        }
    }

    public bool AtMaxHealth
    {
        get { return atMaxHealth();  }
    }

    public float MaxHealthValue
    {
        get { return _maxHealth; }
    }

    public float HealthValue { 
        get { return _healthValue; }

        set { 
            _healthValue = value;

            if (_healthBarUI != null)
            {
                _healthBarUI.SetHealth(value);
            }
        }
    }

    public bool Died
    {
        get { return atZeroHealth();  }
    }

    protected void Start()
    {
        _healthValue = _maxHealth;

        if (_healthBarUI != null)
        {

            _healthBarUI.SetMaxHealth(_maxHealth);
            _healthBarUI.SetHealth(_maxHealth);
        }
    }

    /* pass in a positive number to add health, or a negative to damage*/
    public virtual void alterHealth(float value)
    {
        _healthValue += value;
        if (atMaxHealth())
        {
            _healthValue = _maxHealth;
            
        }
        else if (atZeroHealth())
        {
            _healthValue = 0;
        }

        if (_healthBarUI != null)
        {
            _healthBarUI.SetHealth(_healthValue);
        }
    }

    public virtual void SetHealth(float healthValue)
    {
        HealthValue = healthValue;

        if(_healthBarUI != null)
        {
            _healthBarUI.SetHealth(HealthValue);
        }
    }

    protected virtual bool atMaxHealth() { return _healthValue >= _maxHealth; }
    protected virtual bool atZeroHealth() { return _healthValue <= 0; }
}
