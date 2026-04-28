using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnitController : BaseController
{
    [SerializeField] LevelUnitView _view;

    public BindableProperty<bool> IsOn = new BindableProperty<bool>(false);

	protected override void OnInit()
	{
		base.OnInit();

		IsOn.RegisterWithInitValue(_view.SetIconState);
	}


}
