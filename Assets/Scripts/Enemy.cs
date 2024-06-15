using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navMeshAgent;  // 네이게이션 에이전트
    Transform player;

    public float MoveSpeed;
    private float gravity = 9.81f;

    public float detectionRange = 10f;      // 추적 범위
    public float attackRange;               // 공격 범위
    public float attackCooldown;            // 공격 대기 시간
    private float lastAttackTime = 0;       // 마지막 공격 시간

    public Transform groundCheck;
    public LayerMask groundLayer;
    private BoxCollider boxCollider;
    private Rigidbody rb;
    private bool isGrounded;

     void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // 기존 중력 사용하지 않음
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        Vector3 boxCenter = transform.TransformPoint(boxCollider.center);
        Vector3 boxSize = boxCollider.size;

        // 땅 감지
        isGrounded = Physics.CheckBox(boxCenter, boxSize / 2, transform.rotation, groundLayer);

        // 플레이어 추적 및 공격 로직
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
            else if (distanceToPlayer <= detectionRange)
            {
                ChasePlayer();
            }
            else
            {
                animator.SetBool("is_Move", false);
            }
        }
    }

    void FixedUpdate()
    {
        ApplyGravity();
        
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * gravity * rb.mass);
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        // 회전 방향 설정
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * MoveSpeed);
        }

        // 이동
        Vector3 movement = direction * MoveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        animator.SetBool("is_Move", true);
    }

    void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }

        animator.SetBool("is_Move", false);
    }
}
