using System;
using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;
using UnityEngine.EventSystems;

public class BirdInputController : BaseController
{

	[SerializeField] BirdInputView _view;


	protected override void OnInit()
	{
		base.OnInit();

		_view.RegisterPointerDown(OnPointerDown);
	}


	
	private void OnPointerDown(PointerEventData eventData)
	{
		this.SendCommand<BirdInputDownCommand>();
	}


}


public class BirdInputDownEvent
{

}


public class BirdInputDownCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<BirdInputDownEvent>();
	}
}
