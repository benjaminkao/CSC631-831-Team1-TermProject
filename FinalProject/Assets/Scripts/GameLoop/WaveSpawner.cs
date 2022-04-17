// Justin Diones
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WaveSpawner : NetworkBehaviour
{
    public static event Action<WaveSpawner> OnWaveSpawnerEnabled;
    public static event Action<WaveSpawner> OnWaveSpawnerDisabled;
    public static event Action OnWaveStart;
    public static event Action OnWaveCompleted;

    //public enum SpawnState {SPAWNING, WAITING, COUNTING};    // Check the state of the spawner
    public SpawnManager spawnManager;
    public LightingManager lightingManager;

    // Define what our wave is
    [System.Serializable]
    public class Wave {

        [System.Serializable]
        public class WaveItem
        {
            public Transform enemy;
            public int count;
            [Tooltip("Everytime this wave is spawned, how much more difficult does it get. How many more of this enemy is spawned each time.")]
            public int addRate;
        }


        public bool boss;
        public string name;     // Name of wave we're spawning in. Regular zombie wave, boss wave, etc.

        public List<WaveItem> enemies;

        //public Transform enemy; // Reference to enemy we want to instantiate
        //public Transform enemy2;    // Second enemy transform to add new enemy type

        //public int count;       // Count of enemies to spawn
        //public int count2;
        public float spawnRate;      // Rate to spawn in enemies

        [Tooltip("Every nth wave, this wave will spawn.")]
        public int nthWave = 1;
    }

    public Wave[] waves;
    private int nextWave = 1;   // Store index of wave we want to create next
    private Wave currentWave;
    public Transform[] spawnPoints; // Array of spawn points

    public float timeBetweenWaves = 5f; // Time between waves, 5 seconds
    //public float waveCountdown = 0f;
    //private float searchCheck = 1f; // Set search time to 1 second. This is used to check if enemies are alive. We want this on a timer because running the search every frame is taxing on the game. 
    [SerializeField] private float numberOfEnemiesSpawned;
    [SerializeField] private float numberOfEnemiesDied;

    //private SpawnState state = SpawnState.COUNTING;     // Set default state to COUNTING

    void Start(){
    
        // Error if no spawn points are set
        if (spawnPoints.Length == 0){
            Debug.Log("No spawn points referenced");
        }
        //waveCountdown = timeBetweenWaves;
    }


    private void OnEnable()
    {
        Enemy.OnEnemyDied += HandleEnemyDied;

        OnWaveSpawnerEnabled?.Invoke(this);
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= HandleEnemyDied;

        OnWaveSpawnerDisabled?.Invoke(this);
    }



    //void Update(){

    //    if(!isServer)
    //    {
    //        return;
    //    }


    //    //// Check if player has killed all enemies
    //    //if (state == SpawnState.WAITING){
    //    //    if (!EnemyIsAlive()){
    //    //        // Begin new round if EnemyIsAlive is false
    //    //        Debug.Log("Should turn to day");
    //    //        RpcDay();
    //    //        WaveCompleted(waves[nextWave]);
    //    //    } else {
    //    //        return; // Simply return until player kills all enemies
    //    //    }
    //    //}

    //    //if (waveCountdown <= 0)
    //    //{    // Time to start spawning waves
    //    //    if (state != SpawnState.SPAWNING)
    //    //    {
    //    //        // Start spawning wave when countdown is at 0
    //    //        StartCoroutine(SpawnWave(waves[nextWave]));
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    // Make sure that time counts down correctly to time and not frames drawn per second (countdown from 5)
    //    //    waveCountdown -= Time.deltaTime;
    //    //}
    //}

    public void StartWave()
    {
        if(!isServer)
        {
            return;
        }


        this.numberOfEnemiesSpawned = 0;
        this.numberOfEnemiesDied = 0;

        StartCoroutine(SpawnWave(GetWaveType(nextWave)));
        RpcNight();
    }





    // Tell the game what to do when a wave is completed
    void WaveCompleted(Wave _wave){
        
        if (!isServer)
        {
            return;
        }

        Debug.Log("Wave Completed!");

        // Set state back to counting when wave is completed and restart the countdown timer
        //state = SpawnState.COUNTING;
        //waveCountdown = timeBetweenWaves;


        UpdateWaveInfo(_wave);

        //if (nextWave + 1 > waves.Length- 1){
        //    // Loop and increment final wave set by inspector

        //    foreach(WaveSpawner.Wave.WaveItem waveItem in _wave.enemies)
        //    {
        //        waveItem.count += 2;
        //    }

        //    _wave.spawnRate += 0.5f;
        //    Debug.Log("Completed all waves: LOOPING...");
        //} else {
        nextWave++;
        //}

        // Make it daytime to indicate Preparation Phase
        RpcDay();

        OnWaveCompleted?.Invoke();
    }




    // Return true or false if an enemy is still alive by checking for objects in the scene with an "Enemy" tag
    //bool EnemyIsAlive(){
    //    searchCheck -= Time.deltaTime;
    //    if (searchCheck <= 0f){
    //        searchCheck = 1f; 
    //        if (GameObject.FindGameObjectWithTag("Enemy") == null){
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    Wave GetWaveType(int waveNum)
    {
        Wave waveToSpawn = null;

        foreach(Wave wave in this.waves)
        {
            if(wave.nthWave == 0 || waveNum % wave.nthWave == 0)
            {
                // wave is a valid wave to spawn for waveNum

                // Need to check if the nthWave is greater than the current waveToSpawn's nthWave
                if(waveToSpawn == null || waveToSpawn.nthWave < wave.nthWave)
                {
                    waveToSpawn = wave;
                }

            }
        }


        return waveToSpawn;
    }


    void HandleEnemyDied()
    {
        if(!isServer)
        {
            return;
        }

        this.numberOfEnemiesDied++;

        if(this.numberOfEnemiesDied >= this.numberOfEnemiesSpawned)
        {
            WaveCompleted(currentWave);
        }

    }




    // IEnumerator used to write method that has to wait a certain amount of time
    IEnumerator SpawnWave(Wave _wave){
        // Make night to indicate wave starting
        OnWaveStart?.Invoke();

        RpcNight();

        currentWave = _wave;

        if (_wave == null)
        {
            Debug.LogError("No wave specified. No wave can spawn.");

        }
        else
        {
            Debug.Log($"Wave #{nextWave}: Spawning \"{_wave.name}\" Wave.");


            for (int i = 0; i < _wave.enemies.Count; i++)
            {
                WaveSpawner.Wave.WaveItem waveItem = _wave.enemies[i];

                for (int j = 0; j < waveItem.count; j++)
                {

                    SpawnEnemy(waveItem.enemy);
                    yield return new WaitForSeconds(1f / _wave.spawnRate);
                }


            }

            //state = SpawnState.SPAWNING;
            //for (int i = 0; i < _wave.count; i++){
            //    SpawnEnemy(_wave.enemy);
            //    yield return new WaitForSeconds(1f/_wave.rate);
            //}
            //for (int i = 0; i < _wave.count2; i++){
            //    SpawnEnemy(_wave.enemy2);
            //    yield return new WaitForSeconds(1f/_wave.rate);
            //}
            //state = SpawnState.WAITING;

        }

        yield break;
    }

    void UpdateWaveInfo(Wave wave)
    {

        wave.spawnRate += 0.05f;
        

        foreach(Wave.WaveItem enemy in wave.enemies)
        {
            enemy.count += enemy.addRate;
        }


    }



    void SpawnEnemy(Transform _enemy){
        // Spawn/Instantiate enemy enemy
        this.numberOfEnemiesSpawned++;
        Transform spawnPt = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        //Poolable networkEnemy = spawnManager.Get(_enemy.GetComponent<Poolable>(), spawnPt.position);

        //networkEnemy.Spawn();

        GameObject networkEnemy = Instantiate(_enemy.gameObject, spawnPt.position, spawnPt.rotation);
        NetworkServer.Spawn(networkEnemy.gameObject);
    }


    [ClientRpc]
    public void RpcNight()
    {
        Debug.Log("Changing to Night");
        lightingManager.SetTime(5);
        
    }

    [ClientRpc]
    public void RpcDay()
    {
        Debug.Log("Changing to Day");
        lightingManager.SetTime(8);
        
    }
}
