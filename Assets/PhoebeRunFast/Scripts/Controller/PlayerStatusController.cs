using Codice.CM.Common;
using Frame;
using QMVC;
using TextMateSharp.Internal.Rules;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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

    }

	private void Start()
	{
		_globalSystem.GameSingleton.GameEntity.Health.Register(OnHealthChanged);
		_globalSystem.GameSingleton.GameEntity.Energy.Register(OnManaChanged);
	}



	private void OnHealthChanged(float currentHealth)
    {
        //_view.UpdateHealthBar(_entity.Health.Value);
        _view.UpdateHealthValue(currentHealth);
    }

    private void OnManaChanged(float currentMana)
    {
        //_view.UpdateManaBar(_entity.Mana.Value);
        _view.UpdateManaValue(currentMana);
    }

    /// <summary>
    /// 反初始化方法
    /// </summary>
    protected override void OnDeInit()
    {
        base.OnDeInit();

		// 注销事件，对象会直接消失，可以不用注销
		//_globalSystem.GameSingleton.GameEntity.Health.UnRegister(OnHealthChanged);
		//_globalSystem.GameSingleton.GameEntity.Energy.UnRegister(OnManaChanged);
	}
}
