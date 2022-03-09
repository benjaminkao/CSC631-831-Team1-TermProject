using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    private int _healthValue;

    public int HealthValue { get { return _healthValue; } }

    private void Start()
    {
        _healthValue = _maxHealth;
    }

    /* pass in a positive number to add health, or a negative to damage*/
    public void alterHealth(int value)
    {
        _healthValue += value;
        if (atMaxHealth())
        {
            _healthValue = _maxHealth;
        }
        else if (atZeroHealth())
        {
            gameObject.SetActive(false);
        }
    }

    private bool atMaxHealth() { return _healthValue >= _maxHealth; }
    private bool atZeroHealth() { return _healthValue <= 0; }
}
