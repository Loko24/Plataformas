using System.IO.Pipes;
using System.IO;
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
    private bool wall = false;
    private bool doubleJump;
    private bool isSticking = false;

    private Vector2 direction;

    [SerializeField] 
    private LayerMask groundLayer;
    private float groundRayDistCheck = .17f;
    private float wallRayDistCheck = .11f;

    void Start()
    {
        rg2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        checkGround();
        checkWallJump();

        hr = Input.GetAxisRaw("Horizontal");
        vr = Input.GetAxisRaw("Vertical");

        if (hr < 0.0f) flip(-1f);
        else if (hr > 0.0f) flip(1f);

        if (Input.GetButtonDown("Jump") && ground && !isSticking)
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

        //Aqui estoy
        if(Input.GetButtonDown("Jump") && isSticking){
            rg2D.constraints &= RigidbodyConstraints2D.FreezeRotation;
            isSticking = false;
            flip(-direction.x);
            animator.Play("JumpUp");
            rg2D.AddForce(new Vector2(direction.x * jumpForce, jumpForce));
        }

        animator.SetFloat("speedX", Math.Abs(rg2D.velocity.x));
        animator.SetBool("walk", hr != 0.0f);
        animator.SetFloat("speedY", rg2D.velocity.y);
        
        direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    }   

    private void FixedUpdate()
    {
        movePlayer();

        if (wall && Math.Abs(hr) > 0.0f && !isSticking)
        {
            rg2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            animator.Play("wall");
            isSticking = true;
        }
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRayDistCheck, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * groundRayDistCheck, Color.red);

        ground = hit.collider != null;
    }

    private void checkWallJump(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallRayDistCheck, groundLayer);        
        Debug.DrawRay(transform.position, direction * wallRayDistCheck, Color.blue);

        wall = hit.collider != null;
    }

    public void flip(float x)
    {
        if(!isSticking)
            gameObject.transform.localScale = new Vector3(x, 1, 1);
    }
}
