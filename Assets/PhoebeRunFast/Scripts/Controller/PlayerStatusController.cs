using QMVC;
using UnityEngine;

/// <summary>
/// 玩家状态控制器
/// </summary>
public class PlayerStatusController : BaseController
{
    [SerializeField] PlayerStatusView _view;

    PlayerStatusEntity _entity;

    GlobalSystem _globalSystem;

    /// <summary>
    /// 初始化方法
    /// </summary>
    protected override void OnInit()
    {
        base.OnInit();
        _entity = new PlayerStatusEntity();
        _globalSystem = this.GetSystem<GlobalSystem>();

        this.RegisterEvent<UpdateStatusMaxHealthEvent>(OnUpdateMaxHealth);
        this.RegisterEvent<UpdateStatusCurHealthEvent>(OnUpdateCurHealth);
        this.RegisterEvent<UpdateStatusLossEvent>(OnUpdateLoss);
        this.RegisterEvent<UpdateStatusMaxEnergyEvent>(OnUpdateMaxEnergy);
        this.RegisterEvent<UpdateStatusCurEnergyEvent>(OnUpdateCurEnergy);

    }

    private void Start()
    {

    }


    #region  更新玩家状态

    private void OnUpdateMaxHealth(UpdateStatusMaxHealthEvent evt)
    {

    }

    private void OnUpdateCurHealth(UpdateStatusCurHealthEvent evt)
    {
        _view.UpdateHealthBar(evt.currentHealth);
        _view.UpdateHealthValue(evt.currentHealth);
    }

    private void OnUpdateLoss(UpdateStatusLossEvent evt)
    {

    }

    private void OnUpdateMaxEnergy(UpdateStatusMaxEnergyEvent evt)
    {

    }

    private void OnUpdateCurEnergy(UpdateStatusCurEnergyEvent evt)
    {
        _view.UpdateEnergyBar(evt.currentEnergy);
        _view.UpdateEnergyValue(evt.currentEnergy);
    }

    #endregion


    /// <summary>
    /// 反初始化方法
    /// </summary>
    protected override void OnDeInit()
    {
        base.OnDeInit();

        this.UnRegisterEvent<UpdateStatusMaxHealthEvent>(OnUpdateMaxHealth);
        this.UnRegisterEvent<UpdateStatusCurHealthEvent>(OnUpdateCurHealth);
        this.UnRegisterEvent<UpdateStatusLossEvent>(OnUpdateLoss);
        this.UnRegisterEvent<UpdateStatusMaxEnergyEvent>(OnUpdateMaxEnergy);
        this.UnRegisterEvent<UpdateStatusCurEnergyEvent>(OnUpdateCurEnergy);

    }
}
