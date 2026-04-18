using Frame;
using QMVC;
using UnityEngine;

/// <summary>
/// 玩家控制器
/// </summary>
public class PlayerController : BaseController
{
    [SerializeField] PlayerView _view;


    #region 轨道配置
    [Header("轨道配置")]
    public float[] lanes = { -2f, 0f, 2f };
    public float laneChangeSpeed = 15f;
    #endregion

    #region 移动参数
    [Header("移动参数")]
    public float baseSpeed = 8f;
    public float maxSpeed = 20f;
    public float speedIncreaseRate = 0.1f;
    #endregion

    #region 跳跃参数
    [Header("跳跃参数")]
    public float jumpForce = 10f;
    public float gravity = 25f;
    public float jumpBufferTime = 0.15f;
    public float coyoteTime = 0.1f;
    public float groundCheckRadius = 0.3f;
    #endregion

    private int _currentLane = 1;
    private int _targetLane = 1;
    private float _currentSpeed;
    private bool _isGrounded;
    private float _lastGroundedTime;
    private float _lastJumpPressedTime;
    private bool _jumpRequested;
    private bool _isDrag;
    private Vector3 _startPos;
    private float _minDis = 100;
    public bool isTurn;

    /// <summary>
    /// 初始化方法
    /// </summary>
    protected override void OnInit()
    {
        base.OnInit();

        _view.RB.useGravity = false;
        _view.RB.freezeRotation = true;

        _currentSpeed = baseSpeed;

        // 初始化位置到中间轨道
        Vector3 pos = transform.position;
        pos.x = lanes[_currentLane];
        transform.position = pos;

        // 注册更新方法
        MonoService.Instance.AddUpdateListener(UpdateFunction);
        MonoService.Instance.AddFixedUpdateListener(FixedUpdateFunction);
    }

    /// <summary>
    /// 更新方法
    /// </summary>
    private void UpdateFunction()
    {
        HandleInput();
    }

    /// <summary>
    /// 固定更新方法
    /// </summary>
    private void FixedUpdateFunction()
    {
        MoveForward();
        SwitchLane();
        ApplyGravity();
    }

    /// <summary>
    /// 处理输入
    /// </summary>
    private void HandleInput()
    {
        // 键盘输入
        if (Input.GetKeyDown(KeyCode.A))
        {
            _targetLane = Mathf.Max(0, _currentLane - 1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _targetLane = Mathf.Min(2, _currentLane + 1);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _lastJumpPressedTime = Time.time;
            _jumpRequested = true;
        }

        // 鼠标拖拽输入
        if (Input.GetMouseButtonDown(0))
        {
            _startPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            _isDrag = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_isDrag)
            {
                Vector3 dur = Input.mousePosition - _startPos;
                float angle = Vector2.Angle(Vector2.left, new Vector2(dur.x, dur.y));

                if (angle >= 0 && angle <= 30)
                {
                    if (dur.x < -_minDis)
                    {
                        if (isTurn)
                        {
                            isTurn = false;
                            LeftTurn(90);
                        }
                        else
                        {
                            _targetLane = Mathf.Max(0, _currentLane - 1);
                        }
                    }
                }
                else if (angle >= 60 && angle <= 120)
                {
                    if (dur.y > 0 && dur.y > _minDis)
                    {
                        // 跳跃
                        _lastJumpPressedTime = Time.time;
                        _jumpRequested = true;
                    }
                    else if (dur.y < 0 && dur.y < -_minDis)
                    {
                        // 下蹲
                    }
                }
                else if (angle >= 150 && angle <= 180)
                {
                    if (dur.x > _minDis)
                    {
                        if (isTurn)
                        {
                            isTurn = false;
                            RightTurn(90);
                        }
                        else
                        {
                            _targetLane = Mathf.Min(2, _currentLane + 1);
                        }
                    }
                }
            }
            _isDrag = false;
        }

        GroundCheck();
    }

