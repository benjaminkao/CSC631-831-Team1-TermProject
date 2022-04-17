using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class PointBank : NetworkBehaviour
{
    [SerializeField] private int _startingPoints;
    private int _totalPoints;

    [SerializeField] private ContainmentPlayer _player;

    [SerializeField] private TextMeshProUGUI label;

    public int TotalPoints { get { return _totalPoints; } }

    public override void OnStartClient()
    {
        if (_player == null)
        {
            Debug.LogError("Please instantiate the ContainmentPlayer object in PlayerPoints.");
        }


        if (isLocalPlayer)
        {
            label = GameObject.Find("PlayerPointsLabel").GetComponent<TextMeshProUGUI>();
        }


        


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

        RpcUpdatePlayerPoints(this._totalPoints);
    }

    public void AddPoints(int amount)
    {
        _totalPoints += amount;

        RpcUpdatePlayerPoints(this._totalPoints);
    }


    void OnEnable()
    {
        Enemy.OnEnemyDiedPoints += AddPoints;
    }

    void OnDisable()
    {
        Enemy.OnEnemyDiedPoints -= AddPoints;
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

        RpcUpdatePlayerPoints(this._totalPoints);

        UpdateUI();
    }

    public void SubtractPoints(ContainmentPlayer player, int points)
    {
        if (this._player != player)
        {
            return;
        }

        this._totalPoints += points;

        RpcUpdatePlayerPoints(this._totalPoints);

    }


    [ClientRpc]
    void RpcUpdatePlayerPoints(int points)
    {
        this._totalPoints = points;

        UpdateUI();
    }



    private void UpdateUI()
    {
        if(label == null)
        {
            return;
        }

        label.text = $"{this._totalPoints}";
    }
}
