using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{


    public int Points
    {
        get { return this._points; }
    }


    private int _points;

    [SerializeField] private ContainmentPlayer _player;



    // Start is called before the first frame update
    void Start()
    {
        if(_player == null)
        {
            Debug.LogError("Please instantiate the ContainmentPlayer object in PlayerPoints.");
        }
        Reset();
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
        _points = 0;
    }


    public void AddPoints(ContainmentPlayer player, int points)
    {

        if (this._player != player)
        {
            return;
        }

        this._points += points;
    }

    public void SubtractPoints(ContainmentPlayer player, int points)
    {
        if (this._player != player)
        {
            return;
        }

        this._points += points;
    }






}
