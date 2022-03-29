using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBank : MonoBehaviour
{
    [SerializeField] private int _startingPoints;
    private int _totalPoints;

    public int TotalPoints { get { return _totalPoints; } }

    void Start()
    {
        _totalPoints = _startingPoints;
    }

    public bool HasSufficientPoints(int pointAmount)
    {
        return _totalPoints <= pointAmount;
    }

    public void SpendPoints(int amount)
    {
        if (HasSufficientPoints(amount))
        {
            _totalPoints -= amount;
        }
    }

    public void AddPoints(int amount)
    {
        _totalPoints += amount;
    }
}
