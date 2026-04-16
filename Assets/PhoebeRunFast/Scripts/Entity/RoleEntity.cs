using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

/// <summary>
/// 角色实体
/// </summary>
public class RoleEntity : IEntity
{
	//RoleDatas

	/// <summary>
	/// 是否忙碌
	/// </summary>
	public bool isBusy;

	/// <summary>
	/// 获取架构实例
	/// </summary>
	/// <returns>架构实例</returns>
	public IArchitecture GetArchitecture()
	{
		return PhoebeRunFast.Interface;
	}
}
