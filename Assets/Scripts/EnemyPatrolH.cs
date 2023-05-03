using System.IO.Pipes;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolH : MonoBehaviour
{
    private float _moveSpeed = 2f;
    [SerializeField]
    private float _patrolPointOne;
    [SerializeField]
    private float _patrolPointTwo;
    private float _hr;
    
    private int _currentPointIndex = 0;

    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        _patrolPointOne = transform.position.x - _patrolPointOne;
        _patrolPointTwo = transform.position.x + _patrolPointTwo;
    }

    private void Update()
    {
        _hr = rb.velocity.x;

        if (_hr < 0.0f) flip(-1f);
        else if (_hr > 0.0f) flip(1f);
    }

    void FixedUpdate()
    {

        if (transform.position.x > _patrolPointOne && _currentPointIndex == 0)
        {
            rb.velocity = new Vector2(-_moveSpeed, rb.velocity.y);
        }

        if (transform.position.x < _patrolPointTwo && _currentPointIndex == 1)
        {
            rb.velocity = new Vector2(_moveSpeed, rb.velocity.y);
        }
        if (transform.position.x < _patrolPointOne)
        {
            _currentPointIndex = 1;
        }
        else if (transform.position.x > _patrolPointTwo)
        {
            _currentPointIndex = 0;
        }
    }

    public void flip(float x)
    {
            gameObject.transform.localScale = new Vector3(x, 1, 1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;

        if (normal == Vector2.down && collision.gameObject.CompareTag("Player"))
        {
            death();
        }
    }

    void death(){
        Destroy(gameObject);
    }
}
