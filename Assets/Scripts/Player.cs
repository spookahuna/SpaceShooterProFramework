using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour

//When the player is out of ammo 
//provide feedback through on-screen elements or sound effects. 
//(ex. beep or ammo count displayed on screen)

{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;

   //Thruster speed
    private float _thruster = 7f;
    [SerializeField]
    private bool _isThrusterActive;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    [SerializeField]
    private int _startAmmoCount = 15;
    [SerializeField]
    private int _ammoOut = 0;

    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldsActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    //Visualizer for shield strength between strong, medium, and weak.
    [SerializeField]
    private int _shieldStrengthLevel;
    //[SerializeField]
    //private bool _shieldStrengthVisualizer;
    //Renderer _shieldRend;

    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    [SerializeField]
    private int _score;
    //Ammo Count
    [SerializeField]
    private int _ammoCount;
    private UIManager _uiManager;

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
            _isThrusterActive = true;
        }
        //if Left Shift key is depressed
        else
        {
            _speed = 3.5f;
            _isThrusterActive = false;
        }
        //then return to normal speed
        
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
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
    //Subtract from 15 to 0 as lasers are fired
    //Disable laser at 0
    //Add audio clip of click when laser ammo counter is 0 when fired
    void FireLaser()
    {

            _canFire = Time.time + _fireRate;

        /*
        if (_startAmmoCount <= _ammoOut)
        {
            
        }
        */

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

/*
                if (_isShieldsActive == true)
                {
                    _isShieldsActive = false; 
                    _shieldVisualizer.gameObject.SetActive(false);
                    return;
                }

*/

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

    /*
    public void SubtractAmmo(int ammo)
    {
        _ammoCount -= ammo;
        _uiManager.UpdateAmmoCount(_ammoCount);
    }
    */
}
