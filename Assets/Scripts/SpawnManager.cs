using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //-----------------------------------
    // Enemy Prefabs
    [SerializeField]
    private GameObject _easyEnemy;
    [SerializeField]
    private GameObject _mediumEnemy;
    [SerializeField]
    private GameObject _hardEnemy;
    [SerializeField]
    private GameObject _bossEnemy;
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
    public int _totalEnemy = 10;
    [SerializeField]
    private int _numEnemy = 0;
    [SerializeField]
    private int _spawnedEnemy = 0;
    //----------------------------------
    //Methods of Spawning Enemy Waves
    //----------------------------------
    [SerializeField]
    private bool _waveSpawn = false;
    public bool _spawn = true;
    public _spawnTypes _spawnType = _spawnTypes._normal;
    //----------------------------------
    //Timing for Spawning Enemy Wave Intervals
    public float _waveTimer = 30.0f;
    [SerializeField]
    private float _timeTillNextWave = 0.0f;
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
        _spawnID = Random.Range(1, 500);
        _enemies.Add(_enemyLevels._easy, _easyEnemy);
        _enemies.Add(_enemyLevels._medium, _mediumEnemy);
        _enemies.Add(_enemyLevels._hard, _hardEnemy);
        _enemies.Add(_enemyLevels._boss, _bossEnemy);
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
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_easyEnemy, posToSpawn, Quaternion.identity);
            newEnemy.SendMessage("setName", _spawnID);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);

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
        //If enemy spawnerId is equal to this spawnerID then remove an Enemy count
        if (_spawnID == spawnerID)
        {
            _numEnemy--;
        }
    }

    //Enable the Spawner based on spawnerID
    public void enableSpawner(int spawnerID)
    {
        if (_spawnID == spawnerID)
        {
            _spawn = true;
        }
    }
    //Disable the Spawner based on spawnerID
    public void disableSpawner(int spawnerID)
    {
        if (_spawnID == spawnerID)
        {
            _spawn = false;
        }
    }

    //Returns the "Time Till the next Spawn Enemy Wave" for an Interface, etc.
    public float TimeTillNextWave
    {
        get
        {
            return _timeTillNextWave;
        }
    }
    //Enables the spawner. Use this function to trigger events because you don't know the spawner's ID.
    public void enableTrigger()
    {
        _spawn = true;
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;    
    }


}
