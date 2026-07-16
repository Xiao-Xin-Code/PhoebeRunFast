using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Frame;
using QMVC;
using UnityEngine;

/// <summary>
/// 角色系统控制器
/// </summary>
public class RoleSystemController : BaseController
{
    [SerializeField] RoleSystemView _view;
    RoleSystemEntity _entity;

	GlobalSystem _globalSystem;
	RoleSystem _roleSystem;
	AccountSystem _accountSystem;

	/// <summary>
	/// 当前角色
	/// </summary>
	RoleController character;

	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{
		base.OnInit();
		_globalSystem = this.GetSystem<GlobalSystem>();
		_roleSystem = this.GetSystem<RoleSystem>();
		_accountSystem = this.GetSystem<AccountSystem>();
		
		_entity = new RoleSystemEntity();

		// 注册事件
		this.RegisterEvent<RoleMenuActiveEvent>(OnRoleMenuActive);
		this.RegisterEvent<ToLeftRoleEvent>(OnToLeftRole);
		this.RegisterEvent<ToRightRoleEvent>(OnToRightRole);
		this.RegisterEvent<InitCharacterEvent>(OnInitCharacter);
		this.RegisterEvent<UpGradeLevelEvent>(OnUpGradeLevel);
	}

	Coroutine targetCoroutine;
	Coroutine leftCoroutine;
	Coroutine rightCoroutine;


	/// <summary>
	/// 角色菜单激活事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnRoleMenuActive(RoleMenuActiveEvent evt)
	{
		_view.SetSwitchActive(evt.isActive);
		_view.SetPropertyActive(evt.isActive);
		if (targetCoroutine != null)
		{
			MonoService.Instance.StopCoroutine(targetCoroutine);
			targetCoroutine = null;
		}
		targetCoroutine = MonoService.Instance.StartCoroutine(ToTargetAsync(_entity.outRoleIndex));
	}

	/// <summary>
	/// 切换到目标
	/// </summary>
	/// <param name="evt"></param>
	private void OnInitCharacter(InitCharacterEvent evt)
	{
		if(targetCoroutine != null)
		{
			MonoService.Instance.StopCoroutine(targetCoroutine);
			targetCoroutine = null;
		}
		targetCoroutine = MonoService.Instance.StartCoroutine(ToTargetAsyncWithoutData(evt.tableId));
		_entity.outRoleIndex = evt.tableId;		
	}

	/// <summary>
	/// 切换到左侧角色
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnToLeftRole(ToLeftRoleEvent evt)
	{
		if (_entity.isBusy) return;
		_entity.isBusy = true;
		if (leftCoroutine != null) 
		{
			MonoService.Instance.StopCoroutine(leftCoroutine);
			leftCoroutine = null;
		}
		leftCoroutine = MonoService.Instance.StartCoroutine(ToLeftAsync());
	}

	/// <summary>
	/// 切换到右侧角色
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnToRightRole(ToRightRoleEvent evt)
	{
		if (_entity.isBusy) return;
		_entity.isBusy = true;
		if (rightCoroutine != null)
		{
			MonoService.Instance.StopCoroutine(rightCoroutine);
			rightCoroutine = null;
		}
		rightCoroutine = MonoService.Instance.StartCoroutine(ToRightAsync());
	}


	private void OnUpGradeLevel(UpGradeLevelEvent evt)
	{
		switch (evt.level)
		{
			case Consts.Chain:
				MonoService.Instance.StartCoroutine(ChainUpgradeAsync());
				break;
			case Consts.Health:
				MonoService.Instance.StartCoroutine(HealthUpgradeAsync());
				break;
			case Consts.Energy:
				MonoService.Instance.StartCoroutine(EnergyUpgradeAsync());
				break;
			case Consts.Defense:
				MonoService.Instance.StartCoroutine(DefenseUpgradeAsync());
				break;
			case Consts.CooldownReduction:
				MonoService.Instance.StartCoroutine(CooldownReductionUpgradeAsync());
				break;
			default:
				break;
		}
	}

	#region 等级提升协程

