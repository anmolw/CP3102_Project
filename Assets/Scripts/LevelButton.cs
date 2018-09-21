using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour {
    public int levelId;
    public GameObject inactive;
    public GameObject active;

	
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Enable() {
        inactive.SetActive(false);
        active.SetActive(true);
    }

    public void LoadLevel() {
        SceneManager.LoadScene(levelId, LoadSceneMode.Single);
    }
}
