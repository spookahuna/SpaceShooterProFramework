using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeeker : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6.0f;
    private bool _isEnemyHeatSeeker = false;

    // Start is called before the first frame update

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