	IEnumerator ChainUpgradeAsync()
	{
		string roleId = _entity.roleIds[_entity.outRoleIndex];
		Task<ChainLevelJson> chainTask = _roleSystem.GetChainLevel(roleId);
		Task<AccountRole> roleTask = _accountSystem.GetRole(_globalSystem.GlobalModel.UserJson.userId, roleId);
		Task total = Task.WhenAll(chainTask, roleTask);
		yield return new WaitUntil(() => total.IsCompleted);
		if (roleTask.Result.chainLevel < chainTask.Result.chainLevel.maxLevel)
		{
			roleTask.Result.chainLevel++;
			this.SendCommand(new ShowLevelCommand(Consts.Chain, chainTask.Result.chainLevel.baseLevel, roleTask.Result.chainLevel, chainTask.Result.chainLevel.maxLevel));
			Task task = _roleSystem.UpdateChainLevel(roleId);
			yield return new WaitUntil(() => task.IsCompleted);

			//TODO: 处理当前状态
		}
	}

	IEnumerator HealthUpgradeAsync()
	{
		string roleId = _entity.roleIds[_entity.outRoleIndex];
		Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);
		Task<AccountRole> roleTask = _accountSystem.GetRole(_globalSystem.GlobalModel.UserJson.userId, roleId);
		Task total = Task.WhenAll(propertyLevelTask, roleTask);
		yield return new WaitUntil(() => total.IsCompleted);
		if (roleTask.Result.rolePropertyLevel.health < propertyLevelTask.Result.healthLevel.maxLevel)
		{
			roleTask.Result.rolePropertyLevel.health++;
			this.SendCommand(new ShowLevelCommand(Consts.Health, propertyLevelTask.Result.healthLevel.baseLevel, roleTask.Result.rolePropertyLevel.health, propertyLevelTask.Result.healthLevel.maxLevel));

			Task task = _roleSystem.UpdatePropertyLevel(roleId);
			yield return new WaitUntil(() => task.IsCompleted);

			//TODO: 处理当前状态
		}
	}

	IEnumerator EnergyUpgradeAsync()
	{
		string roleId = _entity.roleIds[_entity.outRoleIndex];
		Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);
		Task<AccountRole> roleTask = _accountSystem.GetRole(_globalSystem.GlobalModel.UserJson.userId, roleId);
		Task total = Task.WhenAll(propertyLevelTask, roleTask);
		yield return new WaitUntil(() => total.IsCompleted);
		if (roleTask.Result.rolePropertyLevel.energy < propertyLevelTask.Result.energyLevel.maxLevel)
		{
			roleTask.Result.rolePropertyLevel.energy++;
			this.SendCommand(new ShowLevelCommand(Consts.Energy, propertyLevelTask.Result.energyLevel.baseLevel, roleTask.Result.rolePropertyLevel.energy, propertyLevelTask.Result.energyLevel.maxLevel));
			
			Task task = _roleSystem.UpdatePropertyLevel(roleId);
			yield return new WaitUntil(() => task.IsCompleted);

			//TODO: 处理当前状态

		}
	}

	IEnumerator DefenseUpgradeAsync()
	{
		string roleId = _entity.roleIds[_entity.outRoleIndex];
		Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);
		Task<AccountRole> roleTask = _accountSystem.GetRole(_globalSystem.GlobalModel.UserJson.userId, roleId);
		Task total = Task.WhenAll(propertyLevelTask, roleTask);
		yield return new WaitUntil(() => total.IsCompleted);
		if (roleTask.Result.rolePropertyLevel.defense < propertyLevelTask.Result.defenseLevel.maxLevel)
		{
			roleTask.Result.rolePropertyLevel.defense++;
			this.SendCommand(new ShowLevelCommand(Consts.Defense, propertyLevelTask.Result.defenseLevel.baseLevel, roleTask.Result.rolePropertyLevel.defense, propertyLevelTask.Result.defenseLevel.maxLevel));
			Task task = _roleSystem.UpdatePropertyLevel(roleId);
			yield return new WaitUntil(() => task.IsCompleted);

			//TODO: 处理当前状态
		}
	}

	IEnumerator CooldownReductionUpgradeAsync()
	{
		string roleId = _entity.roleIds[_entity.outRoleIndex];
		Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);
		Task<AccountRole> roleTask = _accountSystem.GetRole(_globalSystem.GlobalModel.UserJson.userId, roleId);
		Task total = Task.WhenAll(propertyLevelTask, roleTask);
		yield return new WaitUntil(() => total.IsCompleted);
		if (roleTask.Result.rolePropertyLevel.cooldownReduction < propertyLevelTask.Result.cooldownReductionLevel.maxLevel)
		{
			roleTask.Result.rolePropertyLevel.cooldownReduction++;
			this.SendCommand(new ShowLevelCommand(Consts.CooldownReduction, propertyLevelTask.Result.cooldownReductionLevel.baseLevel, roleTask.Result.rolePropertyLevel.cooldownReduction, propertyLevelTask.Result.cooldownReductionLevel.maxLevel));
			Task task = _roleSystem.UpdatePropertyLevel(roleId);
			yield return new WaitUntil(() => task.IsCompleted);

			//TODO: 处理当前状态
		}
	}

	#endregion

	#region 协程

	/// <summary>
	/// 转移到目标角色，但是不获取与设置角色详细数据
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	IEnumerator ToTargetAsyncWithoutData(int index)
	{
		if (character)
		{
			_roleSystem.RecycleRole(character);
			character = null;
		}
		string roleId = _globalSystem.GlobalModel.RoleJsons[index].roleId;
		Task<RoleController> task = _roleSystem.GetRole(roleId);
		yield return new WaitUntil(() => task.IsCompleted);
		RoleController controller = task.Result;
		character = controller;
		Debug.Log(_view);
		character.transform.position = _view.Center.position;
	}


	IEnumerator ToTargetAsync(int index)
	{
		if (character)
		{
			_roleSystem.RecycleRole(character);
			character = null;
		}
		string roleId = _globalSystem.GlobalModel.RoleJsons[index].roleId;
		Task<RoleController> task = _roleSystem.GetRole(roleId);
		yield return new WaitUntil(() => task.IsCompleted);
		RoleController controller = task.Result;
		character = controller;
		Debug.Log(_view);
		character.transform.position = _view.Center.position;

		//TODO: 确定状态

		Task<ChainLevelJson> chainLevelTask = _roleSystem.GetChainLevel(roleId);
		Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);

		Task<AccountRole> roleTask = _accountSystem.GetRole(_globalSystem.GlobalModel.UserJson.userId, roleId);

		Task levelTask = Task.WhenAll(chainLevelTask, propertyLevelTask, roleTask);
		yield return new WaitUntil(() => levelTask.IsCompleted);

		this.SendCommand(new ShowLevelCommand(Consts.Chain, chainLevelTask.Result.chainLevel.baseLevel, roleTask.Result.chainLevel, chainLevelTask.Result.chainLevel.maxLevel));
		this.SendCommand(new ShowLevelCommand(Consts.Health, propertyLevelTask.Result.healthLevel.baseLevel, roleTask.Result.rolePropertyLevel.health, propertyLevelTask.Result.healthLevel.maxLevel));
		this.SendCommand(new ShowLevelCommand(Consts.Energy, propertyLevelTask.Result.energyLevel.baseLevel, roleTask.Result.rolePropertyLevel.energy, propertyLevelTask.Result.energyLevel.maxLevel));
		this.SendCommand(new ShowLevelCommand(Consts.Defense, propertyLevelTask.Result.defenseLevel.baseLevel, roleTask.Result.rolePropertyLevel.defense, propertyLevelTask.Result.defenseLevel.maxLevel));
		this.SendCommand(new ShowLevelCommand(Consts.CooldownReduction, propertyLevelTask.Result.cooldownReductionLevel.baseLevel, roleTask.Result.rolePropertyLevel.cooldownReduction, propertyLevelTask.Result.cooldownReductionLevel.maxLevel));

	}

	IEnumerator ToLeftAsync()
	{
		GetPreviousRoleId();

		string roleId = _entity.roleIds[_entity.outRoleIndex];
		Task<RoleController> roleTask = _roleSystem.GetRole(roleId);
		Task<InfoJson> infoTask = _roleSystem.GetInfo(roleId);
		Task<LockJson> lockTask = _roleSystem.GetLock(roleId);

		Task<AccountRole> accountRoleTask = _accountSystem.GetRole(_globalSystem.GlobalModel.UserJson.userId, roleId);

		Task total = Task.WhenAll(roleTask, infoTask, lockTask, accountRoleTask);
		yield return new WaitUntil(() => total.IsCompleted);

		RoleController leftCharacter = roleTask.Result;
		leftCharacter.transform.position = _view.Left.position;
		RoleController temp = character;
		character = leftCharacter;

		bool isUnLock = accountRoleTask.Result != null;
		if (isUnLock)
		{
			Task<ChainLevelJson> chainLevelTask = _roleSystem.GetChainLevel(roleId);
			Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);
			Task levelTask = Task.WhenAll(chainLevelTask, propertyLevelTask);
			yield return new WaitUntil(() => levelTask.IsCompleted);

			this.SendCommand(new ShowLevelCommand(Consts.Chain, chainLevelTask.Result.chainLevel.baseLevel, accountRoleTask.Result.chainLevel, chainLevelTask.Result.chainLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.Health, propertyLevelTask.Result.healthLevel.baseLevel, accountRoleTask.Result.rolePropertyLevel.health, propertyLevelTask.Result.healthLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.Energy, propertyLevelTask.Result.energyLevel.baseLevel, accountRoleTask.Result.rolePropertyLevel.energy, propertyLevelTask.Result.energyLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.Defense, propertyLevelTask.Result.defenseLevel.baseLevel, accountRoleTask.Result.rolePropertyLevel.defense, propertyLevelTask.Result.defenseLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.CooldownReduction, propertyLevelTask.Result.cooldownReductionLevel.baseLevel, accountRoleTask.Result.rolePropertyLevel.cooldownReduction, propertyLevelTask.Result.cooldownReductionLevel.maxLevel));
		}
		else
		{

		}

		_view.SetRoleLockActive(false);

		Sequence mainSequence = DOTween.Sequence();
		Sequence oldSequence = DOTween.Sequence();
		oldSequence.Append(temp.transform.DORotate(new Vector3(0, 90, 0), 0.5f));
		oldSequence.Join(_view.SwitchSequence(false));
		oldSequence.Join(_view.PropertySequence(false));
		oldSequence.Append(temp.transform.DOMoveX(_view.Right.position.x, 1f));
		oldSequence.Append(temp.transform.DORotate(new Vector3(0, 0, 0), 0.5f));
		Sequence newSequence = DOTween.Sequence();
		newSequence.Append(leftCharacter.transform.DORotate(new Vector3(0, 90, 0), 0.5f));
		newSequence.Append(leftCharacter.transform.DOMoveX(_view.Center.position.x, 1f));
		newSequence.Append(leftCharacter.transform.DORotate(new Vector3(0, 0, 0), 0.5f));
		newSequence.Join(_view.SwitchSequence(true));
		if (isUnLock) newSequence.Join(_view.PropertySequence(true));
		else newSequence.AppendCallback(() => { _view.SetRoleLockActive(true); });
		mainSequence.Join(oldSequence);
		mainSequence.Join(newSequence);
		mainSequence.OnComplete(() =>
		{
			_roleSystem.RecycleRole(temp);
			_entity.isBusy = false;
		});
		mainSequence.Play();
	}

	IEnumerator ToRightAsync()
	{
		GetNextRoleId();
		string roleId = _entity.roleIds[_entity.outRoleIndex];
		Task<RoleController> task = _roleSystem.GetRole(roleId);
		Task<LockJson> lockTask = _roleSystem.GetLock(roleId);

		Task<AccountRole> accountRoleTask = _accountSystem.GetRole(_globalSystem.GlobalModel.UserJson.userId, roleId);

		Task total = Task.WhenAll(task, lockTask, accountRoleTask);
		yield return new WaitUntil(() => total.IsCompleted);
		bool isUnLock = accountRoleTask.Result != null;

		if (isUnLock)
		{
			Task<ChainLevelJson> chainLevelTask = _roleSystem.GetChainLevel(roleId);
			Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);
			Task levelTask = Task.WhenAll(chainLevelTask, propertyLevelTask);
			yield return new WaitUntil(() => levelTask.IsCompleted);

			this.SendCommand(new ShowLevelCommand(Consts.Chain, chainLevelTask.Result.chainLevel.baseLevel, accountRoleTask.Result.chainLevel, chainLevelTask.Result.chainLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.Health, propertyLevelTask.Result.healthLevel.baseLevel, accountRoleTask.Result.rolePropertyLevel.health, propertyLevelTask.Result.healthLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.Energy, propertyLevelTask.Result.energyLevel.baseLevel, accountRoleTask.Result.rolePropertyLevel.energy, propertyLevelTask.Result.energyLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.Defense, propertyLevelTask.Result.defenseLevel.baseLevel, accountRoleTask.Result.rolePropertyLevel.defense, propertyLevelTask.Result.defenseLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.CooldownReduction, propertyLevelTask.Result.cooldownReductionLevel.baseLevel, accountRoleTask.Result.rolePropertyLevel.cooldownReduction, propertyLevelTask.Result.cooldownReductionLevel.maxLevel));
		}
		else
		{
			//TODO: 获取解锁条件


		}

		RoleController rightCharacter = task.Result;
		rightCharacter.transform.position = _view.Right.position;
		RoleController temp = character;
		character = rightCharacter;
		_view.SetRoleLockActive(false);
		Sequence mainSequence = DOTween.Sequence();
		Sequence oldSequence = DOTween.Sequence();
		oldSequence.Append(temp.transform.DORotate(new Vector3(0, -90, 0), 0.5f));
		oldSequence.Join(_view.SwitchSequence(false));
		oldSequence.Join(_view.PropertySequence(false));
		oldSequence.Append(temp.transform.DOMoveX(_view.Left.position.x, 1f));
		oldSequence.Append(temp.transform.DORotate(new Vector3(0, 0, 0), 0.5f));
		Sequence newSequence = DOTween.Sequence();
		newSequence.Append(rightCharacter.transform.DORotate(new Vector3(0, -90, 0), 0.5f));
		newSequence.Append(rightCharacter.transform.DOMoveX(_view.Center.position.x, 1f));
		newSequence.Append(rightCharacter.transform.DORotate(new Vector3(0, 0, 0), 0.5f));
		newSequence.Join(_view.SwitchSequence(true));
		if (isUnLock) newSequence.Join(_view.PropertySequence(true));
		else newSequence.AppendCallback(() => { _view.SetRoleLockActive(true); });
		mainSequence.Join(oldSequence);
		mainSequence.Join(newSequence);
		mainSequence.OnComplete(() =>
		{
			_roleSystem.RecycleRole(temp);
			_entity.isBusy = false;
		});
		mainSequence.Play();
	}

	#endregion


	//处理初始化角色列表排序问题
	private void InitRoleList()
	{

		//先获取账户持有角色数据


		if (_entity.outRoleIndex == -1)
		{
			_entity.outRoleIndex = 0;
		}
	}

	private void GetNextRoleId()
	{
		if (_entity.outRoleIndex == _globalSystem.GlobalModel.RoleJsons.Length - 1) _entity.outRoleIndex = 0;
		else _entity.outRoleIndex++;
	}

	private void GetPreviousRoleId()
	{
		if (_entity.outRoleIndex == 0) _entity.outRoleIndex = _globalSystem.GlobalModel.RoleJsons.Length - 1;
		else _entity.outRoleIndex--;
	}






	protected override void OnDeInit()
	{
		base.OnDeInit();

		if (character != null)
		{
			_roleSystem.RecycleRole(character);
			character = null;
		}

		MonoService.Instance.StopCoroutine(targetCoroutine);
		MonoService.Instance.StopCoroutine(leftCoroutine);
		MonoService.Instance.StopCoroutine(rightCoroutine);

		this.UnRegisterEvent<RoleMenuActiveEvent>(OnRoleMenuActive);
		this.UnRegisterEvent<ToLeftRoleEvent>(OnToLeftRole);
		this.UnRegisterEvent<ToRightRoleEvent>(OnToRightRole);
		this.UnRegisterEvent<InitCharacterEvent>(OnInitCharacter);
		this.UnRegisterEvent<UpGradeLevelEvent>(OnUpGradeLevel);
	}

}





