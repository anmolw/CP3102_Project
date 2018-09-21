using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [SerializeField] GameObject treasureFull;
    [SerializeField] GameObject treasureEmpty;
    public GameManager gameManager;

    public bool isEmpty = false;

    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = treasureFull.GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isEmpty)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = treasureEmpty.GetComponent<SpriteRenderer>().sprite;
            isEmpty = true;
            gameManager.ChestActivated();
        }
    }
}