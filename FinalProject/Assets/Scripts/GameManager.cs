using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager Instance;

    [SerializeField] private List<Player> _players;
    [SerializeField] private SpawnManager _spawnManager;

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

    // Register player with the game so Enemies can target the players
    public void RegisterPlayer(Player player)
    {
        this._players.Add(player);
    }

    public void RegisterSpawnManager(SpawnManager spawnManager)
    {
        this._spawnManager = spawnManager;
    }
    
}
