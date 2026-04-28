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
		tableId = _globalSystem.GlobalModel.OutRoleTableId.Value;
		if (targetCoroutine != null)
		{
			MonoService.Instance.StopCoroutine(targetCoroutine);
			targetCoroutine = null;
		}
		targetCoroutine = MonoService.Instance.StartCoroutine(ToTargetAsync(tableId));
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
		targetCoroutine = MonoService.Instance.StartCoroutine(ToTargetAsync(evt.tableId));
		this.tableId = evt.tableId;
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

	int tableId = 0;


	private void OnUpGradeLevel(UpGradeLevelEvent evt)
	{
		switch (evt.level)
		{
			case Consts.Star:
				MonoService.Instance.StartCoroutine(StarUpgradeAsync());
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

	IEnumerator StarUpgradeAsync()
	{
		string roleId = _globalSystem.GlobalModel.RoleJsons[tableId].roleId;
		Task<StarLevelJson> starTask = _roleSystem.GetStarLevel(roleId);
		yield return new WaitUntil(() => starTask.IsCompleted);
		if (starTask.Result.starLevel.currentLevel < starTask.Result.starLevel.maxLevel)
		{
			starTask.Result.starLevel.currentLevel++;
			this.SendCommand(new ShowLevelCommand(Consts.Star, starTask.Result.starLevel.baseLevel, starTask.Result.starLevel.currentLevel, starTask.Result.starLevel.maxLevel));
			Task task = _roleSystem.UpdateStarLevel(roleId);
			yield return new WaitUntil(() => task.IsCompleted);

			//TODO: 处理当前状态
		}
	}

	IEnumerator HealthUpgradeAsync()
	{
		string roleId = _globalSystem.GlobalModel.RoleJsons[tableId].roleId;
		Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);
		yield return new WaitUntil(() => propertyLevelTask.IsCompleted);
		if (propertyLevelTask.Result.healthLevel.currentLevel < propertyLevelTask.Result.healthLevel.maxLevel)
		{
			propertyLevelTask.Result.healthLevel.currentLevel++;
			this.SendCommand(new ShowLevelCommand(Consts.Health, propertyLevelTask.Result.healthLevel.baseLevel, propertyLevelTask.Result.healthLevel.currentLevel, propertyLevelTask.Result.healthLevel.maxLevel));

			Task task = _roleSystem.UpdatePropertyLevel(roleId);
			yield return new WaitUntil(() => task.IsCompleted);

			//TODO: 处理当前状态
		}
	}

	IEnumerator EnergyUpgradeAsync()
	{
		string roleId = _globalSystem.GlobalModel.RoleJsons[tableId].roleId;
		Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);
		yield return new WaitUntil(() => propertyLevelTask.IsCompleted);
		if (propertyLevelTask.Result.energyLevel.currentLevel < propertyLevelTask.Result.energyLevel.maxLevel)
		{
			propertyLevelTask.Result.energyLevel.currentLevel++;
			this.SendCommand(new ShowLevelCommand(Consts.Energy, propertyLevelTask.Result.energyLevel.baseLevel, propertyLevelTask.Result.energyLevel.currentLevel, propertyLevelTask.Result.energyLevel.maxLevel));

			Task task = _roleSystem.UpdatePropertyLevel(roleId);
			yield return new WaitUntil(() => task.IsCompleted);

			//TODO: 处理当前状态

		}
	}

	IEnumerator DefenseUpgradeAsync()
	{
		string roleId = _globalSystem.GlobalModel.RoleJsons[tableId].roleId;
		Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);
		yield return new WaitUntil(() => propertyLevelTask.IsCompleted);
		if (propertyLevelTask.Result.defenseLevel.currentLevel < propertyLevelTask.Result.defenseLevel.maxLevel)
		{
			propertyLevelTask.Result.defenseLevel.currentLevel++;
			this.SendCommand(new ShowLevelCommand(Consts.Defense, propertyLevelTask.Result.defenseLevel.baseLevel, propertyLevelTask.Result.defenseLevel.currentLevel, propertyLevelTask.Result.defenseLevel.maxLevel));
			Task task = _roleSystem.UpdatePropertyLevel(roleId);
			yield return new WaitUntil(() => task.IsCompleted);

			//TODO: 处理当前状态
		}
	}

	IEnumerator CooldownReductionUpgradeAsync()
	{
		string roleId = _globalSystem.GlobalModel.RoleJsons[tableId].roleId;
		Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);
		yield return new WaitUntil(() => propertyLevelTask.IsCompleted);
		if (propertyLevelTask.Result.cooldownReductionLevel.currentLevel < propertyLevelTask.Result.cooldownReductionLevel.maxLevel)
		{
			propertyLevelTask.Result.cooldownReductionLevel.currentLevel++;
			this.SendCommand(new ShowLevelCommand(Consts.CooldownReduction, propertyLevelTask.Result.cooldownReductionLevel.baseLevel, propertyLevelTask.Result.cooldownReductionLevel.currentLevel, propertyLevelTask.Result.cooldownReductionLevel.maxLevel));
			Task task = _roleSystem.UpdatePropertyLevel(roleId);
			yield return new WaitUntil(() => task.IsCompleted);

			//TODO: 处理当前状态
		}
	}

	#endregion

	#region 协程

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

	}

	IEnumerator ToLeftAsync()
	{
		if (tableId == 0) tableId = _globalSystem.GlobalModel.RoleJsons.Length - 1;
		else
		{
			tableId--;
		}

		string roleId = _globalSystem.GlobalModel.RoleJsons[tableId].roleId;
		Task<RoleController> roleTask = _roleSystem.GetRole(roleId);
		Task<InfoJson> infoTask = _roleSystem.GetInfo(roleId);
		Task<LockJson> lockTask = _roleSystem.GetLock(roleId);
		Task total = Task.WhenAll(roleTask, infoTask, lockTask);
		yield return new WaitUntil(() => total.IsCompleted);

		RoleController leftCharacter = roleTask.Result;
		leftCharacter.transform.position = _view.Left.position;
		RoleController temp = character;
		character = leftCharacter;

		bool isUnLock = lockTask.Result.isUnlock;
		if (isUnLock)
		{
			Task<StarLevelJson> starLevelTask = _roleSystem.GetStarLevel(roleId);
			Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);
			Task levelTask = Task.WhenAll(starLevelTask, propertyLevelTask);
			yield return new WaitUntil(() => levelTask.IsCompleted);

			this.SendCommand(new ShowLevelCommand(Consts.Star, starLevelTask.Result.starLevel.baseLevel, starLevelTask.Result.starLevel.currentLevel, starLevelTask.Result.starLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.Health, propertyLevelTask.Result.healthLevel.baseLevel, propertyLevelTask.Result.healthLevel.currentLevel, propertyLevelTask.Result.healthLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.Energy, propertyLevelTask.Result.energyLevel.baseLevel, propertyLevelTask.Result.energyLevel.currentLevel, propertyLevelTask.Result.energyLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.Defense, propertyLevelTask.Result.defenseLevel.baseLevel, propertyLevelTask.Result.defenseLevel.currentLevel, propertyLevelTask.Result.defenseLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.CooldownReduction, propertyLevelTask.Result.cooldownReductionLevel.baseLevel, propertyLevelTask.Result.cooldownReductionLevel.currentLevel, propertyLevelTask.Result.cooldownReductionLevel.maxLevel));
		}
		else
		{

		}

		_view.SetRoleLockActive(false);

		Sequence mainSequence = DOTween.Sequence();
		Sequence oldSequence = DOTween.Sequence();
		oldSequence.Append(temp.transform.DORotate(new Vector3(0, -90, 0), 0.5f));
		oldSequence.Join(_view.SwitchSequence(false));
		oldSequence.Join(_view.PropertySequence(false));
		oldSequence.Append(temp.transform.DOMoveX(_view.Right.position.x, 1f));
		oldSequence.Append(temp.transform.DORotate(new Vector3(0, 0, 0), 0.5f));
		Sequence newSequence = DOTween.Sequence();
		newSequence.Append(leftCharacter.transform.DORotate(new Vector3(0, -90, 0), 0.5f));
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
		if (tableId == _globalSystem.GlobalModel.RoleJsons.Length - 1) tableId = 0;
		else tableId++;
		string roleId = _globalSystem.GlobalModel.RoleJsons[tableId].roleId;
		Task<RoleController> task = _roleSystem.GetRole(roleId);
		Task<LockJson> lockTask = _roleSystem.GetLock(roleId);

		Task total = Task.WhenAll(task, lockTask);
		yield return new WaitUntil(() => total.IsCompleted);
		bool isUnLock = lockTask.Result.isUnlock;

		if (isUnLock)
		{
			Task<StarLevelJson> starLevelTask = _roleSystem.GetStarLevel(roleId);
			Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);
			Task levelTask = Task.WhenAll(starLevelTask, propertyLevelTask);
			yield return new WaitUntil(() => levelTask.IsCompleted);

			this.SendCommand(new ShowLevelCommand(Consts.Star, starLevelTask.Result.starLevel.baseLevel, starLevelTask.Result.starLevel.currentLevel, starLevelTask.Result.starLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.Health, propertyLevelTask.Result.healthLevel.baseLevel, propertyLevelTask.Result.healthLevel.currentLevel, propertyLevelTask.Result.healthLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.Energy, propertyLevelTask.Result.energyLevel.baseLevel, propertyLevelTask.Result.energyLevel.currentLevel, propertyLevelTask.Result.energyLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.Defense, propertyLevelTask.Result.defenseLevel.baseLevel, propertyLevelTask.Result.defenseLevel.currentLevel, propertyLevelTask.Result.defenseLevel.maxLevel));
			this.SendCommand(new ShowLevelCommand(Consts.CooldownReduction, propertyLevelTask.Result.cooldownReductionLevel.baseLevel, propertyLevelTask.Result.cooldownReductionLevel.currentLevel, propertyLevelTask.Result.cooldownReductionLevel.maxLevel));
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
		oldSequence.Append(temp.transform.DORotate(new Vector3(0, 90, 0), 0.5f));
		oldSequence.Join(_view.SwitchSequence(false));
		oldSequence.Join(_view.PropertySequence(false));
		oldSequence.Append(temp.transform.DOMoveX(_view.Left.position.x, 1f));
		oldSequence.Append(temp.transform.DORotate(new Vector3(0, 0, 0), 0.5f));
		Sequence newSequence = DOTween.Sequence();
		newSequence.Append(rightCharacter.transform.DORotate(new Vector3(0, 90, 0), 0.5f));
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



	protected override void OnDeInit()
	{
		base.OnDeInit();

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





