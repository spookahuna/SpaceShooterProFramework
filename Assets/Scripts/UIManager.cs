using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Script Communication between UI and Player
    [SerializeField]
    private Player _playerScript;

    //Ammo Counter in UI
    public int _ammo = 15;
    public Text _ammoCountText;

    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _ammoCountText.text = "Ammo: " + _ammo;
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _playerScript = GameObject.Find("Player").GetComponent<Player>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is Null.");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateAmmo()
    { 
        _ammoCountText.text = "Ammo: " + Player._ammoCount.ToString();
    }


    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "LOSE MONEY, YOU DIED!";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);

        }
    }

}
