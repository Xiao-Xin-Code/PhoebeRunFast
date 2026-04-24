using UnityEngine;
using QMVC;

public abstract class RoleController : BaseController
{
    [SerializeField] RoleView _view;

    RoleEntity _entity;

    public RoleData RoleData =>_entity.roleData;

    /// <summary>
    /// 初始化方法
    /// </summary>
    protected override void OnInit()
    {
        base.OnInit();
        _entity = new RoleEntity();
    }

    public abstract void MoveForward();
    public abstract void MoveToLeft();
    public abstract void MoveToRight();
    public abstract void Jump();
    public abstract void Roll();


    /// <summary>
    /// 作用天赋，表示天赋可使用时作用于角色的状态变化，通常是初始化时作用
    /// </summary>
    /// <param name="star">星级</param>
    protected abstract void ApplyTalent(int star);

    /// <summary>
    /// 激活天赋,表示触发那个天赋
    /// </summary>
    /// <param name="star">星级</param>
    protected abstract void ActiveTalent(int star);


}