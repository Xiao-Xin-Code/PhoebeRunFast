using Frame;
using QMVC;
using UnityEngine;

/// <summary>
/// 玩家控制器
/// </summary>
public class PlayerController : BaseController
{
    [SerializeField] PlayerView _view;
    [SerializeField] RoleController _roleController;

    GlobalSystem _globalSystem;


    PlayerEntity _entity;


    float currentSpeed = 5;



    protected override void OnInit()
    {
        base.OnInit();
        _globalSystem = this.GetSystem<GlobalSystem>();
        _entity = new PlayerEntity();

        Debug.Log("注册");
        this.RegisterEvent<SetPlayerRoleEvent>(OnSetPlayerRole);

        MonoService.Instance.AddFixedUpdateListener(OnFixedFixedUpdate);


        //绑定Inputs触发
        _globalSystem.Inputs.Player.Left.performed += callback => { Debug.Log("Left"); };
        _globalSystem.Inputs.Player.Right.performed += callback => { Debug.Log("Right"); };
        _globalSystem.Inputs.Player.Jump.performed += callback => { Debug.Log("Jump"); };
        _globalSystem.Inputs.Player.Slow.performed += callback => { Debug.Log("Slow"); };
    }


    private void OnFixedFixedUpdate()
    {
        switch (_globalSystem.GameSingleton.GameEntity.GameState.Value)
        {
            case GameState.Ready:
                break;
            case GameState.Running:
                MoveForward();
                break;
            case GameState.Paused:
				Vector3 velocity = _view.RB.velocity;
				velocity.z = 0;
				_view.RB.velocity = velocity;
				break;
            case GameState.Over:
                break;
            default:
                break;
        }
    }




    //MoveForward
    private void MoveForward()
    {
        Vector3 velocity = _view.RB.velocity;
        velocity.z = currentSpeed;
        _view.RB.velocity = velocity;
    }

    //SwitchLine
    private void SwitchLine(int targetLane)
    {
        float targetX = _globalSystem.GameSingleton.GetLine(targetLane).position.x;

        //如果当前Line不相同，执行运行动作
        //移动到指定位置，需要平滑移动
        Vector3 targetPosition = new Vector3(targetX, _view.transform.position.y, _view.transform.position.z);
        _view.transform.position = Vector3.Lerp(_view.transform.position, targetPosition, 0.1f);
    }
    
    //Jump
    private void Jump()
    {
        
    }

    //Slow
    private void Slow()
    {
        
    }





    private void OnSetPlayerRole(SetPlayerRoleEvent evt)
    {
        _roleController = evt.role;
		_roleController.transform.SetParent(transform);
		_roleController.transform.localPosition = Vector3.zero;
        _roleController.transform.eulerAngles = new Vector3(0, 180, 0); 
    }

}
