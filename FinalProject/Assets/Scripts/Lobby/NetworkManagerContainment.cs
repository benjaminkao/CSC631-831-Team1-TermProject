using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// This code is from Dapper Dino's Main Menu Lobby Tutorial: https://www.youtube.com/watch?v=Fx8efi2MNz0
/// </summary>


public class NetworkManagerContainment : NetworkManager
{
    [SerializeField] private int minPlayers = 1;

    [Scene] [SerializeField] private string menuScene = string.Empty;
    [Scene] [SerializeField] private string gameScene = string.Empty;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerContainment roomPlayerPrefab = null;

    [Header("Game")]
    [SerializeField] private NetworkGamePlayerContainment gamePlayerPrefab = null;
    [SerializeField] private GameObject playerSpawnSystem = null;



    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;

    public List<NetworkRoomPlayerContainment> RoomPlayers { get; } = new List<NetworkRoomPlayerContainment>();
    public List<NetworkGamePlayerContainment> GamePlayers { get; } = new List<NetworkGamePlayerContainment>();




    public override void OnStartServer()
    {
        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    }


    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }


    public override void OnClientConnect()
    {
        base.OnClientConnect();

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        // Don't let player connect if max number of players in game
        if(numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        // Don't let player connect if game has already started
        if(SceneManager.GetActiveScene().path != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if(conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerContainment>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn); 
    }


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if(SceneManager.GetActiveScene().path == menuScene)
        {
            bool isLeader = RoomPlayers.Count == 0;

            NetworkRoomPlayerContainment roomPlayerInstance = Instantiate(roomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader;


            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);

            NotifyPlayersOfReadyState();
        }
    }

    public override void OnStopServer()
    {
        RoomPlayers.Clear();
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }


    public void NotifyPlayersOfReadyState()
    {
        foreach(var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if(numPlayers < minPlayers)
        {
            return false;
        }

        foreach(var player in RoomPlayers)
        {
            if(!player.IsReady)
            {
                return false;
            }
        }

        return true;
    }


    public void StartGame()
    {
        Debug.Log(gameScene);
        if(SceneManager.GetActiveScene().path == menuScene)
        {
            if(!IsReadyToStart())
            {
                return;
            }

            ServerChangeScene(gameScene);
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        // From Menu to Game
        if(SceneManager.GetActiveScene().path == menuScene && newSceneName == gameScene)
        {

            for(int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;

                var gamePlayerInstance = Instantiate(gamePlayerPrefab);
                gamePlayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                // Destroy NetworkRoomPlayerContainment's gameobject that is attached to the NetworkIdentity
                NetworkServer.Destroy(conn.identity.gameObject);

                // Assign the new NetworkGamePlayerContainment's gameobject to the isolated NetworkIdentity that is the player's connection to the server
                NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject);
            }

        }



        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if(sceneName == gameScene)
        {
            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);
        }
    }

}