using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkGamePlayerContainment : NetworkBehaviour
{

    [SyncVar]
    [SerializeField]
    private string displayName = "Loading...";

    public string DisplayName { get { return displayName;  } }

    //private bool isLeader;

    //public bool IsLeader
    //{
    //    set
    //    {
    //        isLeader = value;
    //    }
    //}

    private NetworkManagerContainment room;

    private NetworkManagerContainment Room
    {
        get
        {
            if (room != null)
            {
                return room;
            }
            return room = NetworkManager.singleton as NetworkManagerContainment;
        }
    }

    public override void OnStartServer()
    {
        DontDestroyOnLoad(gameObject);
        Room.GamePlayers.Add(this);
    }

    public override void OnStopServer()
    {
        Room.GamePlayers.Remove(this);
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room.GamePlayers.Add(this);
    }


    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }
}
