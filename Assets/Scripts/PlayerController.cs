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
    public float gravity = 9.81f; // 사용자가 설정할 중력의 기본값
    public int itemCnt;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody rb;
    public Vector3 moveDirection;
    private bool isGrounded;
    private int jumpCount;
    private bool canDash;

    private float currentSpeed = 0f;
    public float accleration = 4f;

    public float minHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // 기존 중력 사용하지 않음
        animator = GetComponent<Animator>();
        canDash = true;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // 땅 감지
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        // 캐릭터가 땅에 있는지 확인
        if (isGrounded)
        {
            //Debug.Log("땅에 있는 상태");
            canDash = true;
        }

        if (isGrounded && rb.velocity.y <= 0)
        {
            jumpCount = 0; // 착지 시 점프 카운트 초기화
            animator.SetBool("is_Jump", false);
            animator.SetBool("is_Fall", false);
            animator.SetBool("is_Dash", false);
        }
        else if (!isGrounded && rb.velocity.y < 0 && !animator.GetBool("is_Fall"))
        {
            animator.SetBool("is_Fall", true); // 떨어지는 상태로 전환
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


        // 낭떠러지로 플레이어가 떨어지는지 체크
        //ChangeSceneFallDown();
    }

    void FixedUpdate()
    {
        ApplyGravity(); // 중력 적용
        Move();
    }

    void Move()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            Quaternion newRotation = Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            rb.MoveRotation(newRotation);

            // 가속
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
            Debug.Log($"점프: {jumpCount}, isGrounded: {isGrounded}");
            isGrounded = false; // 점프 시 isGrounded를 false로 설정
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
        rb.AddForce(Vector3.down * gravity * rb.mass); // 중력 적용
    }

    /*
    void ChangeSceneFallDown()
    {
        if (transform.position.y > minHeight)
        {
            // 현재 씬을 로드 (현재 씬 재시작)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    */
}