using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager Instance;

    [SerializeField] private List<ContainmentPlayer> _players;
    [SerializeField] private ContainmentPlayer _localPlayer;
    [SerializeField] private List<Objective> _objectives;

    [SerializeField] private WaveSpawner _waveSpawner;


    private int _playersReadied;

    public List<GameObject> EnemyTargetables
    {
        get
        {
            return _enemyTargetables;
        }
    }

    [SerializeField] private List<GameObject> _enemyTargetables;

    public ContainmentPlayer LocalPlayer
    {
        get { return _localPlayer; }
    }

    public List<ContainmentPlayer> Players
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

        this._players = new List<ContainmentPlayer>();
        this._playersReadied = 0;
    }

    private void OnEnable()
    {
        ContainmentPlayer.OnPlayerJoin += RegisterPlayer;
        ContainmentPlayer.OnPlayerLeave += RemovePlayer;
        ContainmentPlayer.OnPlayerDown += HandlePlayerDowned;
        ContainmentPlayer.OnPlayerDeath += HandlePlayerDeath;
        ContainmentPlayer.OnPlayerReady += HandlePlayerReady;


        TargetableManager.OnTargetableEnabled += RegisterTargetable;
        TargetableManager.OnTargetableDisabled += DeregisterTargetable;



        Objective.OnObjectiveEnabled += RegisterObjective;
        Objective.OnObjectiveDisabled += DeregisterObjective;
        Objective.OnObjectiveDestroyed += HandleObjectiveDestroyed;


        //ContainmentPlayer.OnLocalPlayerJoin += RegisterLocalPlayer;
        //ContainmentPlayer.OnLocalPlayerLeave += RemoveLocalPlayer;

        this._playersReadied = 0;
    }

    private void OnDisable()
    {
        ContainmentPlayer.OnPlayerJoin -= RegisterPlayer;
        ContainmentPlayer.OnPlayerLeave -= RemovePlayer;
        ContainmentPlayer.OnPlayerDown -= HandlePlayerDowned;
        ContainmentPlayer.OnPlayerDeath -= HandlePlayerDeath;
        ContainmentPlayer.OnPlayerReady -= HandlePlayerReady;


        TargetableManager.OnTargetableEnabled -= RegisterTargetable;
        TargetableManager.OnTargetableDisabled -= DeregisterTargetable;



        Objective.OnObjectiveEnabled -= RegisterObjective;
        Objective.OnObjectiveDisabled -= DeregisterObjective;
        Objective.OnObjectiveDestroyed -= HandleObjectiveDestroyed;


        //ContainmentPlayer.OnLocalPlayerJoin -= RegisterLocalPlayer;
        //ContainmentPlayer.OnLocalPlayerLeave -= RemoveLocalPlayer;
    }


    // NOTE: For Networking, the parameters for these functions may need to be changed
    // to NetworkIdentity

    // Register player with the game so Enemies can target the players
    public void RegisterPlayer(ContainmentPlayer player)
    {
        this._players.Add(player);
    }

    public void RemovePlayer(ContainmentPlayer player)
    {
        this._players.Remove(player);
    }


    public void RegisterLocalPlayer(ContainmentPlayer player)
    {
        this._localPlayer = player;
    }

    public void RemoveLocalPlayer(ContainmentPlayer player)
    {
        this._localPlayer = null;
    }

    public void RegisterObjective(Objective objective)
    {
        this._objectives.Add(objective);
    }

    public void DeregisterObjective(Objective objective)
    {
        this._objectives.Remove(objective);
    }


    public void RegisterTargetable(ITargetable targetable)
    {
        this._enemyTargetables.Add(targetable.gameObject);
    }

    public void DeregisterTargetable(ITargetable targetable)
    {
        this._enemyTargetables.Remove(targetable.gameObject);
    }


    public void HandlePlayerReady(ContainmentPlayer player)
    {


        _playersReadied++;

        if(_playersReadied >= this._players.Count)
        {
            StartNextWave();
        }


    }



    
    public void HandlePlayerDowned(ContainmentPlayer playerDowned)
    {
        bool gameOver = true;
        foreach(ContainmentPlayer player in this._players)
        {
            if(player == playerDowned)
            {
                player.Downed = true;
            }

            if(!player.Downed && !player.Died)
            {
                gameOver = false;
            }
        }


        if(gameOver)
        {
            Debug.Log("Game Over");
            // Handle Game Over
        }

        // Networking - Notify clients of player down and if game is over
    }

    public void HandlePlayerDeath(ContainmentPlayer playerDied)
    {
        bool gameOver = true;

        foreach(ContainmentPlayer player in this._players)
        {
            if(player == playerDied)
            {
                player.Died = true;
            }

            if(!player.Downed && !player.Died)
            {
                gameOver = false;
            }
        }
        

        if(gameOver)
        {
            Debug.Log("Game Over - All Players Down or Killed");
            // Handle Game Over
        }

        // Networking - Notify clients of player died and if game is over
    }

    public void HandleObjectiveDestroyed(Objective objectiveDestroyed)
    {
        bool gameOver = true;

        foreach(Objective objective in this._objectives)
        {
            if(!objective.Destroyed)
            {
                gameOver = false;
            }
        }

        if(gameOver)
        {
            Debug.Log("Game Over - All Objectives Destroyed");
        }
    }

    public void StartNextWave()
    {
        this._waveSpawner.StartWave();
    }
}
