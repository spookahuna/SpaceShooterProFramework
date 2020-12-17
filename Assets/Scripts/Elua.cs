﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elua : MonoBehaviour
{
    //Spawner script communication
    private SpawnManager _spawnManager;
    [SerializeField]
    private int _spawnerID;

    //Enemy Waves Functionality

    //Enemy Movement
    [SerializeField]
    private float _magnitude = 5f;
    private float _swaySpeed = 1f;

    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;

    //Heat Seeker
    [SerializeField]
    private GameObject _heatSeekerPrefab;

    public static Transform _target;

    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        Elua(_spawnerID = 2);

        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyWeapon();
            }
        }

        //Enemy movement from Right to Left
        transform.position = new Vector3((Mathf.Sin(Time.time * _swaySpeed) * _magnitude), transform.position.y, transform.position.z);


    }

    void CalculateMovement()
    {
        float randomX = Random.Range(-8f, 8f);

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            transform.position = new Vector3(randomX, 7, 0);
        }

    }


    void Ekahi(int _ekahi)
    {
        _spawnerID = _ekahi;
    }

    void Elua(int _elua)
    {
        _spawnerID = _elua;
    }

    void Ekolu(int _ekolu)
    {
        _spawnerID = _ekolu;
    }

    void Alii(int _alii)
    {
        _spawnerID = _alii;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(this.gameObject, 2.0f);
        }


        if (other.tag == "Laser" || other.tag == "Heat_Seeker")
        {
            _spawnManager.BroadcastMessage("killEkahi", 1);
            Destroy(other.gameObject);

            //May need to create a different method to kill Enemies based on Spawner ID's
            if (_player != null)
            {
                _spawnManager.killEkahi(1);
                _spawnManager.killEkolu(2);
                _spawnManager.killEkolu(3);
                _spawnManager.killAlii(4);
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.0f);
        }

    }
}