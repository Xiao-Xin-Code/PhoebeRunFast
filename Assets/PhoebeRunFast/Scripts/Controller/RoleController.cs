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
		this.RegisterEvent<ToLeftRoleEvent>(OnToLeftRole);
		this.RegisterEvent<ToRightRoleEvent>(OnToRightRole);
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




	private void OnToLeftRole(ToLeftRoleEvent evt)
	{
		if (_entity.isBusy) return;
		_entity.isBusy = true;
		CharacterController leftCharacter = characterPool.Get();
		leftCharacter.transform.position = _view.Left.position;
		CharacterController temp = character;
		character = leftCharacter;
		Sequence mainSequence = DOTween.Sequence();
		Sequence oldSequence = DOTween.Sequence();
		oldSequence.Append(temp.transform.DORotate(new Vector3(0, -90, 0), 0.5f));
		oldSequence.Join(RoleMenuSequence(false));
		oldSequence.Append(temp.transform.DOMoveX(_view.Right.position.x, 1f));
		oldSequence.Append(temp.transform.DORotate(new Vector3(0, 0, 0), 0.5f));
		Sequence newSequence = DOTween.Sequence();
		newSequence.Append(leftCharacter.transform.DORotate(new Vector3(0, -90, 0), 0.5f));
		newSequence.Append(leftCharacter.transform.DOMoveX(_view.Center.position.x, 1f));
		newSequence.Append(leftCharacter.transform.DORotate(new Vector3(0, 0, 0), 0.5f));
		newSequence.Join(RoleMenuSequence(true));
		mainSequence.Join(oldSequence);
		mainSequence.Join(newSequence);
		mainSequence.OnComplete(() =>
		{
			characterPool.Recycle(temp);
			_entity.isBusy = false;
		});
		mainSequence.Play();
	}

	private void OnToRightRole(ToRightRoleEvent evt)
	{
		if (_entity.isBusy) return;
		_entity.isBusy = true;
		CharacterController rightCharacter = characterPool.Get();
		rightCharacter.transform.position = _view.Right.position;
		CharacterController temp = character;
		character = rightCharacter;
		Sequence mainSequence = DOTween.Sequence();
		Sequence oldSequence = DOTween.Sequence();
		oldSequence.Append(temp.transform.DORotate(new Vector3(0, 90, 0), 0.5f));
		oldSequence.Join(RoleMenuSequence(false));
		oldSequence.Append(temp.transform.DOMoveX(_view.Left.position.x, 1f));
		oldSequence.Append(rightCharacter.transform.DORotate(new Vector3(0, 0, 0), 0.5f));
		Sequence newSequence = DOTween.Sequence();
		newSequence.Append(rightCharacter.transform.DORotate(new Vector3(0, 90, 0), 0.5f));
		newSequence.Append(rightCharacter.transform.DOMoveX(_view.Center.position.x, 1f));
		newSequence.Append(rightCharacter.transform.DORotate(new Vector3(0, 0, 0), 0.5f));
		newSequence.Join(RoleMenuSequence(true));
		mainSequence.Join(oldSequence);
		mainSequence.Join(newSequence);
		mainSequence.OnComplete(() =>
		{
			characterPool.Recycle(temp);
			_entity.isBusy = false;
		});
		mainSequence.Play();
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





