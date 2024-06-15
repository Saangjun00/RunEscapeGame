using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navMeshAgent;  // ���̰��̼� ������Ʈ
    Transform player;

    public float MoveSpeed;
    private float gravity = 9.81f;

    public float detectionRange = 10f;      // ���� ����
    public float attackRange;               // ���� ����
    public float attackCooldown;            // ���� ��� �ð�
    private float lastAttackTime = 0;       // ������ ���� �ð�

    public Transform groundCheck;
    public LayerMask groundLayer;
    private BoxCollider boxCollider;
    private Rigidbody rb;
    private bool isGrounded;

     void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // ���� �߷� ������� ����
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        Vector3 boxCenter = transform.TransformPoint(boxCollider.center);
        Vector3 boxSize = boxCollider.size;

        // �� ����
        isGrounded = Physics.CheckBox(boxCenter, boxSize / 2, transform.rotation, groundLayer);

        // �÷��̾� ���� �� ���� ����
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

        // ȸ�� ���� ����
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * MoveSpeed);
        }

        // �̵�
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
