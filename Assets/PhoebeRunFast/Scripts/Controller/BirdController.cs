using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

public class BirdController : BaseController
{
	[SerializeField] BirdView _view;

	[SerializeField] FlyBirdController flyBird;


	protected override void OnInit()
	{
		base.OnInit();

		this.RegisterEvent<BirdInputDownEvent>(OnBirdUp);
	}




	void OnBirdUp(BirdInputDownEvent evt)
	{
		if(flyBird.FlyBirdState == FlyBirdState.Run)
		{
			_view.RB.velocity = Vector2.up * 600f;
		}
	}


	public void StateInit()
	{
		_view.RB.velocity = Vector2.zero;
		_view.RectTransform.anchoredPosition = Vector2.zero;
	}


	public void SetIsKinematic(bool isKinematic)
	{
		_view.RB.isKinematic = isKinematic;
	}



	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Pipe" || collision.tag == "Boundary")
		{
			flyBird.FlyBirdState = FlyBirdState.Over;
		}
	}


}
