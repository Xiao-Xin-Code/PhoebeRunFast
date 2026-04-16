using Frame;
using QMVC;
using UnityEngine;

/// <summary>
/// 玩家状态控制器
/// </summary>
public class PlayerStatusController : BaseController
{
    [SerializeField] PlayerStatusView _view;

    GameModel _gameModel;

    /// <summary>
    /// 初始化方法
    /// </summary>
    protected override void OnInit()
    {
        base.OnInit();

        _gameModel = this.GetModel<GameModel>();

        // 注册事件
        this.RegisterEvent<HealthChangeEvent>(OnHealthChange);
        this.RegisterEvent<ManaChangeEvent>(OnManaChange);

        // 初始化显示
        UpdateStatusDisplay();
    }

    /// <summary>
    /// 生命值变化事件
    /// </summary>
    /// <param name="evt">事件参数</param>
    private void OnHealthChange(HealthChangeEvent evt)
    {
        _view.UpdateHealthBar(evt.currentHealth, evt.maxHealth);
    }

    /// <summary>
    /// 魔法值变化事件
    /// </summary>
    /// <param name="evt">事件参数</param>
    private void OnManaChange(ManaChangeEvent evt)
    {
        _view.UpdateManaBar(evt.currentMana, evt.maxMana);
    }

    /// <summary>
    /// 更新状态显示
    /// </summary>
    private void UpdateStatusDisplay()
    {
        _view.UpdateHealthBar(_gameModel.PlayerHealth.Value, _gameModel.PlayerMaxHealth.Value);
        _view.UpdateManaBar(_gameModel.PlayerMana.Value, _gameModel.PlayerMaxMana.Value);
    }

    /// <summary>
    /// 反初始化方法
    /// </summary>
    protected override void OnDeInit()
    {
        base.OnDeInit();

        // 注销事件
        this.UnRegisterEvent<HealthChangeEvent>(OnHealthChange);
        this.UnRegisterEvent<ManaChangeEvent>(OnManaChange);
    }
}
