using Frame;
using QMVC;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 玩家控制器
/// </summary>
public class PlayerController : BaseController
{
    [SerializeField] PlayerView _view;
    [SerializeField] RoleController _roleController;

    GlobalSystem _globalSystem;


    PlayerEntity _entity;


    float forwardSpeed = 5;
    float sideSpeed = 10;
    /// <summary>
    /// 跳跃速度
    /// </summary>
    float jumpSpeed = 10;
    /// <summary>
    /// 重力加速度
    /// </summary>
    float gravity = -30;
    /// <summary>
    /// 最大下落速度
    /// </summary>
    float maxFallSpeed = 45;
    /// <summary>
    /// 当前垂直速度
    /// </summary>
    float verticalSpeed = 0;

    int currentLine = 1;
    bool _isGrounded = true;
    bool isJumping = false;


    protected override void OnInit()
    {
        base.OnInit();
        _globalSystem = this.GetSystem<GlobalSystem>();
        _entity = new PlayerEntity();

        Debug.Log("注册");
        this.RegisterEvent<SetPlayerRoleEvent>(OnSetPlayerRole);

        MonoService.Instance.AddFixedUpdateListener(OnFixedUpdate);


        //绑定Inputs触发
        _globalSystem.Inputs.Player.Left.performed += OnLeftPressed;
        _globalSystem.Inputs.Player.Right.performed += OnRightPressed;
        _globalSystem.Inputs.Player.Jump.performed += OnJumpPressed;
        _globalSystem.Inputs.Player.Slow.performed += OnSlowPressed;
    }


    private void OnFixedUpdate()
    {
        switch (_globalSystem.GameSingleton.GameEntity.GameState.Value)
        {
            case GameState.Ready:
                break;
            case GameState.Running:
                GroundCheck();
                MoveForward();
                SwitchLine();
                ApplyGravity();
                break;
            case GameState.Paused:
                
				break;
            case GameState.Over:
                break;
            default:
                break;
        }
    }

    private void OnLeftPressed(InputAction.CallbackContext context)
    {
        if (currentLine > 0) currentLine--;
    }

    private void OnRightPressed(InputAction.CallbackContext context)
    {
        if (currentLine < _globalSystem.GameSingleton.GetLines().Length - 1) currentLine++;
    }

    private void OnJumpPressed(InputAction.CallbackContext context)
    {
        if (_isGrounded && !isJumping)  
        {
            verticalSpeed = jumpSpeed;
            isJumping = true;
            _isGrounded = false;

            //即刻应用效果
            Vector3 pos = transform.position;
            pos.y += verticalSpeed * Time.fixedDeltaTime;
            transform.position = pos;
        }
    }

    private void OnSlowPressed(InputAction.CallbackContext context)
    {
        Slow();
    }


    //MoveForward
    private void MoveForward()  
    {
        Vector3 pos = transform.position;
        pos.z += forwardSpeed * Time.fixedDeltaTime;
        transform.position = pos;
    }

    //SwitchLine
    private void SwitchLine()
    {
        float targetX = _globalSystem.GameSingleton.GetLine(currentLine).position.x;
        Vector3 currentPosition = transform.position;
		currentPosition.x = Mathf.Lerp(currentPosition.x, targetX, sideSpeed * Time.fixedDeltaTime);
        transform.position = currentPosition;
    }

    //Slow
    private void Slow()
    {
        _roleController.Roll();
        Debug.Log("Slow");
    }

    private void ApplyGravity()
    {
        if (_isGrounded) return;

        verticalSpeed += gravity * Time.fixedDeltaTime;
        verticalSpeed = Mathf.Max(verticalSpeed, -maxFallSpeed);
        Vector3 pos = transform.position;
        pos.y += verticalSpeed * Time.fixedDeltaTime;
        transform.position = pos;
    }

    private void GroundCheck()
    {
        bool wasGrounded = _isGrounded;
		_isGrounded = Physics.CheckSphere(_view.GroundCheck.position, 0.05f, _view.GroundLayer);
        if (_isGrounded && !wasGrounded)
        {
            isJumping = false;
        }
	}



    private void OnSetPlayerRole(SetPlayerRoleEvent evt)
    {
        _roleController = evt.role;
		_roleController.transform.SetParent(transform);
		_roleController.transform.localPosition = Vector3.zero;
        _roleController.transform.eulerAngles = new Vector3(0, 180, 0); 
    }


	protected override void OnDeInit()
	{
		base.OnDeInit();
		this.UnRegisterEvent<SetPlayerRoleEvent>(OnSetPlayerRole);
		MonoService.Instance.RemoveFixedUpdateListener(OnFixedUpdate);

         _globalSystem.Inputs.Player.Left.performed -= OnLeftPressed;
        _globalSystem.Inputs.Player.Right.performed -= OnRightPressed;
        _globalSystem.Inputs.Player.Jump.performed -= OnJumpPressed;
        _globalSystem.Inputs.Player.Slow.performed -= OnSlowPressed;
	}
}
