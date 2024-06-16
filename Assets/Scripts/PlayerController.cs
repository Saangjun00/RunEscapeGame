using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    public float maxMoveSpeed = 30f;
    public float dashSpeed = 10f;
    public float rotationSpeed = 10f;
    public float jumpForce = 10f;
    public float doubleJumpForce = 8f;
    public float gravity = 9.81f; // ����ڰ� ������ �߷��� �⺻��
    public int itemCnt;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool isGrounded;
    private int jumpCount;
    private bool canDash;

    private float currentSpeed = 0f;
    public float accleration = 4f;

    public float minHeight = -10f;

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // ���� �߷� ������� ����
        animator = GetComponent<Animator>();
        canDash = true;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // �� ����
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        // ĳ���Ͱ� ���� �ִ��� Ȯ��
        if (isGrounded)
        {
            //Debug.Log("���� �ִ� ����");
            canDash = true;
        }

        if (isGrounded && rb.velocity.y <= 0)
        {
            jumpCount = 0; // ���� �� ���� ī��Ʈ �ʱ�ȭ
            animator.SetBool("is_Jump", false);
            animator.SetBool("is_Fall", false);
            animator.SetBool("is_Dash", false);
        }
        else if (!isGrounded && rb.velocity.y < 0 && !animator.GetBool("is_Fall"))
        {
            animator.SetBool("is_Fall", true); // �������� ���·� ��ȯ
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }

        animator.SetBool("is_Walk", moveDirection != Vector3.zero);
        animator.SetFloat("Speed", currentSpeed / maxMoveSpeed);

        // RŰ ������ �� �� ��ε�
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }

        // Ű�е� ���ڷ� �� ��ȯ
        // Ű�е� ���� �Է����� �� ����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadScene(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadScene(3);
        }

        // ���� ���� ���Ϸ� �������� ĳ���� ���
        if (transform.position.y < minHeight)
        {
            if (!isDead)
            {
                isDead = true;
                animator.SetBool("is_Hurt", true);
                Invoke("Die", 1f);
            }
        }
    }

    void FixedUpdate()
    {
        ApplyGravity(); // �߷� ����
        Move();
    }

    void Move()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            Quaternion newRotation = Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            rb.MoveRotation(newRotation);

            // ����
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxMoveSpeed, accleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = 0f;
        }

        Vector3 movement = moveDirection * currentSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        animator.SetFloat("Speed", currentSpeed / maxMoveSpeed);
    }

    void Jump()
    {
        if (isGrounded || jumpCount < 2)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * (isGrounded ? jumpForce : doubleJumpForce), ForceMode.Impulse);
            animator.SetBool("is_Jump", true);
            animator.SetBool("is_Fall", false);
            jumpCount++;
            Debug.Log($"����: {jumpCount}, isGrounded: {isGrounded}");
            isGrounded = false; // ���� �� isGrounded�� false�� ����
        }
    }

    void Dash()
    {
        if (!isGrounded && canDash)
        {
            Vector3 dashDirection = transform.forward;
            rb.velocity = dashDirection * dashSpeed;

            animator.SetBool("is_Dash", true);
            canDash = false;
        }
    }

    void ApplyGravity()
    {
        rb.AddForce(Vector3.down * gravity * rb.mass); // �߷� ����
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!isDead)
            {
                isDead = true;
                animator.SetBool("is_Hurt", true);
                Invoke("Die", 1f);
            }
        }
    }

    void Die()
    {
        animator.SetBool("is_Hurt", false);
        animator.SetBool("is_Dead", true);
        Invoke("ReloadScene", 1.5f);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
}