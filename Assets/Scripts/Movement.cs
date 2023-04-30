using System.IO.Pipes;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rg2D;
    private Animator animator;


    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float timeWallJump;
    [SerializeField]
    private float speedForceXWall;
    [SerializeField]
    private float speedForceYWall;
    private float hr;
    private float vr;
    private float wallJump;

    private int _live = 3;

    private bool ground = false;
    private bool wall = false;
    private bool doubleJump;
    private bool isSticking = false;

    private Vector2 direction;

    [SerializeField] 
    private LayerMask groundLayer;
    private float groundRayDistCheck = .17f;
    private float wallRayDistCheck = .11f;

    [SerializeField]
    private TMP_Text _liveUI; 

    void Awake()
    {
        rg2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        life();
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
            wallJump = 0f;
        }

        if(Input.GetButtonDown("Jump") && isSticking){
            rg2D.constraints &= RigidbodyConstraints2D.FreezeRotation;
            animator.Play("JumpUp");
            rg2D.velocity = new Vector2(speedForceXWall * -direction.x, speedForceYWall);
            StartCoroutine(jumpWall());
        }

        animator.SetFloat("speedX", Math.Abs(rg2D.velocity.x));
        animator.SetBool("walk", hr != 0.0f);
        animator.SetFloat("speedY", rg2D.velocity.y);
        
        direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    }   

    private void FixedUpdate()
    {
        if(!isSticking){
            movePlayer();
        }

        if (wall && Math.Abs(hr) > 0.0f && !isSticking && wallJump != direction.x)
        {
            wallJump = direction.x;
            isSticking = true;
            rg2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            animator.Play("wall");
        }
    }

    private void movePlayer()
    {
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

    IEnumerator jumpWall(){
        isSticking = true;
        yield return new WaitForSeconds (timeWallJump);
        flip(-direction.x);
        isSticking = false;
    }

    private void life () {
        _liveUI.text = " " + _live + "X";
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 normal = other.contacts[0].normal;

        if(other.gameObject.CompareTag("Enemy")){
            _live--;
            life();
            animationController(1);
            if(_live == 0){
                animationController(2);
            }
        }
    }

    private void animationController (int x) {
        Debug.Log(x);
        if(x == 0) animator.Play("Idle");
        if(x == 1) animator.Play("Hit");
        if(x == 2) animator.Play("Death");
    }

    private void reloadScene () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
