using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioClip deathClip;
    public float jumpForce = 700f;

    private int jumpCount = 0;
    private bool isGrounded = false;
    private bool isDead = false;
    public float speed = default;
    public float movespeed = 5f;

    private Rigidbody2D playerRigid = default;
    private Animator animator = default;
    private AudioSource playerAudio = default;


    void Start()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        GFunc.Assert(playerRigid != null);
        GFunc.Assert(animator != null);
        GFunc.Assert(playerAudio != null);
    }


    void Update()
    {
        


        if (isDead) { return; }


        // LEGACY:
        //if (Input.GetMouseButtonDown(0) && jumpCount < 2)
        //{
        //    jumpCount++;
        //    playerRigid.velocity = Vector2.zero;
        //    playerRigid.AddForce(new Vector2(0, jumpForce));
        //    playerAudio.Play();
        //}
        //else if (Input.GetMouseButtonDown(0) && 0 < playerRigid.velocity.y)
        //{
        //    playerRigid.velocity = playerRigid.velocity * 0.5f;
        //}

        //Jump();
        animator.SetBool("Ground", isGrounded);

        float horizontalInput = Input.GetAxis("Horizontal");
        float movement = horizontalInput * movespeed * Time.deltaTime;

        transform.Translate(Vector3.right * movement);

    }
    public void Jump()
    {
        if (jumpCount < 2)
        {
            jumpCount++;
            playerRigid.velocity = Vector2.zero;
            playerRigid.AddForce(new Vector2(0, jumpForce));
            playerAudio.Play();
        }
        else if (0 < playerRigid.velocity.y)
        {
            playerRigid.velocity = playerRigid.velocity * 0.5f;
        }
    }



    private void Die()
    {
        animator.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();

        playerRigid.velocity = Vector2.zero;
        isDead = true;

        GameManager.instance.onPlayerDead();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag.Equals("Dead") && isDead == false)
        {
            Die();
        }
        if(collision.tag.Equals("Coin"))
        {
            Debug.Log("들어오는지?");
            GameManager.instance.AddScore(1);
            collision.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private void moving()
    {

    }


}
   