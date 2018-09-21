using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public GameManager gameManager;
    public Waypoint[] wayPoints;
    public float speed = 3f;
    public bool isCircular;
    public bool inReverse = true;
    public GameObject cone;


    private Rigidbody2D rb;
    private BoxCollider2D collider;
    private Animator anim;
    private Waypoint currentWaypoint;
    private int currentIndex = 0;
    private bool isWaiting = false;
    private float speedStorage = 0;
    private bool facingRight = true;
    private bool playerDetected = false;
    private bool onladder = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        if (wayPoints.Length > 0)
        {
            currentWaypoint = wayPoints[0];
        }
	}

	// Update is called once per frame
	void Update () {
        if (currentWaypoint != null && !isWaiting && !playerDetected)
        {
            MoveTowardsWaypoint();
        }
	}

    void Pause()
    {
        isWaiting = !isWaiting;
    }

    private void MoveTowardsWaypoint()
    {
        // Get the moving objects current position
        Vector3 currentPosition = this.transform.position;
        //Vector3 currentPosition = GetComponent<SpriteRenderer>().bounds.center;

        // Get the target waypoints position
        Vector3 targetPosition = currentWaypoint.transform.position;

        // If the moving object isn't that close to the waypoint
        if (Vector3.Distance(currentPosition, targetPosition) > 0.01f)
        {

            // Get the direction and normalize
            Vector3 directionOfTravel = targetPosition - currentPosition;
            directionOfTravel.Normalize();

            this.transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

            //scale the movement on each axis by the directionOfTravel vector components
            //this.transform.Translate(
            //    directionOfTravel.x * speed * Time.deltaTime,
            //    directionOfTravel.y * speed * Time.deltaTime,
            //    directionOfTravel.z * speed * Time.deltaTime,
            //    Space.World
            //);
            // Debug.Log(Mathf.Abs(directionOfTravel.y));
            if (Mathf.Abs(directionOfTravel.x) < 0.1 && Mathf.Abs(directionOfTravel.y) > 0.1) {
                // Must be on a ladder
                anim.SetBool("climbing", true);
                collider.enabled = false;
                cone.SetActive(false);
            }
            else {
                anim.SetBool("climbing", false);
                collider.enabled = true;
                cone.SetActive(true);
            }
            if (directionOfTravel.x < 0 && facingRight) {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                facingRight = !facingRight;
            }
            else if(directionOfTravel.x > 0 && !facingRight) {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                facingRight = !facingRight;
            }
            anim.SetBool("walking", true);
        }
        else
        {
            this.transform.position = targetPosition;

            // If the waypoint has a pause amount then wait a bit
            if (currentWaypoint.waitSeconds > 0)
            {
                anim.SetBool("walking", false);
                Pause();
                Invoke("Pause", currentWaypoint.waitSeconds);
            }

            // If the current waypoint has a speed change then change to it
            if (currentWaypoint.speedOut > 0)
            {
                speedStorage = speed;
                speed = currentWaypoint.speedOut;
            }
            else if (speedStorage != 0)
            {
                speed = speedStorage;
                speedStorage = 0;
            }

            NextWaypoint();
        }
    }



    private void NextWaypoint()
    {
        if (isCircular)
        {

            if (!inReverse)
            {
                currentIndex = (currentIndex + 1 >= wayPoints.Length) ? 0 : currentIndex + 1;
            }
            else
            {
                currentIndex = (currentIndex == 0) ? wayPoints.Length - 1 : currentIndex - 1;
            }

        }
        else
        {
            if ((!inReverse && currentIndex + 1 >= wayPoints.Length) || (inReverse && currentIndex == 0))
            {
                inReverse = !inReverse;
            }
            currentIndex = (!inReverse) ? currentIndex + 1 : currentIndex - 1;

        }

        currentWaypoint = wayPoints[currentIndex];
    }

    public void PlayerDetected()
    {
        playerDetected = true;
        anim.SetTrigger("detected");
        anim.SetBool("walking", false);
        cone.SetActive(false);
    }

}
