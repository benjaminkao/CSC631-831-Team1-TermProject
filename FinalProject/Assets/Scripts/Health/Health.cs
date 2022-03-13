using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected HealthBar _healthBarUI;

    private float _healthValue;

    public float HealthValue { get { return _healthValue; } }

    public bool Died
    {
        get { return atZeroHealth();  }
    }

    protected void Start()
    {
        _healthValue = _maxHealth;

        if(_healthBarUI == null)
        {
            Debug.LogError("Please attach the HealthBar component.");
        }

        _healthBarUI.SetMaxHealth(_maxHealth);
        _healthBarUI.SetHealth(_maxHealth);
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
        _healthBarUI.SetHealth(_healthValue);
    }

    protected virtual bool atMaxHealth() { return _healthValue >= _maxHealth; }
    protected virtual bool atZeroHealth() { return _healthValue <= 0; }
}
