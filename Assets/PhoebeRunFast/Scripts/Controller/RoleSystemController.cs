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
		character.transform.position = _view.Center.position;
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

			//TODO: 设置星级属性
			this.SendCommand(new SetRoleStarCommand(starLevelTask.Result.starLevel.currentLevel));
			this.SendCommand(new SetRolePropertyCommand(propertyLevelTask.Result.healthLevel.currentLevel, propertyLevelTask.Result.energyLevel.currentLevel, propertyLevelTask.Result.defenseLevel.currentLevel, propertyLevelTask.Result.cooldownReductionLevel.currentLevel));
			
		}
		else
		{

		}



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
		newSequence.Join(_view.PropertySequence(true));
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
			//TODO: 获取属性信息
			Task<PropertyJson> propertyTask = _roleSystem.GetProperty(roleId);
			Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(roleId);
			Task<StarLevelJson> starLevelTask = _roleSystem.GetStarLevel(roleId);

			//TODO: 判断等级，如果还可以提升，就需要读取升级代价信息

			Task levelTask = Task.WhenAll(propertyTask, starLevelTask, propertyLevelTask);
			yield return new WaitUntil(() => levelTask.IsCompleted);

			//TODO: 设置等级状态

			if(starLevelTask.Result.starLevel.currentLevel == starLevelTask.Result.starLevel.maxLevel)
			{

			}

			#region 属性设置

			if (propertyLevelTask.Result.healthLevel.currentLevel == propertyLevelTask.Result.healthLevel.maxLevel)
			{

			}
			if (propertyLevelTask.Result.energyLevel.currentLevel == propertyLevelTask.Result.energyLevel.maxLevel)
			{

			}
			if (propertyLevelTask.Result.defenseLevel.currentLevel == propertyLevelTask.Result.defenseLevel.maxLevel)
			{

			}
			if (propertyLevelTask.Result.cooldownReductionLevel.currentLevel == propertyLevelTask.Result.cooldownReductionLevel.maxLevel)
			{

			}

			#endregion

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

}





