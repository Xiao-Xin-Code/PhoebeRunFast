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
	/// 角色对象池
	/// </summary>
	MonoPool<RoleController> characterPool;

	/// <summary>
	/// 当前角色
	/// </summary>
	RoleController character;

	[SerializeField] RoleController prefab;

	/// <summary>
	/// 对象池父物体
	/// </summary>
	Transform poolParent;

	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{
		base.OnInit();
		_globalSystem = this.GetSystem<GlobalSystem>();
		_roleSystem = this.GetSystem<RoleSystem>();

		_entity = new RoleSystemEntity();
		poolParent = new GameObject("Pool").transform;
		characterPool = new MonoPool<RoleController>(prefab, poolParent, 2);

		// 注册事件
		this.RegisterEvent<RoleMenuActiveEvent>(OnRoleMenuActive);
		this.RegisterEvent<ToLeftRoleEvent>(OnToLeftRole);
		this.RegisterEvent<ToRightRoleEvent>(OnToRightRole);
		this.RegisterEvent<InitCharacterEvent>(OnInitCharacter);
	}

	/// <summary>
	/// 开始时初始化角色
	/// </summary>
	private void Start()
	{
		//CharacterInit();
	}

	/// <summary>
	/// 初始化角色
	/// </summary>
	private void CharacterInit()
	{
		//初始显示
		RoleController character = characterPool.Get();
		character.transform.position = _view.Center.position;
		this.character = character;
	}

	/// <summary>
	/// 角色移动到目标位置
	/// </summary>
	private void ToTarget()
	{
		RoleController targetCharacter = characterPool.Get();
		targetCharacter.transform.position = _view.Left.position;
		Sequence mainSequence = DOTween.Sequence();
		Sequence sequence1 = DOTween.Sequence();
		sequence1.Append(character.transform.DORotate(new Vector3(0, -90, 0), 0.5f));
		Sequence sequence2 = _view.PropertySequence(false);
		sequence1.Join(sequence2);
		sequence1.Append(character.transform.DOMoveX(_view.Right.position.x, 2f));
		Sequence sequence3 = DOTween.Sequence();
		sequence3.Append(targetCharacter.transform.DORotate(new Vector3(0, -90, 0), 0.5f));
		sequence3.Append(targetCharacter.transform.DOMoveX(_view.Center.position.x, 2f));
		sequence3.Append(targetCharacter.transform.DORotate(new Vector3(0, 0, 0), 0.5f));
		Sequence sequence4 = _view.PropertySequence(true);
		sequence3.Join(sequence4);
		mainSequence.Join(sequence1);
		mainSequence.Join(sequence3);
		mainSequence.OnComplete(() => _entity.isBusy = false);
		mainSequence.Play();
	}

	/// <summary>
	/// 切换到左侧角色
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnToLeftRole(ToLeftRoleEvent evt)
	{
		if (_entity.isBusy) return;
		_entity.isBusy = true;
		MonoService.Instance.StartCoroutine(ToLeftAsync());
		//RoleController leftCharacter = characterPool.Get();
		//leftCharacter.transform.position = _view.Left.position;
		//RoleController temp = character;
		//character = leftCharacter;
		//Sequence mainSequence = DOTween.Sequence();
		//Sequence oldSequence = DOTween.Sequence();
		//oldSequence.Append(temp.transform.DORotate(new Vector3(0, -90, 0), 0.5f));
		//oldSequence.Join(RoleMenuSequence(false));
		//oldSequence.Append(temp.transform.DOMoveX(_view.Right.position.x, 1f));
		//oldSequence.Append(temp.transform.DORotate(new Vector3(0, 0, 0), 0.5f));
		//Sequence newSequence = DOTween.Sequence();
		//newSequence.Append(leftCharacter.transform.DORotate(new Vector3(0, -90, 0), 0.5f));
		//newSequence.Append(leftCharacter.transform.DOMoveX(_view.Center.position.x, 1f));
		//newSequence.Append(leftCharacter.transform.DORotate(new Vector3(0, 0, 0), 0.5f));
		//newSequence.Join(RoleMenuSequence(true));
		//mainSequence.Join(oldSequence);
		//mainSequence.Join(newSequence);
		//mainSequence.OnComplete(() =>
		//{
		//	characterPool.Recycle(temp);
		//	_entity.isBusy = false;
		//});
		//mainSequence.Play();
	}

	/// <summary>
	/// 切换到右侧角色
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnToRightRole(ToRightRoleEvent evt)
	{
		if (_entity.isBusy) return;
		_entity.isBusy = true;
		MonoService.Instance.StartCoroutine(ToRightAsync());
		//RoleController rightCharacter = characterPool.Get();
		//rightCharacter.transform.position = _view.Right.position;
		//RoleController temp = character;
		//character = rightCharacter;
		//Sequence mainSequence = DOTween.Sequence();
		//Sequence oldSequence = DOTween.Sequence();
		//oldSequence.Append(temp.transform.DORotate(new Vector3(0, 90, 0), 0.5f));
		//oldSequence.Join(RoleMenuSequence(false));
		//oldSequence.Append(temp.transform.DOMoveX(_view.Left.position.x, 1f));
		//oldSequence.Append(rightCharacter.transform.DORotate(new Vector3(0, 0, 0), 0.5f));
		//Sequence newSequence = DOTween.Sequence();
		//newSequence.Append(rightCharacter.transform.DORotate(new Vector3(0, 90, 0), 0.5f));
		//newSequence.Append(rightCharacter.transform.DOMoveX(_view.Center.position.x, 1f));
		//newSequence.Append(rightCharacter.transform.DORotate(new Vector3(0, 0, 0), 0.5f));
		//newSequence.Join(RoleMenuSequence(true));
		//mainSequence.Join(oldSequence);
		//mainSequence.Join(newSequence);
		//mainSequence.OnComplete(() =>
		//{
		//	characterPool.Recycle(temp);
		//	_entity.isBusy = false;
		//});
		//mainSequence.Play();
	}

	/// <summary>
	/// 角色菜单动画序列
	/// </summary>
	/// <param name="isOpen">是否打开</param>
	/// <returns>动画序列</returns>
	Sequence RoleMenuSequence(bool isOpen)
	{
		Sequence mainSequence = DOTween.Sequence();
		Sequence switchSequence = _view.SwitchSequence(isOpen);
		Sequence propertySequence = _view.PropertySequence(isOpen);
		mainSequence.Join(switchSequence);
		mainSequence.Join(propertySequence);
		return mainSequence;
	}

	/// <summary>
	/// 角色菜单激活事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnRoleMenuActive(RoleMenuActiveEvent evt)
	{
		_view.SetSwitchActive(evt.isActive);
		_view.SetPropertyActive(evt.isActive);
		tableId = _globalSystem.GlobalModel.OutRoleTableId.Value;
		MonoService.Instance.StartCoroutine(ToTargetAsync(tableId));
	}

	int tableId = 0;


	private void OnInitCharacter(InitCharacterEvent evt)
	{
		MonoService.Instance.StartCoroutine(ToTargetAsync(evt.tableId));
		this.tableId = evt.tableId;
	}


	IEnumerator ToTargetAsync(int index)
	{
		if (character)
		{
			_roleSystem.RecycleRole(character);
			character = null;
		}
		Task<RoleController> task = _roleSystem.GetRole(_roleSystem.RoleArray[index].roleId);
		yield return new WaitUntil(() => task.IsCompleted);
		RoleController controller = task.Result;
		character = controller;
		character.transform.position = _view.Center.position;
	}

	IEnumerator ToLeftAsync()
	{
		if (tableId == 0) tableId = _roleSystem.RoleArray.Length - 1;
		else
		{
			tableId--;
		}
		Task<RoleController> task = _roleSystem.GetRole(_roleSystem.RoleArray[tableId].roleId);
		yield return new WaitUntil(() => task.IsCompleted);
		RoleController leftCharacter = task.Result;
		leftCharacter.gameObject.SetActive(true);
		leftCharacter.transform.position = _view.Left.position;
		RoleController temp = character;
		character = leftCharacter;
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
		if (tableId == _roleSystem.RoleArray.Length - 1) tableId = 0;
		else tableId++;
		Task<RoleController> task = _roleSystem.GetRole(_roleSystem.RoleArray[tableId].roleId);
		Task<LockJson> lockTask = _roleSystem.GetLock(_roleSystem.RoleArray[tableId].roleId);

		Task total = Task.WhenAll(task, lockTask);
		yield return new WaitUntil(() => total.IsCompleted);
		Debug.Log($"{_roleSystem.RoleArray[tableId].roleId},{lockTask.Result.isUnlock}");
		bool isUnLock = lockTask.Result.isUnlock;

		if (isUnLock)
		{
			//TODO: 获取属性信息
			Task<PropertyJson> propertyTask = _roleSystem.GetProperty(_roleSystem.RoleArray[tableId].roleId);
			Task<PropertyLevelJson> propertyLevelTask = _roleSystem.GetPropertyLevel(_roleSystem.RoleArray[tableId].roleId);
			Task<StarLevelJson> starLevelTask = _roleSystem.GetStarLevel(_roleSystem.RoleArray[tableId].roleId);

			//TODO: 判断等级，如果还可以提升，就需要读取升级代价信息
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
		oldSequence.Join(RoleMenuSequence(false));
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





