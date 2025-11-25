using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;
    Transform target;
    private Vector2 moveDir;
    private Knockback knockback;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDir = direction;
        }
    }

    private void FixedUpdate()
    {
        if (knockback.GettingKnockedBack) { return; }
        if (target != null)
        {
            //rb.velocity = new Vector2 (moveDir.x, moveDir.y * moveSpeed).normalized;
            rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
        }
    }

    // public void MoveTo(Vector2 targetPosition)
    // {
    //     moveDir = targetPosition;
    // }
}