using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

/// <summary>
/// 角色系统数据
/// </summary>
public class RoleSystemEntity : IEntity
{
	//RoleDatas

	/// <summary>
	/// 是否忙碌
	/// </summary>
	public bool isBusy;


	//先获取用户持有的角色信息，然后获取用户持有的角色信息
	//然后先填入用户持有的角色ID，然后根据RoleJson依次填入

	public int outRoleIndex = -1;

	public List<string> roleIds = new List<string>();




	/// <summary>
	/// 获取架构实例
	/// </summary>
	/// <returns>架构实例</returns>
	public IArchitecture GetArchitecture()
	{
		return PhoebeRunFast.Interface;
	}
}
