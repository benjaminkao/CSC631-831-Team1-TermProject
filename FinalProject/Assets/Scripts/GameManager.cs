using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    NOTSTARTED, PREPARATION, WAVEINPROGRESS, GAMEOVER
}


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager Instance;

    [SerializeField] private List<ContainmentPlayer> _players;
    [SerializeField] private ContainmentPlayer _localPlayer;
    [SerializeField] private List<Objective> _objectives;

    [SerializeField] private WaveSpawner _waveSpawner;
    [SerializeField] private AudioManager _audioManager;

    // Used to give each player a unique player number, total number of players joined, doesn't decrease if players leave
    private int _numPlayersJoined;

    // Used for preparation phase
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



    public GameState GameState
    {
        get { return this._gameState; }
    }

    private GameState _gameState;


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
        this._numPlayersJoined = 0;
        this._playersReadied = 0;
        this._gameState = GameState.NOTSTARTED;
    }

    private void OnEnable()
    {
        ContainmentPlayer.OnPlayerJoin += RegisterPlayer;
        ContainmentPlayer.OnPlayerLeave += RemovePlayer;
        ContainmentPlayer.OnPlayerDown += HandlePlayerDowned;
        ContainmentPlayer.OnPlayerDeath += HandlePlayerDeath;
        ContainmentPlayer.OnPlayerReady += HandlePlayerReady;
        ContainmentPlayer.OnPlayerResurrect += HandlePlayerResurrect;


        TargetableManager.OnTargetableEnabled += RegisterTargetable;
        TargetableManager.OnTargetableDisabled += DeregisterTargetable;



        Objective.OnObjectiveEnabled += RegisterObjective;
        Objective.OnObjectiveDisabled += DeregisterObjective;
        Objective.OnObjectiveDestroyed += HandleObjectiveDestroyed;


        ShieldBeacon.OnShieldBeaconDamaged += NotifyShieldBeaconDamaged;
        ShieldBeacon.OnShieldBeaconCritical += NotifyShieldBeaconCritical;

        WaveSpawner.OnWaveSpawnerEnabled += RegisterWaveSpawner;
        WaveSpawner.OnWaveStart += HandleWaveStart;
        WaveSpawner.OnWaveProgress += HandleWaveProgress;
        WaveSpawner.OnWaveCompleted += HandleWaveEnd;

        AudioManager.OnAudioManagerEnabled += RegisterAudioManager;


        ContainmentPlayer.OnLocalPlayerJoin += RegisterLocalPlayer;
        ContainmentPlayer.OnLocalPlayerLeave += RemoveLocalPlayer;

        this._playersReadied = 0;
    }

    private void OnDisable()
    {
        ContainmentPlayer.OnPlayerJoin -= RegisterPlayer;
        ContainmentPlayer.OnPlayerLeave -= RemovePlayer;
        ContainmentPlayer.OnPlayerDown -= HandlePlayerDowned;
        ContainmentPlayer.OnPlayerDeath -= HandlePlayerDeath;
        ContainmentPlayer.OnPlayerReady -= HandlePlayerReady;
        ContainmentPlayer.OnPlayerResurrect -= HandlePlayerResurrect;


        TargetableManager.OnTargetableEnabled -= RegisterTargetable;
        TargetableManager.OnTargetableDisabled -= DeregisterTargetable;

        AudioManager.OnAudioManagerEnabled -= RegisterAudioManager;


        Objective.OnObjectiveEnabled -= RegisterObjective;
        Objective.OnObjectiveDisabled -= DeregisterObjective;
        Objective.OnObjectiveDestroyed -= HandleObjectiveDestroyed;


        ShieldBeacon.OnShieldBeaconDamaged -= NotifyShieldBeaconDamaged;
        ShieldBeacon.OnShieldBeaconCritical -= NotifyShieldBeaconCritical;

        WaveSpawner.OnWaveSpawnerEnabled -= RegisterWaveSpawner;
        WaveSpawner.OnWaveStart -= HandleWaveStart;
        WaveSpawner.OnWaveProgress -= HandleWaveProgress;
        WaveSpawner.OnWaveCompleted -= HandleWaveEnd;


        ContainmentPlayer.OnLocalPlayerJoin -= RegisterLocalPlayer;
        ContainmentPlayer.OnLocalPlayerLeave -= RemoveLocalPlayer;
    }


    // NOTE: For Networking, the parameters for these functions may need to be changed
    // to NetworkIdentity

    // Register player with the game so Enemies can target the players
    public void RegisterPlayer(ContainmentPlayer player)
    {
        this._players.Add(player);

        player.PlayerNum = this._numPlayersJoined;
        this._numPlayersJoined++;
    }

    public void RemovePlayer(ContainmentPlayer player)
    {
        this._players.Remove(player);
    }


    public void RegisterLocalPlayer(ContainmentPlayer player)
    {
        this._localPlayer = player;
    }

    public void RemoveLocalPlayer()
    {
        this._localPlayer = null;
    }

    public void RegisterWaveSpawner(WaveSpawner waveSpawner)
    {
        this._waveSpawner = waveSpawner;
    }

    public void DeregisterWaveSpawner(WaveSpawner waveSpawner)
    {
        this._waveSpawner = waveSpawner;
    }

    public void RegisterAudioManager(AudioManager audioManager)
    {
        this._audioManager = audioManager;
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

        _enemyTargetables.Remove(playerDowned.gameObject);

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

        _enemyTargetables.Remove(playerDied.gameObject);

        foreach (ContainmentPlayer player in this._players)
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

    public void HandlePlayerResurrect(ContainmentPlayer playerRez)
    {
        if(!_enemyTargetables.Contains(playerRez.gameObject))
        {
            _enemyTargetables.Add(playerRez.gameObject);
        }
    }


    public void HandleWaveStart(bool boss)
    {
        this._playersReadied = 0;

        if (!boss)
        {
            this._audioManager.RpcPlayAIVoiceLine(AudioManager.AIWAVESTART);
        } else
        {
            int rand = (int)UnityEngine.Random.Range(0, 10);

            if (rand <= 7)
            {
                this._audioManager.RpcPlayAIVoiceLine(AudioManager.AIBOSSWAVESTART);
            }
            else
            {
                if (this._players.Count <= 0)
                {
                    return;
                }

                // Notify random player to say voice line
                int randomPlayer = (int)UnityEngine.Random.Range(0, this._players.Count);

                ContainmentPlayer playerToNotify = this._players[randomPlayer];

                playerToNotify.NotifyBossWave();
            }
        }

    }

    public void HandleWaveProgress()
    {
        if (this._players.Count <= 0)
        {
            return;
        }

        // Notify random player to say voice line
        int randomPlayer = (int)UnityEngine.Random.Range(0, this._players.Count);

        ContainmentPlayer playerToNotify = this._players[randomPlayer];

        playerToNotify.NotifyWaveEnding();

    }

    public void HandleWaveEnd()
    {
        this._audioManager.RpcPlayAIVoiceLine(AudioManager.AIWAVEEND);
    }



    public void NotifyShieldBeaconDamaged()
    {
        
        if (this._players.Count <= 0)
        {
            return;
        }

        // Notify random player to say voice line
        int randomPlayer = (int)UnityEngine.Random.Range(0, this._players.Count);

        ContainmentPlayer playerToNotify = this._players[randomPlayer];

        playerToNotify.NotifyShieldBeaconDamaged();
        

    }

    public void NotifyShieldBeaconCritical()
    {
        int rand = (int)UnityEngine.Random.Range(0, 10);

        if (rand <= 7)
        {
            this._audioManager.RpcPlayAIVoiceLine(AudioManager.AISHIELDBEACONLOW);
        }
        else
        {
            if (this._players.Count <= 0)
            {
                return;
            }

            // Notify random player to say voice line
            int randomPlayer = (int)UnityEngine.Random.Range(0, this._players.Count);

            ContainmentPlayer playerToNotify = this._players[randomPlayer];

            playerToNotify.NotifyShieldBeaconCritical();
        }
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
