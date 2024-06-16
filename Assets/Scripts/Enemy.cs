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

    public float chaseDistance = 20f;       // ���Ͱ� �÷��̾ ������ �Ÿ�
    private Vector3 startPos;               // ������ �ʱ� ��ġ
    public float stopDistance = 4f;         // ���Ͱ� �÷��̾ ������ �ּ� �Ÿ�

    // ���� �ڽ� �ݶ��̴� ��ġ�� ũ��
    private Vector3 originalColliderCenter;
    private Vector3 originalColliderSize;

    // ���� �� �ݶ��̴� ��ġ�� ũ��
    private Vector3 attackColliderCenter = new Vector3(0.006066442f, 0.3665345f, 1f);
    private Vector3 attackColliderSize = new Vector3(2.201339f, 1f, 4.433465f);

    // ���Ͱ� ������ ��ġ �迭
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

     void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // ���� �߷� ������� ����
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();

        // ���� �ڽ� �ݶ��̴��� ��ġ�� ũ�� ����
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

        // �� ����
        isGrounded = Physics.CheckBox(boxCenter, boxSize / 2, transform.rotation, groundLayer);

        // �÷��̾�� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // �÷��̾ �����Ÿ� �ȿ� ���� ���
        if (distanceToPlayer <= chaseDistance)
        {
            // �÷��̾� ����
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            // ���� �������� �̵�
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

        // �÷��̾ �ʹ� ������ ���� ���
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

        // ���� ���� ������ �����ߴ��� Ȯ��
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // ���� ���� �������� �̵�
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
