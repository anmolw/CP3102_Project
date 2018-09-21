using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public bool isStealLevel;
    public string nextScene;
    public GameObject exitTrigger1;
    public GameObject exitTrigger2;
    public GameObject door1;
    public GameObject door2;
    public GameObject door1Open;
    public GameObject door2Open;
    public GameObject gameOverScreen;
    public GameObject levelCompleteScreen;
    public Text timeLeftText; // For the level complete screen
    public Text scoreText;
    public Sprite doorOpen;
    public float levelTime = 120f;
    public Text timeText; 
    int numChests;
    int numLeft;
    public bool musicEnabled;
    public bool paused = false;
    bool clockPlayed;
    public AudioClip gameOverSound;
    public AudioClip chestSound;
    public AudioClip clockSound;
    public AudioClip levelMusic;

    AudioSource[] audioSources;

    private bool timerActive = true;

	// Use this for initialization
	void Start () {
        musicEnabled = (PlayerPrefs.GetInt("musicEnabled", 1) == 1);
        audioSources = GetComponents<AudioSource>();
        audioSources[1].clip = levelMusic;
        if (musicEnabled)
        {
            audioSources[1].Play();
        }
        numChests = GameObject.FindGameObjectsWithTag("chest").Length;
        numLeft = numChests;
        musicEnabled = (PlayerPrefs.GetInt("musicEnabled", 1) == 1);
	}

	private void Update()
	{
        if (timerActive && !paused)
        {
            levelTime -= Time.deltaTime;
        }
        if (levelTime < 30 && !clockPlayed)
        {
            audioSources[0].clip = clockSound;
            audioSources[0].Play();
            clockPlayed = true;
        }
        if (levelTime < 0)
        {
            GameOver();
        }
        else
        {
            int minutes = (int)levelTime / 60;
            int seconds = (int)levelTime % 60;
            string secondsStr = seconds.ToString();
            if (seconds < 10)
            {
                secondsStr = "0" + seconds;
            }
            timeText.text = minutes + ":" + secondsStr;
        }
	}

	public void ChestActivated() {
        audioSources[0].clip = chestSound;
        audioSources[0].Play();
        if (numLeft > 0) {
            numLeft--;
            if (numLeft == 0)
            {
                ActivateDoors();
            }
        }
    }

    void ActivateDoors() {
        door1.SetActive(false);
        door2.SetActive(false);
        door1Open.SetActive(true);
        door2Open.SetActive(true);
        exitTrigger1.GetComponent<BoxCollider2D>().enabled = true;
        exitTrigger2.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void NextLevel() {
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }

    public void LevelCompleted() {
        int lastCompleted = PlayerPrefs.GetInt("latestLevel", 0);
        if(SceneManager.GetActiveScene().buildIndex > lastCompleted) {
            PlayerPrefs.SetInt("latestLevel", SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.Save();
        }
        timerActive = false;
        int finaltime = (int)levelTime;
        FindObjectOfType<PlayerController>().enabled = false;
        foreach (EnemyAI ai in FindObjectsOfType<EnemyAI>()) {
            ai.enabled = false;
        }
        levelCompleteScreen.SetActive(true);
        timeLeftText.text = timeLeftText.text + finaltime + "s";
        scoreText.text = scoreText.text + finaltime;
    }

    public void GameOver() {
        audioSources[1].clip = gameOverSound;
        audioSources[1].Play();
        timerActive = false;
        timeText.enabled = false;
        gameOverScreen.SetActive(true);
        FindObjectOfType<PlayerController>().enabled = false;
        foreach (EnemyAI ai in FindObjectsOfType<EnemyAI>())
        {
            ai.enabled = false;
        }
    }

    public void PlayerDetected() {
        Invoke("GameOver", 1.5f);
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TimeExpired () {
        
    }

    public void MainMenu() {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    public void ToggleMusic() {
        musicEnabled = !musicEnabled;
        PlayerPrefs.SetInt("musicEnabled", (musicEnabled) ? 1 : 0);
    }
}