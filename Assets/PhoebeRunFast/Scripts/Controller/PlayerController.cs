using System.Diagnostics.CodeAnalysis;
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

    protected override void OnInit()
    {
        base.OnInit();
        _globalSystem = this.GetSystem<GlobalSystem>();
        _entity = new PlayerEntity();
    }

    //MoveForward
    private void MoveForward()
    {
        Vector3 velocity = _view.RB.velocity;
        velocity.z = 0;//currentSpeed;
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



}
