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

	GlobalSystem _globalSystem;
	RoleSystem _roleSystem;


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

		_globalSystem = this.GetSystem<GlobalSystem>();
		_globalSystem.SetGameSingleton(this);

		_roleSystem = this.GetSystem<RoleSystem>();

		// 注册系统事件
		this.RegisterEvent<LoadGameEvent>(OnLoadGame);
		this.RegisterEvent<UnLoadGameEvent>(OnUnLoadGame);
		this.RegisterEvent<GameInitByTransitionOverEvent>(OnGameInitByTransitionOver);
		this.RegisterEvent<GameResetEvent>(OnGameReset);

		_entity = new GameEntity();
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

		

		string roleId = _globalSystem.GlobalModel.RoleJsons[_globalSystem.GlobalModel.OutRoleTableId.Value].roleId;

		Task<RoleController> roleTask = _roleSystem.GetRole(roleId);

		Task<PropertyJson> propertyTask = _roleSystem.GetProperty(roleId);
		Task<StarLevelJson> starLevelTask = _roleSystem.GetStarLevel(roleId);
		Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);
		Task<StarUpgradeJson> starUpgradeTask = _roleSystem.GetStarUpgrade(roleId);
		Task<PropertyUpgradeJson> propertyUpgradeTask = _roleSystem.GetPropertyUpgrade(roleId);
		Task total = Task.WhenAll(roleTask,propertyTask, starLevelTask, propertyLevelTask, starUpgradeTask, propertyUpgradeTask);
		yield return new WaitUntil(() => total.IsCompleted);

		//TODO: 设置到PlayerController上
		Debug.Log("TODO:将模型设置到Player上");

		Debug.Log($"{_globalSystem.GameSingleton},{_globalSystem.GameSingleton.GameEntity},{_globalSystem.GameSingleton.GameEntity.roleData}");
		_globalSystem.GameSingleton.GameEntity.roleData.property.health = propertyTask.Result.health;
		_globalSystem.GameSingleton.GameEntity.roleData.property.energy = propertyTask.Result.energy;
		_globalSystem.GameSingleton.GameEntity.roleData.property.defense = propertyTask.Result.defense;
		_globalSystem.GameSingleton.GameEntity.roleData.property.cooldownReduction = propertyTask.Result.cooldownReduction;
		for (int i = 0; i < propertyLevelTask.Result.healthLevel.currentLevel; i++)
		{
			_globalSystem.GameSingleton.GameEntity.roleData.property.health += propertyUpgradeTask.Result.healthUpgrade[i];
		}
		for (int i = 0; i < propertyLevelTask.Result.energyLevel.currentLevel; i++)
		{
			_globalSystem.GameSingleton.GameEntity.roleData.property.energy += propertyUpgradeTask.Result.energyUpgrade[i];
		}
		for (int i = 0; i < propertyLevelTask.Result.defenseLevel.currentLevel; i++)
		{
			_globalSystem.GameSingleton.GameEntity.roleData.property.defense += propertyUpgradeTask.Result.defenseUpgrade[i];
		}
		for (int i = 0; i < propertyLevelTask.Result.cooldownReductionLevel.currentLevel; i++)
		{
			_globalSystem.GameSingleton.GameEntity.roleData.property.cooldownReduction += propertyUpgradeTask.Result.cooldownReductionUpgrade[i];
		}

		for(int i = 0; i < starLevelTask.Result.starLevel.currentLevel; i++)
		{
			_globalSystem.GameSingleton.GameEntity.roleData.property.health += starUpgradeTask.Result.upgradeJsons[i].healthUpgrade;
			_globalSystem.GameSingleton.GameEntity.roleData.property.energy += starUpgradeTask.Result.upgradeJsons[i].energyUpgrade;
			_globalSystem.GameSingleton.GameEntity.roleData.property.attack += starUpgradeTask.Result.upgradeJsons[i].attackUpgrade;
			_globalSystem.GameSingleton.GameEntity.roleData.property.defense += starUpgradeTask.Result.upgradeJsons[i].defenseUpgrade;
			_globalSystem.GameSingleton.GameEntity.roleData.property.speed += starUpgradeTask.Result.upgradeJsons[i].speedUpgrade;
			_globalSystem.GameSingleton.GameEntity.roleData.property.cooldownReduction += starUpgradeTask.Result.upgradeJsons[i].cooldownReductionUpgrade;
		}

		//TODO: 应用天赋效果
		Debug.Log("天赋效果");

		//TODO: 初始化界面

		yield return new WaitForSeconds(5f);
	}

	/// <summary>
	/// 卸载游戏资源
	/// </summary>
	/// <returns>协程</returns>
	IEnumerator GameAssetUnLoad()
	{
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
