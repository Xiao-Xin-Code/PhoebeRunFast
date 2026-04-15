using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

public class FlyBirdEntity : IEntity
{
	public IArchitecture GetArchitecture()
	{
		return PhoebeRunFast.Interface;
	}

	public BindableProperty<FlyBirdState> FlyBirdState = new BindableProperty<FlyBirdState>(global::FlyBirdState.Ready);


	public float globalPipeSpeed = 10f;

}
