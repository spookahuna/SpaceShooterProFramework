using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
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
	[SerializeField]
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
        _spawnID = Random.Range(1, 5);
        _spawnType = _spawnTypes._timedWave;
        _enemies.Add(_enemyLevels._easy, _ekahi);
        _enemies.Add(_enemyLevels._medium, _elua);
        _enemies.Add(_enemyLevels._hard, _ekolu);
        _enemies.Add(_enemyLevels._boss, _alii);
    }

	// Start is called before the first frame update

	void Update()
	{
		if (_spawn)
		{
			// Spawns enemies everytime one dies
			if (_spawnType == _spawnTypes._normal)
			{
				// checks to see if the number of spawned enemies is less than the max num of enemies
				if (_numEnemy < _totalEnemy)
				{
					// spawns an enemy
					spawnEnemy();
				}
			}
			// Spawns enemies only once
			else if (_spawnType == _spawnTypes._once)
			{
				// checks to see if the overall spawned num of enemies is more or equal to the total to be spawned
				if (_spawnedEnemy >= _totalEnemy)
				{
					//sets the spawner to false
					_spawn = false;
				}
				else
				{
					// spawns an enemy
					spawnEnemy();
				}
			}
			//spawns enemies in waves, so once all are dead, spawns more
			else if (_spawnType == _spawnTypes._wave)
			{
				if (_numWaves < _totalWaves + 1)
				{
					if (_waveSpawn)
					{
						//spawns an enemy
						spawnEnemy();
					}
					if (_numEnemy == 0)
					{
						// enables the wave spawner
						_waveSpawn = true;
						//increase the number of waves
						_numWaves++;
					}
					if (_numEnemy == _totalEnemy)
					{
						// disables the wave spawner
						_waveSpawn = false;
					}
				}
			}
			// Spawns enemies in waves but based on time.
			else if (_spawnType == _spawnTypes._timedWave)
			{
				// checks if the number of waves is bigger than the total waves
				if (_numWaves <= _totalWaves)
				{
					// Increases the timer to allow the timed waves to work
					_timeTillNextWave += Time.deltaTime;
					if (_waveSpawn)
					{
						//spawns an enemy
						spawnEnemy();
					}
					// checks if the time is equal to the time required for a new wave
					if (_timeTillNextWave >= _waveTimer)
					{
						// enables the wave spawner
						_waveSpawn = true;
						// sets the time back to zero
						_timeTillNextWave = 0.0f;
						// increases the number of waves
						_numWaves++;
						// A hack to get it to spawn the same number of enemies regardless of how many have been killed
						_numEnemy = 0;
					}
					if (_numEnemy >= _totalEnemy)
					{
						// diables the wave spawner
						_waveSpawn = false;
					}
				}
				else
				{
					_spawn = false;
				}
			}
		}
	}
	
	// spawns an enemy based on the enemy level that you selected
	private void spawnEnemy()
	{
		GameObject Enemy = (GameObject)Instantiate(_enemies[enemyLevel], gameObject.transform.position, Quaternion.identity);
		Enemy.SendMessage("killEkahi", SendMessageOptions.DontRequireReceiver);
		// Increase the total number of enemies spawned and the number of spawned enemies
		_numEnemy++;
		_spawnedEnemy++;
	}

	public void StartSpawning()
	{
		//StartCoroutine(SpawnEnemyRoutine());
		StartCoroutine(SpawnPowerupRoutine());
	}

	// Call this function from the enemy when it "dies" to remove an enemy count
	public void killEkahi(int sID)
	{
		// if the enemy's spawnId is equal to this spawnersID then remove an enemy count
		if (_spawnID == sID)
		{
			_numEnemy--;
		}
	}

	public void killElua(int sID)
	{
		// if the enemy's spawnId is equal to this spawnersID then remove an enemy count
		if (_spawnID == sID)
		{
			_numEnemy--;
		}
	}

	public void killEkolu(int sID)
	{
		// if the enemy's spawnId is equal to this spawnersID then remove an enemy count
		if (_spawnID == sID)
		{
			_numEnemy--;
		}
	}

	public void killAlii(int sID)
	{
		// if the enemy's spawnId is equal to this spawnersID then remove an enemy count
		if (_spawnID == sID)
		{
			_numEnemy--;
		}
	}

	//enable the spawner based on spawnerID
	public void enableSpawner(int sID)
	{
		if (_spawnID == sID)
		{
			_spawn = true;
		}
	}
	//disable the spawner based on spawnerID
	public void disableSpawner(int sID)
	{
		if (_spawnID == sID)
		{
			_spawn = false;
		}
	}
	// returns the Time Till the Next Wave, for a interface, ect.
	public float TimeTillWave
	{
		get
		{
			return _timeTillNextWave;
		}
	}
	// Enable the spawner, useful for trigger events because you don't know the spawner's ID.
	public void enableTrigger()
	{
		_spawn = true;
	}

//------------------------------------------------------------------------------------
	//Powerups Spawning Method
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

//------------------------------------------------------------------------------------
//Stop Spawning when Player dies
	public void OnPlayerDeath()
    {
		_stopSpawning = true;
		_spawn = false;
		_waveSpawn = true;
    }


}
