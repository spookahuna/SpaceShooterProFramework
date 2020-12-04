﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeeker : MonoBehaviour
{
    //Script Communication between HeatSeeker to Enemy
    [SerializeField]
    private Enemy _enemyScript;

    //Target variable for Heat Seeker to Destroy
    [SerializeField]
    private Transform _target;

    private float _speed = 5.0f;

    //Rotation Speed for heat seeker
    [SerializeField]
    private float _rotationSpeed = 100f;

    private bool _isEnemyHeatSeeker = false;

    private Rigidbody2D _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        //Calling the RigidBody
        _rigidBody = GetComponent<Rigidbody2D>();

    }
    
    // Update is called once per frame
    void Update()
    {
        if (_isEnemyHeatSeeker == false)
        {
            MoveUp();
        }
        
        else
        {
            MoveDown();
        }
    }

    private void FixedUpdate()
    {
        
        //if (_target != null)
        
        //{

            Vector2 direction = (Vector2)_target.position - _rigidBody.position;

            direction.Normalize();

            float _rotateAmount = Vector3.Cross(direction, transform.up).z;

            _rigidBody.angularVelocity = -_rotateAmount * _rotationSpeed;

            _rigidBody.velocity = transform.up * _speed;
        //}

    }

   /*
    void HomingMissleMovement()
    {

        Vector3 _enemyPosition;
        Vector3 _misslePosition;

        _enemyTarget = GameObject.Find("Enemy(Clone)");
        
        if (_enemyTarget == null)
        {
            Debug.LogError("HeatSeeker: _enemyTarget is null");
        }

        else
        {
            _enemyPosition = _enemyTarget.transform.position;
            _misslePosition = this.transform.position;
        }
    }
    */
    
    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyHeatSeeker()
    {
        _isEnemyHeatSeeker = true;
    }

    private void OnTriggerEnter2D(Collider2D other)

    {
        if (other.tag == "Player" && _isEnemyHeatSeeker == true)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
        }
    }

}