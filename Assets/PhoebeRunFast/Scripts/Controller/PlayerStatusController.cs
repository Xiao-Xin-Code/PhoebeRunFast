using Frame;
using QMVC;
using UnityEngine;

/// <summary>
/// 玩家状态控制器
/// </summary>
public class PlayerStatusController : BaseController
{
    [SerializeField] PlayerStatusView _view;

    PlayerStatusEntity _entity;

    GameModel _gameModel;

    /// <summary>
    /// 初始化方法
    /// </summary>
    protected override void OnInit()
    {
        base.OnInit();

        _entity = new PlayerStatusEntity();
        _gameModel = this.GetModel<GameModel>();

        _entity.Health.Register(OnHealthChanged);
        _entity.Mana.Register(OnManaChanged);

    }


    private void OnHealthChanged(float currentHealth)
    {
        _view.UpdateHealthBar(_entity.Health.Value);
    }

    private void OnManaChanged(float currentMana)
    {
        _view.UpdateManaBar(_entity.Mana.Value);
    }

    /// <summary>
    /// 反初始化方法
    /// </summary>
    protected override void OnDeInit()
    {
        base.OnDeInit();

        // 注销事件
        _entity.Health.UnRegister(OnHealthChanged);
        _entity.Mana.UnRegister(OnManaChanged);
    }
}
