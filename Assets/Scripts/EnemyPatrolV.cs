using System.IO.Pipes;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolV : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
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
    }

    void Start()
    {
        _patrolPointOne = transform.position.y - _patrolPointOne;
        _patrolPointTwo = transform.position.y + _patrolPointTwo;
    }


    void FixedUpdate()
    {

        if (transform.position.y > _patrolPointOne && _currentPointIndex == 0)
        {
            rb.velocity = new Vector2(0, -_moveSpeed);
        }

        if (transform.position.y < _patrolPointTwo && _currentPointIndex == 1)
        {
            rb.velocity = new Vector2(0, _moveSpeed);
        }
        if (transform.position.y < _patrolPointOne)
        {
            _currentPointIndex = 1;
        }
        else if (transform.position.y > _patrolPointTwo)
        {
            _currentPointIndex = 0;
        }
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
