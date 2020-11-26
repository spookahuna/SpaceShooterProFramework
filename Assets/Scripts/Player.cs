using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour

{
    //Script Communication between Player and UI


    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;

    //Thruster speed
    private float _thruster = 7f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    //Ammo Functionality
    public static int _ammoCount = 15;
    public static int _ammo;

    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldsActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    //Shield Strength
    [SerializeField]
    private int _shieldStrengthLevel;
    //Visualizer for shield color transition
    //Renderer _shieldRend;

    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    [SerializeField]
    private int _score;

    public UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>(); 
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is null!");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

    }
    void Update()
    {
        //if Left Shift key is pressed 
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //then activate Thrusters
            _speed = _thruster;
        }
        //else return to normal speed
        else
        {
            _speed = 3.5f;
        }


        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && _ammoCount > 0 && Time.time > _canFire)
        {
            FireLaser();

            Debug.Log("UI Update is being called after Fire Laser");
        }

    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        if (_isSpeedBoostActive == false)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
        }


        if (transform.position.y >= 11.3f)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.x, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

    }

    //Display 15 on ammo counter on UI at Game Start
    //Subtract from 15 to 0 as lasers are fired - Happening only in Player's Inspector
    //Disable laser at 0 - PAU
    //Add audio clip of click when laser ammo counter is 0 when fired

    void FireLaser()
    {
        _ammoCount -= 1;

        _uiManager.UpdateAmmo(); 

        _canFire = Time.time + _fireRate;

        if (_ammoCount < 0)
        {
            _ammoCount = 0;

            //When ammo is empty Play empty ammo chamber sound.
            //_audioSource

        }

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);        
        }

        _audioSource.Play(); 

    }

    public void Damage()
    {

                if (_isShieldsActive == true)
                {

                    ShieldStrengthFunction();
                    Debug.Log("ShieldStrengthFunction works!");
                    //_shieldRend = GetComponent<Renderer>();
                    return;
                 }


        _lives--;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }

       else if (_lives ==1)
        {
            _rightEngine.SetActive(true);
        }


        _uiManager.UpdateLives(_lives);


        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    //Visualize the strength of the shield. 
    //Through UI onscreen or color changing of the shield.
    //Allow for 3 hits on the shield to accommodate visualization.
    public void ShieldStrengthFunction()
            {
            _shieldStrengthLevel--;
            switch (_shieldStrengthLevel)
                {
                    default:                   
                        return;
                    case 2:
                        _shieldVisualizer.transform.GetComponent<SpriteRenderer>().color = Color.yellow;
                        //transform.GetComponent<SpriteRenderer>().color = Color.yellow;
                        return;
                    case 1:
                        _shieldVisualizer.transform.GetComponent<SpriteRenderer>().color = Color.red;
                        //transform.GetComponent<SpriteRenderer>().color = Color.red;
                        return;
                    case 0:
                        _shieldVisualizer.transform.GetComponent<SpriteRenderer>().color = Color.blue;
                        _isShieldsActive = false;
                        //transform.GetComponent<SpriteRenderer>().color = Color.?;
                        _shieldVisualizer.SetActive(false);  
                        return;
                }

    }
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false; 

    }

    public void ShieldsActive()
    {
        _shieldStrengthLevel = 3;
        _isShieldsActive = true;
        _shieldVisualizer.transform.GetComponent<SpriteRenderer>().color = Color.blue;
        _shieldVisualizer.gameObject.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

}
