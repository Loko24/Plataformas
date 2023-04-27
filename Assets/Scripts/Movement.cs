using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rg2D;
    private Animator animator;


    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float jumpForce = 150;
    private float hr;
    private float vr;


    private bool ground = false;
    private bool doubleJump;


    [SerializeField] 
    private LayerMask groundLayer;
    private float groundRayDist = .17f;


    void Start()
    {
        rg2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        checkGround();

        hr = Input.GetAxisRaw("Horizontal");
        vr = Input.GetAxisRaw("Vertical");

        if (hr < 0.0f) flip(-1);
        else if (hr > 0.0f) flip(1);

        if (Input.GetButtonDown("Jump") && (ground))
        {
            rg2D.AddForce(new Vector2(0, jumpForce));
            animator.SetTrigger("jump");
        }
        else if (Input.GetButtonDown("Jump") && doubleJump)
        {
            rg2D.velocity = Vector2.zero;
            rg2D.AddForce(new Vector2(0, jumpForce));
            animator.SetTrigger("jump");
            doubleJump = false;
        }

        if (ground)
        {
            doubleJump = true;
        }

        animator.SetFloat("speedX", Math.Abs(rg2D.velocity.x));
        animator.SetBool("walk", hr != 0.0f);
        animator.SetFloat("speedY", rg2D.velocity.y);
    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    private void movePlayer()
    {
        //Moverse
        if (hr != 0 && rg2D.bodyType == RigidbodyType2D.Dynamic)
        {
            rg2D.velocity = new Vector2(hr * speed, rg2D.velocity.y);
        }
        else
        {
            rg2D.velocity = new Vector2(0, rg2D.velocity.y);
        }
    }

    private void checkGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRayDist, groundLayer);

        ground = hit.collider != null;
    }

    public void flip(int x)
    {
        gameObject.transform.localScale = new Vector3(x, 1, 1);
    }
    
    void OnCollisionStay2D(Collision2D other)
    {
        Vector2 normal = collision.contacts[0].normal;

        if (collision.gameObject.CompareTag("Ground") && normal == Vector2.left)
        {

        }else if (collision.gameObject.CompareTag("Ground") && normal == Vector2.right)
        {

        }
    }
}
