using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float jumpForce;
    private float moveHorizontal;
    private float moveVertical;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool facingRight = true;
    private bool isGrounded;
    private bool isClimbing = false;
    private bool isHidden = false;
    private bool isDetected = false;

    public GameManager gameManager;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public LayerMask whatIsLadder;
    public float distance;
    private Animator animator;
    private int updateSkip = 0;
    public AudioClip jumpSound;



    private bool hasJumped = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (updateSkip == 1) {
            updateSkip = 0;
        }
        else {
            isGrounded = Physics2D.IsTouchingLayers(GetComponent<Collider2D>(), whatIsGround);
        }
         //isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        RaycastHit2D hitinfoUp = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);
        RaycastHit2D hitinfoDown = Physics2D.Raycast(transform.position, Vector2.down, distance, whatIsLadder);


        if (hitinfoUp.collider != null || (hitinfoDown.collider != null && !isGrounded))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isClimbing = true;
                animator.SetBool("climbing", isClimbing);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                isClimbing = true;
                animator.SetBool("climbing", isClimbing);
            }

        }
        else
        {
            isClimbing = false;
            animator.SetBool("climbing", isClimbing);
            animator.speed = 1;
        }

        if (isClimbing && !isDetected)
        {
            moveVertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(0, rb.position.y);
            rb.velocity = new Vector2(rb.position.x, moveVertical * speed);
            rb.gravityScale = 0;
            if (moveVertical == 0f)
            {
                animator.speed = 0;
            }
            else
            {
                animator.speed = 1;
            }
        }
        else
        {
            rb.gravityScale = 1;
        }

        moveHorizontal = Input.GetAxis("Horizontal");
        Debug.Log(moveHorizontal);
        if (!isDetected)
        {
            rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
        }

        if (!facingRight && moveHorizontal > 0 && !isDetected)
        {
            Flip();
        }
        else if (facingRight && moveHorizontal < 0 && !isDetected)
        {
            Flip();
        }
        animator.SetBool("grounded", isGrounded);
        if (isGrounded)
        {
            animator.SetBool("jumping", false);
        }
        animator.SetFloat("velocityX", Mathf.Abs(rb.velocity.x) / speed);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isDetected)
        {
            GetComponent<AudioSource>().clip = jumpSound;
            //rb.velocity = Vector2.up * jumpForce;
            rb.AddForce(rb.transform.InverseTransformDirection(Vector2.up) * 190);
            animator.SetBool("jumping", true);
            GetComponent<AudioSource>().Play();
            isGrounded = false;
            animator.SetBool("grounded", isGrounded);
            updateSkip = 1;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("cover"))
        {
            SpriteRenderer coverSprite = collision.gameObject.GetComponent<SpriteRenderer>();
            Color color = coverSprite.color;
            color.a = 0.7f;
            coverSprite.color = color;
            isHidden = true;
            animator.SetBool("hidden", isHidden);
        }
        else if (collision.CompareTag("enemy") && !isHidden && !isDetected)
        {
            EnemyAI enemy = collision.GetComponent<EnemyAI>();
            enemy.PlayerDetected();
            Detected();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy") && !isHidden && !isDetected)
        {
            EnemyAI enemy = collision.GetComponent<EnemyAI>();
            enemy.PlayerDetected();
            Detected();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("cover"))
        {
            SpriteRenderer coverSprite = collision.gameObject.GetComponent<SpriteRenderer>();
            Color color = coverSprite.color;
            color.a = 1.0f;
            coverSprite.color = color;
            isHidden = false;
            animator.SetBool("hidden", isHidden);
        }
    }

    void Update()
    {
        
    }

    public void Detected()
    {
        rb.velocity = new Vector2(0, 0);
        animator.SetTrigger("detected");
        isDetected = true;
        gameManager.PlayerDetected();
    }

    void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !facingRight;
    }

    public bool hidden()
    {
        return isHidden;
    }

	//private void OnCollisionStay2D(Collision2D collision)
	//{
 //       if (collision.collider.la == whatIsGround.value) {
 //           isGrounded = true;
 //       }
	//}
}
