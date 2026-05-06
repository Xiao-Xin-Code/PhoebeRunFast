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
        Debug.Log("设置：" + evt.role);
        _roleController = evt.role;
		_roleController.transform.SetParent(transform);
		_roleController.transform.localPosition = Vector3.zero;
        _roleController.transform.eulerAngles = new Vector3(0, 180, 0); 
    }

}
