// Justin Diones
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState {SPAWNING, WAITING, COUNTING};    // Check the state of the spawner
    // Define what our wave is
    [System.Serializable]
    public class Wave {
        public string name;     // Name of wave we're spawning in. Regular zombie wave, boss wave, etc.
        public Transform enemy; // Reference to enemy we want to instantiate
        public int count;       // Count of enemies to spawn
        public float rate;      // Rate to spawn in enemies
    }

    public Wave[] waves;
    private int nextWave = 0;   // Store index of wave we want to create next
    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f; // Time between waves, 5 seconds
    public float waveCountdown = 0f;
    private float searchCheck = 1f; // Set search time to 1 second. This is used to check if enemies are alive. We want this on a timer because running the search every frame is taxing on the game. 

    private SpawnState state = SpawnState.COUNTING;

    void Start(){
        if (spawnPoints.Length == 0){
            Debug.Log("No spawn points referenced");
        }
        waveCountdown = timeBetweenWaves;
    }

    void Update(){
        // Check if player has killed all enemies
        if (state == SpawnState.WAITING){
            if (!EnemyIsAlive()){
                // Begin new round if EnemyIsAlive is false
                WaveCompleted();
            } else {
                return; // Simply return until player kills all enemies
            }
        }

        if (waveCountdown <= 0){    // Time to start spawning waves
            if (state != SpawnState.SPAWNING){
                // Start spawning wave when countdown is at 0
                StartCoroutine(SpawnWave(waves[nextWave])); 
            }
        } else {
            // Make sure that time counts down correctly to time and not frames drawn per second (countdown from 5)
            waveCountdown -= Time.deltaTime;
        }
    }


    void WaveCompleted(){
        Debug.Log("Wave Completed!");
        
        // Set state back to counting when wave is completed and restart the countdown timer
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length- 1){
            nextWave = 0;
            Debug.Log("Completed all waves: LOOPING...");
        } else {
            nextWave++;
        }
    }

    // Return true or false if an enemy is still alive
    bool EnemyIsAlive(){
        searchCheck -= Time.deltaTime;
        if (searchCheck <= 0f){
            searchCheck = 1f; 
            if (GameObject.FindGameObjectWithTag("Enemy") == null){
                return false;
            }
        }
        return true;
    }

    // IEnumerator used to write method that has to wait a certain amount of time
    IEnumerator SpawnWave(Wave _wave){
        state = SpawnState.SPAWNING;
        for (int i = 0; i < _wave.count; i++){
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f/_wave.rate);
        }
        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(Transform _enemy){
        // Spawn/Instantiate enemy enemy
        Debug.Log("Spawning Enemy: " + _enemy.name);
        Transform spawnPt = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, spawnPt.position, spawnPt.rotation);
    }
}
