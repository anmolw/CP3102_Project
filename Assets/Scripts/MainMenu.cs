using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject root;
    public GameObject levelSelect;
    public GameObject musicButtonEnabled;
    public GameObject musicButtonDisabled;
    public LevelButton[] levelButtons;
    int latestLevel;
    public bool musicEnabled;

	// Use this for initialization
	void Start () {
        latestLevel = PlayerPrefs.GetInt("latestLevel", 0);
        musicEnabled = (PlayerPrefs.GetInt("musicEnabled", 1) == 1);
        if (musicEnabled)
        {
            musicButtonEnabled.SetActive(true);
            GetComponent<AudioSource>().Play();
        }
        else
        {
            musicButtonDisabled.SetActive(true);
        }
	}

    public void ToggleMusic() {
        musicEnabled = !musicEnabled;
        PlayerPrefs.SetInt("musicEnabled", (musicEnabled) ? 1 : 0);
        if (musicEnabled)
        {
            musicButtonDisabled.SetActive(false);
            musicButtonEnabled.SetActive(true);
            GetComponent<AudioSource>().Play();
        } else
        {
            musicButtonEnabled.SetActive(false);
            musicButtonDisabled.SetActive(true);
            GetComponent<AudioSource>().Stop();
        }
    }

    public void StartGame() {
        SceneManager.LoadScene("LevelOneCastle", LoadSceneMode.Single);
    }

    public void LevelSelect() {
        root.SetActive(false);
        levelSelect.SetActive(true);
        for (int i = 0; i < latestLevel + 1; i++) {
            levelButtons[i].Enable();
        }
    }

    public void Root()
    {
        levelSelect.SetActive(false);
        root.SetActive(true);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
