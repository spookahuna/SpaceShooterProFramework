using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //[SerializeField]
    //private Enemy _enemy;
    //-----------------------------------
    // Enemy Prefabs
    [SerializeField]
    private GameObject _ekahi;
    [SerializeField]
    private GameObject _elua;
    [SerializeField]
    private GameObject _ekolu;
    [SerializeField]
    private GameObject _alii;
    [SerializeField]
    private Dictionary<_enemyLevels, GameObject> _enemies = new Dictionary<_enemyLevels, GameObject>(4);
    //----------------------------------
    // ID of the Spawner
    private int _spawnID;
    //----------------------------------
    //Trash Collector for Enemy Clones
    [SerializeField]
    private GameObject _enemyContainer;
    //----------------------------------
    //Calling Powerups
    [SerializeField]
    private GameObject[] powerups;
    //-----------------------------------
    [SerializeField]
    private bool _stopSpawning = false;
    //-----------------------------------
    //ENEMY WAVES FUNCTIONALITY
    //Types of Enemy Spawn Waves
    public enum _spawnTypes
    {
        _normal,
        _once,
        _wave,
        _timedWave
    }
    //-----------------------------------
    //Different Enemy Levels
    public enum _enemyLevels
    {
        _easy,
        _medium,
        _hard,
        _boss
    }
    //-----------------------------------
    //Level set to Spawn Enemy Waves 
    public _enemyLevels enemyLevel = _enemyLevels._easy;
    //-----------------------------------
    //How many Enemies have been created. How many Enemies need to be created.
    public int _totalEnemy = 1;
    [SerializeField]
    private int _numEnemy = 0;
    [SerializeField]
    private int _spawnedEnemy = 0;
    //----------------------------------
    //Method of Spawning Enemy Waves
    //----------------------------------
    [SerializeField]
    private bool _waveSpawn = false;
    public bool _spawn = true;
    public _spawnTypes _spawnType = _spawnTypes._normal;
    //----------------------------------
    //Timing for Spawning Enemy Wave Intervals
    public float _waveTimer = 5.0f;
    [SerializeField]
    private float _timeTillNextWave = 4.0f;
    //----------------------------------
    //Variables to Control Spawning Enemy Waves
    public int _totalWaves = 5;
    [SerializeField]
    private int _numWaves = 0;
    //----------------------------------

    // Start is called before the first frame update
    public void Start()
    {
        //Randomizes Spawner ID
        //_spawnID = Random.Range(1, 4);
        _spawnType = _spawnTypes._timedWave;
        _enemies.Add(_enemyLevels._easy, _ekahi);
        _enemies.Add(_enemyLevels._medium, _elua);
        _enemies.Add(_enemyLevels._hard, _ekolu);
        _enemies.Add(_enemyLevels._boss, _alii);
    }

    // spawns an enemy based on the enemy level that you selected
    public void SpawnEkahi()
    {
        Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
        GameObject newEnemy = Instantiate(_enemies[enemyLevel], posToSpawn, Quaternion.identity);
        newEnemy.SendMessage("Ekahi", _spawnID);
        _numEnemy++;
        _spawnedEnemy++;
        newEnemy.transform.parent = _enemyContainer.transform;
    }


    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }


    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(4.0f);

        while (_stopSpawning == false)
        {

            if (_spawn)
            {

                /*
                //Spawns Enemies in Waves. Spawns more Enemies when ALL Enemies die.
                if (_spawnType == _spawnTypes._wave)
                {
                    if (_numWaves < _totalWaves + 1)
                    {
                        if (_waveSpawn == false)
                        {
                            SpawnEnemy();
                        }
                        if (_numEnemy == 0)
                        {
                            //Enables the Enemy Wave Spawner
                            _waveSpawn = true;
                            //Increase number of Enemy Waves
                            _numWaves++;
                            Debug.Log("Next Wave");
                        }
                        if (_numEnemy == _totalEnemy)
                        {
                            //Disables Enemy Wave Spawner
                            _waveSpawn = false;
                        }
                    }
                */
                if (_spawnType == _spawnTypes._timedWave)
                {
                    //Check if number of Waves is greater than Total Waves
                    if (_numWaves <= _totalWaves)
                    {
                        // Increases the timer to allow the timed waves to work
                        _timeTillNextWave += Time.deltaTime;

                        if (_waveSpawn == false)
                        {
                            //Spawn Enemy
                            SpawnEkahi();
                            Debug.Log("Spawned Ekahi");
                        }
                        //Check if Time is equal to Time for New Wave
                        if (_timeTillNextWave >= _waveTimer)
                        {
                            //Enable the wave spawner
                            _waveSpawn = true;

                            //Set Time back to Zero
                            _timeTillNextWave = 0.0f;

                            //Increase the number of Enemy Waves
                            _numWaves++;

                            //Spawn same number of enemies no matter how many get killed
                            _numEnemy = 0;
                        }

                        if (_numEnemy >= _totalEnemy)
                        {
                            //Diable Enemy Wave Spawner
                            _waveSpawn = false;
                        }
                    }

                    /*
                    else
                    {
                        _spawn = false;
                    }
                    */
                }

            }

            yield return new WaitForSeconds(5.0f);

        }

    }

    //Returns the Time Till the Next Wave
    public float TimeTillWave
    {
        get
        {
            return _timeTillNextWave;
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        while (_stopSpawning == false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 6);
            Instantiate(powerups[randomPowerUp], postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(2, 5));
        }

    }

    public void killedEnemy(int spawnerID)
    {
        //If Enemy's spawnerId is equal to this spawnerID then remove an Enemy count
        if (_spawnID == spawnerID)
        {
            _numEnemy--;
        }
    }

    //Enables the Spawner based on spawnerID
    public void enableSpawner(int spawnerID)
    {
        if (_spawnID == spawnerID)
        {
            _spawn = true;
        }
    }

    //Disables Spawner based on spawnerID 
    public void disableSpawner(int spawnerID)
    {
        if (_spawnID == spawnerID)
        {
            _spawn = false;
        }
    }
    //Enables Spawner. Method needs for SpawnID
    public void enableSpawnTrigger()
    {
        _spawn = true;
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }


}
