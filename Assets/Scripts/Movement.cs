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
    private float _speed;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _timeWallJump;
    [SerializeField]
    private float _speedForceXWall;
    [SerializeField]
    private float _speedForceYWall;
    private float _hr;
    private float _vr;
    private float _wallJump;

    private bool _ground = false;
    private bool _wall = false;
    private bool _doubleJump;
    private bool _isSticking = false;

    private Vector2 _direction;

    [SerializeField] 
    private LayerMask _groundLayer;
    private float _groundRayDistCheck = .17f;
    private float _wallRayDistCheck = .11f;

    void Start()
    {
        rg2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        check_Ground();
        checkWallJump();

        _hr = Input.GetAxisRaw("Horizontal");
        _vr = Input.GetAxisRaw("Vertical");

        if (_hr < 0.0f) flip(-1f);
        else if (_hr > 0.0f) flip(1f);

        if (Input.GetButtonDown("Jump") && _ground && !_isSticking)
        {
            rg2D.AddForce(new Vector2(0, _jumpForce));
            animator.SetTrigger("jump");
        }
        else if (Input.GetButtonDown("Jump") && _doubleJump)
        {
            rg2D.velocity = Vector2.zero;
            rg2D.AddForce(new Vector2(0, _jumpForce));
            animator.SetTrigger("jump");
            _doubleJump = false;
        }

        if (_ground)
        {
            _doubleJump = true;
            _wallJump = 0f;
        }

        if(Input.GetButtonDown("Jump") && _isSticking){
            rg2D.constraints &= RigidbodyConstraints2D.FreezeRotation;
            animator.Play("JumpUp");
            rg2D.velocity = new Vector2(_speedForceXWall * -_direction.x, _speedForceYWall);
            StartCoroutine(jumpWall());
        }

        animator.SetFloat("speedX", Math.Abs(rg2D.velocity.x));
        animator.SetBool("walk", _hr != 0.0f);
        animator.SetFloat("speedY", rg2D.velocity.y);
        
        _direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    }   

    private void FixedUpdate()
    {
        if(!_isSticking){
            movePlayer();
        }

        if (_wall && Math.Abs(_hr) > 0.0f && !_isSticking && _wallJump != _direction.x)
        {
            _wallJump = _direction.x;
            _isSticking = true;
            rg2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            animator.Play("wall");
        }
    }

    private void movePlayer()
    {
        if (_hr != 0 && rg2D.bodyType == RigidbodyType2D.Dynamic)
        {
            rg2D.velocity = new Vector2(_hr * _speed, rg2D.velocity.y);
        }
        else
        {
            rg2D.velocity = new Vector2(0, rg2D.velocity.y);
        }
    }

    private void check_Ground()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _groundRayDistCheck, _groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * _groundRayDistCheck, Color.red);

        _ground = hit.collider != null;
    }

    private void checkWallJump(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, _wallRayDistCheck, _groundLayer);        
        Debug.DrawRay(transform.position, _direction * _wallRayDistCheck, Color.blue);

        _wall = hit.collider != null;
    }

    public void flip(float x)
    {
        if(!_isSticking)
            gameObject.transform.localScale = new Vector3(x, 1, 1);
    }

    IEnumerator jumpWall(){
        _isSticking = true;
        yield return new WaitForSeconds (_timeWallJump);
        flip(-_direction.x);
        _isSticking = false;
    }
}
