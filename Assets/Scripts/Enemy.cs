using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    Animator animator;

    public Transform player;
    NavMeshAgent agent;

    public float MoveSpeed;
    private float gravity = 9.81f;

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

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        agent.destination = player.transform.position;
    }

    void Update()
    {
        Vector3 boxCenter = transform.TransformPoint(boxCollider.center);
        Vector3 boxSize = boxCollider.size;

        // 땅 감지
        isGrounded = Physics.CheckBox(boxCenter, boxSize / 2, transform.rotation, groundLayer);

        
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
}
