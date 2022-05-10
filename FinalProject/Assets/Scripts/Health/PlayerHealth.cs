using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHealth : Health
{
    public bool Died
    {
        get
        {
            return this._hasDied;
        }

        set
        {
            this._hasDied = value;
        }
    }

    public bool IsDown
    {
        get
        {
            return this._isDown;
        }

        set
        {
            this._isDown = value;
        }
    }

    [SerializeField] private bool _hasDied;

    [SerializeField] private bool _isDown;

    



    public ContainmentPlayer Player
    {
        get
        {
            return this._player;
        }
        set
        {
            this._player = value;
        }
    }

    private ContainmentPlayer _player;


        // Start is called before the first frame update
     void Start()
    {

        _hasDied = false;
        _isDown = false;

        base.Start();
    }


    public override void alterHealth(float value)
    {
        base.alterHealth(value);

        if(atZeroHealth() && !_isDown && !_hasDied)
        {
            // Player is downed
            _isDown = true;
            this.SetHealth(MaxHealthValue);
            return;
        }

        if(atZeroHealth() && _isDown)
        {
            _hasDied = true;
            _isDown = false;
        }

    }

    public override void SetHealth(float healthValue)
    {
        base.SetHealth(healthValue);


        if (atZeroHealth() && !_isDown)
        {
            // Player is downed
            _isDown = true;
            return;
        }

        if (atZeroHealth() && _isDown)
        {
            _hasDied = true;
            _isDown = false;
        }

    }



}
