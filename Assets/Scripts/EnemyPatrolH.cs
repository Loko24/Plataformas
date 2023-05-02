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
    
    private int currentPointIndex = 0;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

        if (transform.position.x > _patrolPointOne && currentPointIndex == 0)
        {
            rb.velocity = new Vector2(-_moveSpeed, rb.velocity.y);
        }

        if (transform.position.x < _patrolPointTwo && currentPointIndex == 1)
        {
            rb.velocity = new Vector2(_moveSpeed, rb.velocity.y);
        }
        if (transform.position.x < _patrolPointOne)
        {
            currentPointIndex = 1;
        }
        else if (transform.position.x > _patrolPointTwo)
        {
            currentPointIndex = 0;
        }
    }

    public void flip(float x)
    {
            gameObject.transform.localScale = new Vector3(x, 1, 1);
    }
}
