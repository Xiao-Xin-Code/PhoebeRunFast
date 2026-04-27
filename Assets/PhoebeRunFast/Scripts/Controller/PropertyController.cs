using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

public class PropertyController : BaseController
{
	[SerializeField] PropertyView _view;

	protected override void OnInit()
	{
		base.OnInit();
		_view.StateInit();

		this.RegisterEvent<SetRoleStarEvent>(OnSetRoleStar);
		this.RegisterEvent<SetRolePropertyEvent>(OnSetRoleProperty);

		gameObject.SetActive(false);
	}




	private void OnSetRoleStar(SetRoleStarEvent evt)
	{

	}

	private void OnSetRoleProperty(SetRolePropertyEvent evt)
	{

	}
}
