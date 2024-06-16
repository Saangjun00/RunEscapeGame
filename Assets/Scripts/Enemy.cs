using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Animator animator;

    private Transform player;
    private NavMeshAgent agent;

    public float MoveSpeed;
    public float chaseSpeed;
    private float gravity = 9.81f;

    public Transform groundCheck;
    public LayerMask groundLayer;
    private BoxCollider boxCollider;
    private Rigidbody rb;
    private bool isGrounded;

    public float chaseDistance = 20f;       // 몬스터가 플레이어를 추적할 거리
    private Vector3 startPos;               // 몬스터의 초기 위치
    public float stopDistance = 4f;         // 몬스터가 플레이어를 추적할 최소 거리

    // 원래 박스 콜라이더 위치와 크기
    private Vector3 originalColliderCenter;
    private Vector3 originalColliderSize;

    // 공격 시 콜라이더 위치와 크기
    private Vector3 attackColliderCenter = new Vector3(0.006066442f, 0.3665345f, 1f);
    private Vector3 attackColliderSize = new Vector3(2.201339f, 1f, 4.433465f);

    // 몬스터가 정찰할 위치 배열
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

     void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // 기존 중력 사용하지 않음
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();

        // 원래 박스 콜라이더의 위치와 크기 저장
        originalColliderCenter = boxCollider.center;
        originalColliderSize = boxCollider.size;

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = transform.position;

        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    void Update()
    {
        Vector3 boxCenter = transform.TransformPoint(boxCollider.center);
        Vector3 boxSize = boxCollider.size;

        // 땅 감지
        isGrounded = Physics.CheckBox(boxCenter, boxSize / 2, transform.rotation, groundLayer);

        // 플레이어와 거리 계산
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // 플레이어가 추적거리 안에 있을 경우
        if (distanceToPlayer <= chaseDistance)
        {
            // 플레이어 추적
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            // 정찰 지점으로 이동
            Patrol();
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

    void ChasePlayer(float distanceToPlayer)
    {
        animator.SetBool("is_Run", true);
        animator.SetBool("is_Walk", false);
        animator.SetBool("is_Attack", false);

        agent.speed = chaseSpeed;

        // 플레이어가 너무 가까이 있을 경우
        if (distanceToPlayer <= stopDistance)
        {
            agent.isStopped = true;
            animator.SetBool("is_Attack", true);
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    void Patrol()
    {
        animator.SetBool("is_Walk", true);
        animator.SetBool("is_Run", false);
        animator.SetBool("is_Attack", false);

        agent.speed = MoveSpeed;

        // 현재 정찰 지점에 도달했는지 확인
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // 다음 정찰 지점으로 이동
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    public void EnableAttackCollider()
    {
        boxCollider.center = attackColliderCenter;
        boxCollider.size = attackColliderSize;
    }

    public void DisableAttackCollider()
    {
        boxCollider.center = originalColliderCenter;
        boxCollider.size = originalColliderSize;
    }
}
