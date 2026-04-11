using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

public class SwitchEntity : IEntity
{
	public IArchitecture GetArchitecture()
	{
		return PhoebeRunFast.Interface;
	}






}
