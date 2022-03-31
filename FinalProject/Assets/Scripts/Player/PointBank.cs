using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointBank : MonoBehaviour
{
    [SerializeField] private int _startingPoints;
    private int _totalPoints;

    [SerializeField] private ContainmentPlayer _player;

    [SerializeField] private TextMeshProUGUI label;

    public int TotalPoints { get { return _totalPoints; } }

    void Start()
    {
        if (_player == null)
        {
            Debug.LogError("Please instantiate the ContainmentPlayer object in PlayerPoints.");
        }

        label = GameObject.Find("PlayerPointsLabel").GetComponent<TextMeshProUGUI>();


        


        _totalPoints = _startingPoints;

        Reset();
    }

    public bool HasSufficientPoints(int pointAmount)
    {
        return _totalPoints >= pointAmount;
    }

    public void SpendPoints(int amount)
    {
        if (HasSufficientPoints(amount))
        {
            _totalPoints -= amount;
        }

        UpdateUI();
    }

    public void AddPoints(int amount)
    {
        _totalPoints += amount;

        UpdateUI();
    }


    void OnEnable()
    {
        Enemy.OnEnemyDied += AddPoints;
    }

    void OnDisable()
    {
        Enemy.OnEnemyDied -= AddPoints;
    }


    public void Reset()
    {
        UpdateUI();
    }


    public void AddPoints(ContainmentPlayer player, int points)
    {

        if (this._player != player)
        {
            return;
        }

        this._totalPoints += points;

        UpdateUI();
    }

    public void SubtractPoints(ContainmentPlayer player, int points)
    {
        if (this._player != player)
        {
            return;
        }

        this._totalPoints += points;

        UpdateUI();
    }



    private void UpdateUI()
    {
        label.text = $"{this._totalPoints}";
    }
}
