using System.IO.Pipes;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rg2D;
    private Animator animator;
    private BoxCollider2D bx2D;

    [Header("Player stats")]
    [SerializeField]
    private int _live = 3;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _speedForceXWall;
    [SerializeField]
    private float _speedForceYWall;

    [Header("Delay")]
    [SerializeField]
    private float _timeWallJump;

    [Header("UI Fileds")]
    [SerializeField]
    private TMP_Text _liveUI;
    [SerializeField]
    private TMP_Text _fruitUI;

    [Header("Validations")]
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private float _groundRayDistCheck = .17f;
    [SerializeField]
    private float _wallRayDistCheck = .11f;

    private GameObject[] _fruits;
    private int _fruit;
    private int _fruitMax;

    private float _hr;
    private float _wallJump;
    private bool _ground = false;
    private bool _wall = false;
    private bool _doubleJump;
    private bool _isSticking = false;

    private Vector2 _direction;
    private Vector3 _position;

    private Timer _timer;


    [SerializeField]
    private GameObject _panel;
    private bool _blockMove = false;

    void Awake()
    {

        _timer = FindAnyObjectByType<Timer>();
        rg2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bx2D = GetComponent<BoxCollider2D>();      
        _fruits = GameObject.FindGameObjectsWithTag("Fruit");
    }

    void Start()
    {
        _position = transform.position;
        _fruitMax = _fruits.Length;
        Life();
    }

    void Update()
    {
        CheckGround();
        CheckWallJump();

        _hr = Input.GetAxisRaw("Horizontal");

        if (_hr < 0.0f) Flip(-1f);
        else if (_hr > 0.0f) Flip(1f);

        if (Input.GetButtonDown("Jump") && _ground && !_isSticking)
        {
            rg2D.AddForce(new Vector2(0, _jumpForce));
            animator.SetTrigger("jump");
        }
        else if (Input.GetButtonDown("Jump") && _doubleJump && !_isSticking)
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

        if (Input.GetButtonDown("Jump") && _isSticking)
        {
            rg2D.constraints &= RigidbodyConstraints2D.FreezeRotation;
            animator.Play("JumpUp");
            rg2D.velocity = new Vector2(_speedForceXWall * -_direction.x, _speedForceYWall);
            StartCoroutine(JumpWall());
        }

        if (Input.GetAxisRaw("Horizontal") == -_direction.x && _isSticking)
        {
            rg2D.constraints &= RigidbodyConstraints2D.FreezeRotation;
            animator.Play("JumpDown");
            Flip(-_direction.x);
            _isSticking = false;
        }

        animator.SetFloat("speedX", Math.Abs(rg2D.velocity.x));
        animator.SetBool("walk", _hr != 0.0f);
        animator.SetFloat("speedY", rg2D.velocity.y);

        _direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    }

    private void FixedUpdate()
    {
        if (!_isSticking && !_blockMove)
        {
            MovePlayer();
        }

        if (_wall && Math.Abs(_hr) > 0.0f && !_isSticking && _wallJump != _direction.x)
        {
            _wallJump = _direction.x;
            _isSticking = true;
            rg2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            animator.Play("wall");
        }
    }

    private void MovePlayer()
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

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(-.08f * _direction.x, 0f, 0f), Vector2.down, _groundRayDistCheck, _groundLayer);
        Debug.DrawRay(transform.position + new Vector3(-.08f * _direction.x, 0f, 0f), Vector2.down * _groundRayDistCheck, Color.red);

        _ground = hit.collider != null;
    }

    private void CheckWallJump()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0f, 0.08f, 0f), _direction, _wallRayDistCheck, _groundLayer);
        Debug.DrawRay(transform.position + new Vector3(0f, 0.08f, 0f), _direction * _wallRayDistCheck, Color.blue);

        _wall = hit.collider != null;
    }

    public void Flip(float x)
    {
        if (!_isSticking && !_blockMove)
            gameObject.transform.localScale = new Vector3(x, 1, 1);
    }

    IEnumerator JumpWall()
    {
        yield return new WaitForSeconds(_timeWallJump);
        Flip(-_direction.x);
        _isSticking = false;
    }

    private void Life()
    {
        _liveUI.text = " " + _live + "X";
    }

    private void Fruit()
    {
        _fruit++;
        _fruitUI.text = "X" + _fruit;
    }

    private void AnimationController(int x)
    {
        if (x == 0) animator.Play("Idle");
        if (x == 1) animator.Play("Hit");
        if (x == 2) animator.Play("Death");
    }

    private void CreditsScene()
    {
        _timer.destroyObject();
        SceneManager.LoadScene("credits");
    }

    IEnumerator ChangeScene(){
        _isSticking = true;
        yield return new WaitForSeconds (2);
        if(SceneManager.GetActiveScene().buildIndex < 4)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else{
            
            _timer.destroyObject();
            SceneManager.LoadScene("credits");
        }
    }


    private void RestartPosition() {
        transform.position = _position;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 normal = other.contacts[0].normal;

        if (other.gameObject.CompareTag("Enemy") && normal != Vector2.up)
        {
            _live--;
            Life();
            AnimationController(1);
            if (_live == 0)
            {
                _blockMove = true;
                _panel.SetActive(true);
                _timer.EndTimer();
                bx2D.enabled = false;
                rg2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                AnimationController(2);

            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Fruit"))
        {
            Fruit();
            Destroy(other.gameObject);

            if(_fruit == _fruitMax){
            StartCoroutine(ChangeScene());
            }
        }

        if (other.gameObject.CompareTag("Void"))
        {
            RestartPosition();
        }
    }
}
