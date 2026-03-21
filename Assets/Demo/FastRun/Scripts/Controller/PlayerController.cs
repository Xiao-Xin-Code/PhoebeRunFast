using Frame;
using QMVC;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerController : BaseController
{
	protected override void Init()
	{
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
		rb.freezeRotation = true;

		currentSpeed = baseSpeed;

		Vector3 pos = transform.position;
		pos.x = lanes[currentLane];
		transform.position = pos;

		control = new InputController();
		control.Player.Enable();
		control.Player.Left.performed += callback=> { targetLane = Mathf.Max(0, currentLane - 1); };
		control.Player.Right.performed += callback=> { targetLane = Mathf.Min(2, currentLane + 1); };

		MonoService.Instance.AddUpdateListener(UpdateFunction);
		MonoService.Instance.AddFixedUpdateListener(FixedUpdateFunction);


		RoadSystem roadsystem = this.GetSystem<RoadSystem>();
		roadsystem.playerController = this;
	}

	InputController control;

	[SerializeField] GameObject commCollision;
	[SerializeField] GameObject halfCollision;


	#region 属性

	private Rigidbody rb;
	[Header("轨道配置")]
	public float[] lanes = { -2f, 0f, 2f };
	public float laneChangeSpeed = 15f;

	[Header("移动参数")]
	public float baseSpeed = 8f;
	public float maxSpeed = 20f;
	public float speedIncreaseRate = 0.1f;  // 每帧增加速度

	[Header("跳跃参数")]
	public float jumpForce = 10f;
	public float gravity = 25f;
	public float jumpBufferTime = 0.15f;     // 跳跃缓冲
	public float coyoteTime = 0.1f;          // 落地缓冲


	[Header("地面检测")]
	public Transform groundCheck;
	public float groundCheckRadius = 0.3f;
	public LayerMask groundLayer;

	private int currentLane = 1;
	private int targetLane = 1;

	[SerializeField] private bool isGrounded;
	private float lastGroundedTime;
	private float lastJumpPressedTime;
	private bool jumpRequested;

	private float currentSpeed;

	#endregion



	void UpdateFunction()
	{
		HandleInput();
	}


	void FixedUpdateFunction()
	{
		//移动
		MoveForward();

		SwitchLane();

		ApplyGravity();
	}




	

	void HandleInput()
	{
		Mouse mouse = Mouse.current;

		if (mouse.leftButton.wasPressedThisFrame)
		{
			startPos = mouse.position.ReadValue();
		}

		if (mouse.leftButton.isPressed)
		{
			isDrag = true;
		}

		if (mouse.leftButton.wasReleasedThisFrame)
		{
			if (isDrag)
			{
				Vector2 dur = mouse.position.ReadValue() - startPos;
				float angle = Vector2.Angle(Vector2.left, dur);
				Debug.Log(dur);
				if (angle >= 0 && angle <= 30)
				{
					if (dur.x < -minDis)
					{
						if (isTurn)
						{
							isTurn = false;
							LeftTurn(90);
						}
						else
						{
							Debug.Log("Left");
							targetLane = Mathf.Max(0, currentLane - 1);
						}
					}
				}
				else if (angle >= 60 && angle <= 120)
				{
					if (dur.y > 0 && dur.y > minDis)
					{
						Debug.Log("Up");
					}
					else if (dur.y < 0 && dur.y < -minDis)
					{
						Debug.Log("Down");
					}
				}
				else if (angle >= 150 && angle <= 180)
				{
					if (dur.x > minDis)
					{
						if (isTurn)
						{
							isTurn = false;
							RightTurn(90);
						}
						else
						{
							Debug.Log("Right");
							targetLane = Mathf.Min(2, currentLane + 1);
						}
					}
				}
			}
		}

		GroundCheck();
	}

	private void GroundCheck()
	{
		isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

		if (isGrounded)
		{
			lastGroundedTime = Time.time;
		}

		// 记录跳跃按键
		if (control.Player.Up.WasPressedThisFrame())
		{
			lastJumpPressedTime = Time.time;
			jumpRequested = true;
		}

		// 跳跃逻辑（带缓冲）
		bool canJump = (isGrounded || Time.time - lastGroundedTime <= coyoteTime);
		bool jumpBuffered = (Time.time - lastJumpPressedTime <= jumpBufferTime);

		if (jumpRequested && canJump && jumpBuffered)
		{
			Jump();
			jumpRequested = false;
		}

		// 加速
		if (currentSpeed < maxSpeed)
		{
			currentSpeed += speedIncreaseRate * Time.deltaTime;
			currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
		}


	}


	private void MoveForward()
	{
		Vector3 velocity = rb.velocity;
		velocity.z = currentSpeed;
		rb.velocity = velocity;
	}


	void SwitchLane()
	{
		if (currentLane != targetLane)
		{
			float targetX = lanes[targetLane];
			Vector3 pos = transform.position;
			pos.x = Mathf.MoveTowards(pos.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);
			transform.position = pos;

			if (Mathf.Abs(pos.x - targetX) < 0.01f)
			{
				currentLane = targetLane;
			}
		}
	}


	void Jump()
	{
		// 清除Y轴速度
		Vector3 velocity = rb.velocity;
		velocity.y = 0;
		rb.velocity = velocity;

		// 施加跳跃力
		rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

		// 重置缓冲时间
		lastJumpPressedTime = 0;

		Debug.Log($"跳跃！当前速度: {currentSpeed:F1}");
	}

	void ApplyGravity()
	{
		if (!isGrounded)
		{
			Vector3 velocity = rb.velocity;
			velocity.y -= gravity * Time.fixedDeltaTime;
			rb.velocity = velocity;
		}
		else if (rb.velocity.y < 0)
		{
			// 落地时重置Y轴速度
			Vector3 velocity = rb.velocity;
			velocity.y = 0;
			rb.velocity = velocity;
		}
	}


	public bool isTurn;

	bool isDrag = false;
	Vector2 startPos;
	float minDis = 100;

	private void LeftTurn(float angle)
	{
		transform.Rotate(new Vector3(0, -angle, 0));
	}

	private void RightTurn(float angle)
	{
		transform.Rotate(new Vector3(0, angle, 0));
	}




	void OnDrawGizmosSelected()
	{
		if (groundCheck != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
		}

		Gizmos.color = Color.yellow;
		for (int i = 0; i < lanes.Length; i++)
		{
			Vector3 lanePos = new Vector3(lanes[i], transform.position.y, transform.position.z + 2f);
			Gizmos.DrawWireCube(lanePos, new Vector3(0.8f, 0.1f, 1f));
		}
	}
}