    /// <summary>
    /// 地面检测
    /// </summary>
    private void GroundCheck()
    {
        _isGrounded = Physics.CheckSphere(_view.GroundCheck.position, groundCheckRadius, _view.GroundLayer);

        if (_isGrounded)
        {
            _lastGroundedTime = Time.time;
        }

        // 跳跃逻辑（带缓冲）
        bool canJump = (_isGrounded || Time.time - _lastGroundedTime <= coyoteTime);
        bool jumpBuffered = (Time.time - _lastJumpPressedTime <= jumpBufferTime);

        if (_jumpRequested && canJump && jumpBuffered)
        {
            Jump();
            _jumpRequested = false;
        }

        // 加速
        if (_currentSpeed < maxSpeed)
        {
            _currentSpeed += speedIncreaseRate * Time.deltaTime;
            _currentSpeed = Mathf.Min(_currentSpeed, maxSpeed);
        }
    }

    /// <summary>
    /// 向前移动
    /// </summary>
    private void MoveForward()
    {
        Vector3 velocity = _view.RB.velocity;
        velocity.z = _currentSpeed;
        _view.RB.velocity = velocity;
    }

    /// <summary>
    /// 切换轨道
    /// </summary>
    private void SwitchLane()
    {
        if (_currentLane != _targetLane)
        {
            float targetX = lanes[_targetLane];
            Vector3 pos = transform.position;
            pos.x = Mathf.MoveTowards(pos.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);
            transform.position = pos;

            if (Mathf.Abs(pos.x - targetX) < 0.01f)
            {
                _currentLane = _targetLane;
            }
        }
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    private void Jump()
    {
        // 清除Y轴速度
        Vector3 velocity = _view.RB.velocity;
        velocity.y = 0;
        _view.RB.velocity = velocity;

        // 施加跳跃力
        _view.RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // 重置缓冲时间
        _lastJumpPressedTime = 0;
    }

    /// <summary>
    /// 应用重力
    /// </summary>
    private void ApplyGravity()
    {
        if (!_isGrounded)
        {
            Vector3 velocity = _view.RB.velocity;
            velocity.y -= gravity * Time.fixedDeltaTime;
            _view.RB.velocity = velocity;
        }
        else if (_view.RB.velocity.y < 0)
        {
            // 落地时重置Y轴速度
            Vector3 velocity = _view.RB.velocity;
            velocity.y = 0;
            _view.RB.velocity = velocity;
        }
    }

    /// <summary>
    /// 左转
    /// </summary>
    /// <param name="angle">旋转角度</param>
    private void LeftTurn(float angle)
    {
        transform.Rotate(new Vector3(0, -angle, 0));
    }

    /// <summary>
    /// 右转
    /// </summary>
    /// <param name="angle">旋转角度</param>
    private void RightTurn(float angle)
    {
        transform.Rotate(new Vector3(0, angle, 0));
    }

    /// <summary>
    /// 绘制辅助线
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (_view.GroundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_view.GroundCheck.position, groundCheckRadius);
        }

        Gizmos.color = Color.yellow;
        for (int i = 0; i < lanes.Length; i++)
        {
            Vector3 lanePos = new Vector3(lanes[i], transform.position.y, transform.position.z + 2f);
            Gizmos.DrawWireCube(lanePos, new Vector3(0.8f, 0.1f, 1f));
        }
    }

    /// <summary>
    /// 反初始化方法
    /// </summary>
    protected override void OnDeInit()
    {
        base.OnDeInit();

        MonoService.Instance.RemoveUpdateListener(UpdateFunction);
        MonoService.Instance.RemoveFixedUpdateListener(FixedUpdateFunction);
    }

    /// <summary>
    /// 获取当前速度
    /// </summary>
    public float CurrentSpeed => _currentSpeed;

    /// <summary>
    /// 获取当前轨道
    /// </summary>
    public int CurrentLane => _currentLane;
}
