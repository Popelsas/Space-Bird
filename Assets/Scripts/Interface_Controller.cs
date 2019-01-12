using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Interface_Controller : MonoBehaviour {

    private TMP_Text _scoreText;
    private TMP_Text _LevelOfDifficulty;
    private TMP_Text _gameOvertext;
    private float _level = 1;

    private float _score;

    private void Awake()
    {
        Messenger<float>.AddListener(GameEvent.SCORE_CHANGE, Score);
        Messenger<float>.AddListener(GameEvent.LEVEL, Level);
        Messenger<bool>.AddListener(GameEvent.GAMEOVER, GameOver);        
    }
    // Use this for initialization
    void Start () {
        _scoreText = transform.Find("Score").GetComponent<TMP_Text>();
        _LevelOfDifficulty = transform.Find("Level of difficulty").GetComponent<TMP_Text>();
        _LevelOfDifficulty.text = "Level Of Difficulty " + string.Format("{0:0.00}", _level);
        _gameOvertext = transform.Find("GameOver").GetComponent<TMP_Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Score(float score)
    {
        _score = score;
        _scoreText.text = "Score " + string.Format("{0:0}", _score);
    }

    private void Level(float level)
    {
        _level = level;
        _LevelOfDifficulty.text = "Level Of Difficulty " + string.Format("{0:0.00}", _level);
    }

    public void Restart()
    {
        //_gameOvertext.gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }

    private void GameOver(bool gOver)
    {
        if (gOver)  //Возможно в дальнейшем будут другие вварианты
        {
            if (_gameOvertext) _gameOvertext.gameObject.SetActive(true);
            _gameOvertext.text = "Game Over \n" +
                "Your Score: " + string.Format("{0:0}", _score);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        Messenger<float>.RemoveListener(GameEvent.SCORE_CHANGE, Score);
        Messenger<float>.RemoveListener(GameEvent.LEVEL, Level);
        Messenger<bool>.AddListener(GameEvent.GAMEOVER, GameOver);
    }


}
