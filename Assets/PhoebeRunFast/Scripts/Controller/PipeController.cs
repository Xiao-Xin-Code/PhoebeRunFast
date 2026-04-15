using System.Collections;
using System.Collections.Generic;
using Codice.CM.Common;
using Frame;
using UnityEngine;

public class PipeController : BaseController
{
	[SerializeField] PipeView _view;

	[SerializeField] static FlyBirdController _flyBird;


	protected override void OnInit()
	{
		base.OnInit();


	}

	public void SetFlyBird(FlyBirdController flyBird)
	{
		_flyBird = flyBird;
	}


	public void OnPipeMove()
	{
		if (_flyBird.FlyBirdState != FlyBirdState.Run) return;
		_view.RectTransform.anchoredPosition += Vector2.left * 200 * Time.deltaTime;
		if (_view.RectTransform.position.x < _flyBird.PipeRecyclePoint.position.x) 
		{
			MonoService.Instance.RemoveUpdateListener(OnPipeMove);
			//回收
			_flyBird.PipePool.Recycle(this);
		}
	}



	protected override void OnDeInit()
	{
		base.OnDeInit();
		MonoService.Instance?.RemoveUpdateListener(OnPipeMove);
	}

}
