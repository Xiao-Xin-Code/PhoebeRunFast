using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StandalonePlayerController : MonoBehaviour
{
    [Header("移动设置")]
    [Tooltip("向前移动速度")]
    public float forwardSpeed = 5f;
    
    [Tooltip("左右切换速度")]
    public float sideSpeed = 10f;
    
    [Tooltip("三个线路的X坐标位置")]
    public float[] lanePositions = new float[] { -2f, 0f, 2f };

    [Header("跳跃设置")]
    [Tooltip("跳跃速度")]
    public float jumpSpeed = 7f;
    
    [Tooltip("重力加速度")]
    public float gravity = 25f;
    
    [Tooltip("最大下落速度")]
    public float maxFallSpeed = 20f;

    [Header("地面检测")]
    [Tooltip("地面检测点")]
    public Transform groundCheck;
    
    [Tooltip("地面检测半径")]
    public float groundCheckRadius = 0.3f;
    
    [Tooltip("地面层")]
    public LayerMask groundLayer;

    private Rigidbody rb;
    private int currentLane = 1;
    private bool isGrounded = true;
    private bool isJumping = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
        if (lanePositions.Length != 3)
        {
            Debug.LogError("必须设置3个线路位置！");
            lanePositions = new float[] { -2f, 0f, 2f };
        }
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        MoveForward();
        SwitchLane();
        ApplyGravity();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        
        if (isGrounded && !wasGrounded)
        {
            isJumping = false;
        }
    }

    private void MoveForward()
    {
        Vector3 velocity = rb.velocity;
        velocity.z = forwardSpeed;
        rb.velocity = velocity;
    }

    private void MoveLeft()
    {
        if (currentLane > 0)
        {
            currentLane--;
        }
    }

    private void MoveRight()
    {
        if (currentLane < lanePositions.Length - 1)
        {
            currentLane++;
        }
    }

    private void SwitchLane()
    {
        float targetX = lanePositions[currentLane];
        Vector3 currentPosition = rb.position;
        currentPosition.x = Mathf.Lerp(currentPosition.x, targetX, sideSpeed * Time.fixedDeltaTime);
        rb.MovePosition(currentPosition);
    }

    private void Jump()
    {
        if (isGrounded && !isJumping)
        {
            Vector3 velocity = rb.velocity;
            velocity.y = jumpSpeed;
            rb.velocity = velocity;
            isJumping = true;
            isGrounded = false;
        }
    }

    private void ApplyGravity()
    {
        Vector3 velocity = rb.velocity;
        
        if (!isGrounded)
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
            velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);
        }
        else if (velocity.y < 0)
        {
            velocity.y = 0;
        }
        
        rb.velocity = velocity;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        
        Gizmos.color = Color.blue;
        for (int i = 0; i < lanePositions.Length; i++)
        {
            Gizmos.DrawLine(new Vector3(lanePositions[i], 0.5f, transform.position.z - 5f),
                           new Vector3(lanePositions[i], 0.5f, transform.position.z + 5f));
        }
    }
}