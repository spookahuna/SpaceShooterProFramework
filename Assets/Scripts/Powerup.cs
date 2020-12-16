using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    //ID for Powerups
    [SerializeField] //0 = Triple Shot, 1 = Speed, 2 = Shields, 3 = Ammo Reload, 4 = Health Collectable, 5 = Heat Seeker
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;
    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);   

            if (player != null)
            {
                switch(powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    case 3:
                        player.AmmoReloadActive();
                        break;
                    case 4:
                        player.HealthCollectableActive();
                        break;
                    case 5:
                        player.HeatSeekerActive(); 
                        break;
                    default:
                    break;

                }
                
            }
            Destroy(this.gameObject);
        }
    }
}
