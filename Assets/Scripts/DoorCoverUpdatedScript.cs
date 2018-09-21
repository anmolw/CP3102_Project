using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCoverUpdatedScript : MonoBehaviour {

    [SerializeField]
    GameObject propsDoorOpen;

    [SerializeField]
    GameObject propsDoorClosed;

    //public bool isClosed = false;
    public void Start()

    {
        gameObject.GetComponent<SpriteRenderer>().sprite = propsDoorOpen.GetComponent<SpriteRenderer>().sprite;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = propsDoorOpen.GetComponent<SpriteRenderer>().sprite;
            Debug.Log("Enter collosion");
        }
           
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = propsDoorClosed.GetComponent<SpriteRenderer>().sprite;
            Debug.Log("Staycollosion");
        }
            
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = propsDoorOpen.GetComponent<SpriteRenderer>().sprite;
        Debug.Log("Left collosion");
        }
            
    }
  
}
