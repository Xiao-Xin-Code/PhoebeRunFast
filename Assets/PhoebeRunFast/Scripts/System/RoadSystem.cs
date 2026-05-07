using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

public class RoadSystem : AbstractSystem
{

	Dictionary<string, MonoPool<RoadController>> roadPools = new Dictionary<string, MonoPool<RoadController>>();

	//当前使用的
	List<RoadController> roads = new List<RoadController>();


	protected override void OnInit()
	{
		
	}



	private void Spawn()
	{

	}




	IEnumerator RoadAsync()
	{
		while (true)
		{
			//获取距离最后的距离
			//等待生成
			//等待回收

			yield return null;
		}
	}

}
