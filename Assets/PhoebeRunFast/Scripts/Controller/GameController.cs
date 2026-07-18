using System;
using System.Collections;
using System.Threading.Tasks;
using Frame;
using QMVC;
using UnityEngine;

/// <summary>
/// 游戏控制器
/// </summary>
public class GameController : BaseController
{
	[SerializeField] GameView _view;

	[SerializeField] PlayerController _playerController;

	public PlayerController PlayerController => _playerController;

	GlobalSystem _globalSystem;
	RoleSystem _roleSystem;
	RoadSystem _roadSystem;
	AccountSystem _accountSystem;


	GameEntity _entity;
	public GameEntity GameEntity => _entity;


	public Transform[] GetLines()
	{
		return _view.Lanes;
	}

	public Transform GetLine(int line)
	{
		if(line < 0 || line >= _view.Lanes.Length)
		{
			Debug.LogError("line out of range");
			return null;
		}
		return _view.Lanes[line];
	}




	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{
		base.OnInit();
		_entity = new GameEntity();
		_globalSystem = this.GetSystem<GlobalSystem>();
		_globalSystem.SetGameSingleton(this);
		_accountSystem = this.GetSystem<AccountSystem>();
		_roleSystem = this.GetSystem<RoleSystem>();
		_roadSystem = this.GetSystem<RoadSystem>();
		Debug.Log("GameInit");

		_entity.GameState.RegisterWithInitValue(OnGameStateChanged);
		// 注册系统事件
		this.RegisterEvent<LoadGameEvent>(OnLoadGame);
		this.RegisterEvent<UnLoadGameEvent>(OnUnLoadGame);
		this.RegisterEvent<GameInitByTransitionOverEvent>(OnGameInitByTransitionOver);
		this.RegisterEvent<GameResetEvent>(OnGameReset);

		
	}


