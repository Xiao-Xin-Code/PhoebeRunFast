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
    float jumpSpeed = 7;
    float gravity = 25;
    float maxFallSpeed = 20;

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

        MonoService.Instance.AddFixedUpdateListener(OnFixedFixedUpdate);


        //绑定Inputs触发
        _globalSystem.Inputs.Player.Left.performed += OnLeftPressed;
        _globalSystem.Inputs.Player.Right.performed += OnRightPressed;
        _globalSystem.Inputs.Player.Jump.performed += OnJumpPressed;
        _globalSystem.Inputs.Player.Slow.performed += OnSlowPressed;
    }


    private void OnFixedFixedUpdate()
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
                _view.RB.velocity = Vector3.zero;
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
            Vector3 velocity = _view.RB.velocity;
            velocity.y = jumpSpeed;
            _view.RB.velocity = velocity;
            isJumping = true;
            _isGrounded = false;
        }
    }

    private void OnSlowPressed(InputAction.CallbackContext context)
    {
        Slow();
    }





    //MoveForward
    private void MoveForward()
    {
        Vector3 velocity = _view.RB.velocity;
        velocity.z = forwardSpeed;
        _view.RB.velocity = velocity;
    }

    //SwitchLine
    private void SwitchLine()
    {
        float targetX = _globalSystem.GameSingleton.GetLine(currentLine).position.x;
        Vector3 currentPosition = transform.position;
		currentPosition.x = Mathf.Lerp(currentPosition.x, targetX, sideSpeed * Time.fixedDeltaTime);
        _view.RB.MovePosition(currentPosition);
    }

    //Slow
    private void Slow()
    {
        _roleController.Roll();
        Debug.Log("Slow");
    }


    private void ApplyGravity()
    {
        Vector3 velocity = _view.RB.velocity;
        if (!_isGrounded)
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
            velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);
        }
        else if (velocity.y < 0) 
        {
            velocity.y = 0;
        }
		_view.RB.velocity = velocity;
	}



    private void GroundCheck()
    {
        bool wasGrounded = _isGrounded;
		_isGrounded = Physics.CheckSphere(_view.GroundCheck.position, 0.1f, _view.GroundLayer);
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

}
