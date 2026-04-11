using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QMVC;
using UnityEngine;

public class RoleController : BaseController
{
    [SerializeField] RoleView _view;

    RoleEntity _entity;

	MonoPool<CharacterController> characterPool;

	CharacterController character;

	[SerializeField] CharacterController prefab;

	Transform poolParent;

	protected override void OnInit()
	{
		base.OnInit();
		_entity = new RoleEntity();
		poolParent = new GameObject("Pool").transform;
		characterPool = new MonoPool<CharacterController>(prefab, poolParent, 2);

		this.RegisterEvent<RoleMenuActiveEvent>(OnRoleMenuActive);
	}

	private void Start()
	{
		CharacterInit();
	}


	private void CharacterInit()
	{
		//初始显示
		CharacterController character = characterPool.Get();
		character.transform.position = _view.Center.position;
		this.character = character;
	}



	private void ToTarget()
	{
		CharacterController targetCharacter = characterPool.Get();
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







	private void ToLeft()
	{
		if (_entity.isBusy) return;
		_entity.isBusy = true;

		CharacterController leftCharacter = characterPool.Get();
		//设置归属的形象

		leftCharacter.transform.position = _view.Left.position;
		

		//界面变化

		//character 移动

		//界面恢复
	}



	Sequence RoleMenuSequence(bool isOpen)
	{
		Sequence mainSequence = DOTween.Sequence();
		Sequence switchSequence = _view.SwitchSequence(isOpen);
		Sequence propertySequence = _view.PropertySequence(isOpen);
		mainSequence.Join(switchSequence);
		mainSequence.Join(propertySequence);
		return mainSequence;
	}


	private void OnRoleMenuActive(RoleMenuActiveEvent evt)
	{
		_view.SetSwitchActive(evt.isActive);
		_view.SetPropertyActive(evt.isActive);
	}

}




public class RoleMenuActiveCommand : AbstractCommand
{
	bool isActive;

	public RoleMenuActiveCommand(bool isActive)
	{
		this.isActive = isActive;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new RoleMenuActiveEvent(isActive));
	}
}
