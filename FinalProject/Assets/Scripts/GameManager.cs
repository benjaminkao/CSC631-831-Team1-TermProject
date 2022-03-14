using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager Instance;

    [SerializeField] private List<Player> _players;
    [SerializeField] private Player _localPlayer;
    [SerializeField] private SpawnManager _spawnManager;

    public Player LocalPlayer
    {
        get { return _localPlayer; }
    }

    public List<Player> Players
    {
        get { return _players; }
    }


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Move GameManager GameObject into DontDestroyOnLoad scene
        DontDestroyOnLoad(gameObject);

        this._players = new List<Player>();
    }

    private void OnEnable()
    {
        Player.OnPlayerJoin += RegisterPlayer;
        Player.OnPlayerLeave += RemovePlayer;
        Player.OnLocalPlayerJoin += RegisterLocalPlayer;
        Player.OnLocalPlayerLeave += RemoveLocalPlayer;
    }

    private void OnDisable()
    {
        Player.OnPlayerJoin -= RegisterPlayer;
        Player.OnPlayerLeave -= RemovePlayer;
        Player.OnLocalPlayerJoin -= RegisterLocalPlayer;
        Player.OnLocalPlayerLeave -= RemoveLocalPlayer;
    }




    // Register player with the game so Enemies can target the players
    public void RegisterPlayer(Player player)
    {
        this._players.Add(player);
    }

    public void RemovePlayer(Player player)
    {
        this._players.Remove(player);
    }


    public void RegisterLocalPlayer(Player player)
    {
        this._localPlayer = player;
    }

    public void RemoveLocalPlayer(Player player)
    {
        this._localPlayer = null;
    }

    public void RegisterSpawnManager(SpawnManager spawnManager)
    {
        this._spawnManager = spawnManager;
    }
    
}
