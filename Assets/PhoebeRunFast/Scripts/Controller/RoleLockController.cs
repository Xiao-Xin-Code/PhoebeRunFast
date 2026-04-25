using UnityEngine;
using QMVC;

public class RoleLockController : BaseController
{

	[SerializeField] RoleLockView _view;


	protected override void OnInit()
	{
		base.OnInit();

		_view.StateInit();
		gameObject.SetActive(false);
	}
}