	private void OnGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.Ready:
				_globalSystem.Inputs.Disable();
				break;
			case GameState.Running:
				_globalSystem.Inputs.Enable();
				break;
			case GameState.Paused:
				_globalSystem.Inputs.Disable();
				break;
			case GameState.Over:
				_globalSystem.Inputs.Disable();
				break;
			default:
				break;
		}
	}



	/// <summary>
	/// 游戏初始化完成事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnGameInitByTransitionOver(GameInitByTransitionOverEvent evt)
	{
		//TODO: 初始可以是一个开场动画（实时）
		//播放
		Debug.Log("开场动画");

		_globalSystem.GameSingleton.GameEntity.GameState.Value = GameState.Running;
	}

	private void OnGameReset(GameResetEvent evt)
	{
		MonoService.Instance.StartCoroutine(GameReset());
	}


	/// <summary>
	/// 加载游戏事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnLoadGame(LoadGameEvent evt)
	{
		evt.enumerator = GameAssetLoad();
	}

	/// <summary>
	/// 卸载游戏事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnUnLoadGame(UnLoadGameEvent evt)
	{
		evt.enumerator = GameAssetUnLoad();
	}

	/// <summary>
	/// 加载游戏资源
	/// </summary>
	/// <returns>协程</returns>
	IEnumerator GameAssetLoad()
	{
		//TODO: 加载资源
		//TODO: 初始一段环境
		//TODO: 初始Player
		//TODO: 根据需要加载的角色获取角色数据
		//TODO：根据获取的角色数据，计算角色属性 赋值RoleData

		

		string roleId = _globalSystem.GlobalModel.UserJson.outRoleId;

		Task<RoleController> roleTask = _roleSystem.GetRole(roleId);

		Task<PropertyJson> propertyTask = _roleSystem.GetProperty(roleId);
		Task<ChainLevelJson> chainLevelTask = _roleSystem.GetChainLevel(roleId);
		Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);
		Task<ChainUpgradeJson> chainUpgradeTask = _roleSystem.GetChainUpgrade(roleId);
		Task<PropertyUpgradeJson> propertyUpgradeTask = _roleSystem.GetPropertyUpgrade(roleId);

		Task<AccountRole> accountRoleTask = _accountSystem.GetRole(_globalSystem.GlobalModel.UserJson.userId, roleId);



		Task total = Task.WhenAll(roleTask,propertyTask, chainLevelTask, propertyLevelTask, chainUpgradeTask, propertyUpgradeTask, accountRoleTask);
		yield return new WaitUntil(() => total.IsCompleted);
		
		yield return _roadSystem.InitRoad();

		//TODO: 设置到PlayerController上
		Debug.Log("TODO:将模型设置到Player上");
		Task<RoleController> task = _roleSystem.GetRole(roleId);
		yield return new WaitUntil(() => task.IsCompleted);
		RoleController controller = task.Result;
		//设置到Player上
		this.SendCommand(new SetPlayerRoleCommand(controller));

		Debug.Log($"{_globalSystem.GameSingleton},{_globalSystem.GameSingleton.GameEntity}");

		float health = 0, energy = 0, attack = 0, defense = 0, speed = 0, cooldownReduction = 0, energyRecoveryRate = 0;

		health = propertyTask.Result.health;
		energy = propertyTask.Result.energy;
		attack = propertyTask.Result.attack;
		defense = propertyTask.Result.defense;
		speed = propertyTask.Result.speed;
		cooldownReduction = propertyTask.Result.cooldownReduction;
		energyRecoveryRate = propertyTask.Result.energyRecoveryRate;

		for (int i = 0; i < accountRoleTask.Result.rolePropertyLevel.health; i++)
		{
			health += propertyUpgradeTask.Result.healthUpgrade[i];
		}
		for (int i = 0; i < accountRoleTask.Result.rolePropertyLevel.energy; i++)
		{
			energy += propertyUpgradeTask.Result.energyUpgrade[i];
		}
		for (int i = 0; i < accountRoleTask.Result.rolePropertyLevel.attack; i++)
		{
			attack += propertyUpgradeTask.Result.attackUpgrade[i];
		}
		for (int i = 0; i < accountRoleTask.Result.rolePropertyLevel.defense; i++)
		{
			defense += propertyUpgradeTask.Result.defenseUpgrade[i];
		}
		for (int i = 0; i < accountRoleTask.Result.rolePropertyLevel.speed; i++)
		{
			speed += propertyUpgradeTask.Result.speedUpgrade[i];
		}
		for (int i = 0; i < accountRoleTask.Result.rolePropertyLevel.cooldownReduction; i++)
		{
			cooldownReduction += propertyUpgradeTask.Result.cooldownReductionUpgrade[i];
		}
		for (int i = 0; i < accountRoleTask.Result.rolePropertyLevel.energyRecoveryRate; i++)
		{
			energyRecoveryRate += propertyUpgradeTask.Result.energyRecoveryRateUpgrade[i];
		}

		for(int i = 0; i < accountRoleTask.Result.chainLevel; i++)
		{
			health += chainUpgradeTask.Result.upgradeJsons[i].health;
			energy += chainUpgradeTask.Result.upgradeJsons[i].energy;
			attack += chainUpgradeTask.Result.upgradeJsons[i].attack;
			defense += chainUpgradeTask.Result.upgradeJsons[i].defense;
			speed += chainUpgradeTask.Result.upgradeJsons[i].speed;
			cooldownReduction += chainUpgradeTask.Result.upgradeJsons[i].cooldownReduction;
		}

		//TODO: 应用天赋效果
		Debug.Log("天赋效果");

		//TODO: 初始化界面

		this.SendCommand(new SetHealthCommand(health));
		this.SendCommand(new SetEnergyCommand(energy));
		this.SendCommand(new SetAttackCommand(attack));
		this.SendCommand(new SetDefenseCommand(defense));
		this.SendCommand(new SetSpeedCommand(speed));
		this.SendCommand(new SetCooldownReductionCommand(cooldownReduction));
		this.SendCommand(new SetEnergyRecoveryRateCommand(energyRecoveryRate));


		
		_roadSystem.StartRoad();

	}

	/// <summary>
	/// 卸载游戏资源
	/// </summary>
	/// <returns>协程</returns>
	IEnumerator GameAssetUnLoad()
	{
		_roadSystem.StopRoad();
		//主动回收RoleController
		_roleSystem.RecycleAllRole();
		_roadSystem.RecycleAllRoad();
		//主动回收RoadController对象池
		yield return new WaitForSeconds(5f);
	}



	IEnumerator GameReset()
	{
		yield return new WaitForSeconds(5f);
		this.SendCommand(new CloseTransitionCommand(() => OnGameInitByTransitionOver(null)));
	}



	/// <summary>
	/// 反初始化方法
	/// </summary>
	protected override void OnDeInit()
	{
		base.OnDeInit();

		_globalSystem.SetGameSingleton(null);

		// 注销事件
		this.UnRegisterEvent<LoadGameEvent>(OnLoadGame);
		this.UnRegisterEvent<UnLoadGameEvent>(OnUnLoadGame);
		this.UnRegisterEvent<GameInitByTransitionOverEvent>(OnGameInitByTransitionOver);

	}

}
