using System.Collections;
using System.Collections.Generic;
using Frame;
using QMVC;
using UnityEngine;

public class RoadSystem : AbstractSystem
{
	GlobalSystem _globalSystem;

	Dictionary<string, MonoPool<RoadController>> roadPools = new Dictionary<string, MonoPool<RoadController>>();

	//当前使用的
	List<RoadController> roads = new List<RoadController>();

	public Transform roadParent;


	protected override void OnInit()
	{
		_globalSystem = this.GetSystem<GlobalSystem>();
		roadParent = new GameObject("RoadsParent").transform;
		RoadController road = Resources.Load<RoadController>("Road");
		roadPools.Add("Road", new MonoPool<RoadController>(road, roadParent));
	}



	private void Spawn()
	{
		//对象池生成
		RoadController road = roadPools["Road"].Get();
		road.transform.position = roads.Count > 0 ? roads[roads.Count - 1].transform.position + Vector3.forward * road.distance : Vector3.zero;
		roads.Add(road);
	}

	public void StartRoad()
	{
		MonoService.Instance.StartCoroutine(RoadAsync());
	}


	IEnumerator RoadAsync()
	{
		while (true)
		{
			if (_globalSystem.GameSingleton.GameEntity.GameState.Value == GameState.Running) 
			{
				//获取距离最后的距离
				float lastDistance = roads.Count > 0 ? Vector3.Distance(roads[roads.Count - 1].transform.position, _globalSystem.GameSingleton.PlayerController.transform.position) : 0;
				//等待生成
				if (lastDistance < 50)
				{
					Spawn();
					yield return null;
				}
				//等待回收
				while (roads.Count > 0)
				{
					RoadController road = roads[0];
					float firstDistance = Vector3.Distance(_globalSystem.GameSingleton.PlayerController.transform.position, roads[0].transform.position);
					if (firstDistance < 50)
					{
						break;
					}
					roads.RemoveAt(0);
					roadPools["Road"].Recycle(road);
					yield return null;
				}
			}
			yield return null;
		}
	}

}
