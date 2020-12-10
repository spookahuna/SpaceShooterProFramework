using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour

{
    [SerializeField]
    private float _rotateSpeed = 3.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private Animator _anim;
    private AudioSource _audioSource;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _anim = transform.GetComponent<Animator>();
        _audioSource = transform.GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {

        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _anim.SetBool("Explode", true);
            _audioSource.Play();
            _spawnManager.StartSpawning();
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.25f);
        }
    }

}
