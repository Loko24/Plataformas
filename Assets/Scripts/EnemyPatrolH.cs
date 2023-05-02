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

    [SerializeField]
    private int currentPointIndex = 0;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _patrolPointOne = transform.position.x - _patrolPointOne;
        _patrolPointTwo = transform.position.x - _patrolPointOne;

    }

    private void Update()
    {
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
}
