using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

public class RoleEntity : IEntity
{
	//RoleDatas


	public bool isBusy;


	public IArchitecture GetArchitecture()
	{
		return PhoebeRunFast.Interface;
	}
}
