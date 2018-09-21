using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour {

    public GameManager gameManager;

	void OnTriggerEnter2D(Collider2D collision)
	{
        if(collision.CompareTag("Player")) {
            gameManager.LevelCompleted();
        }
	}
}
