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
	}

	Coroutine roadCoroutine;

	private void Spawn()
	{
		//对象池生成
		RoadController road = GetRoad(GetRandomRoadType());
		road.transform.position = roads.Count > 0 ? roads[roads.Count - 1].transform.position + Vector3.forward * road.distance : Vector3.zero;
		roads.Add(road);
		road.Spawn();
		Debug.Log($"生成道路{road.roadType},当前道路数量：{roads.Count}");
	}

	public void StartRoad()
	{
		if(roadCoroutine != null)
		{
			MonoService.Instance.StopCoroutine(roadCoroutine);
		}
		roadCoroutine = MonoService.Instance.StartCoroutine(RoadAsync());
	}


	IEnumerator RoadAsync()
	{
		while (true)
		{
			if (_globalSystem.GameSingleton.GameEntity.GameState.Value == GameState.Running) 
			{
				//获取距离最后的距离
				float lastDistance = roads.Count > 0 ? Vector3.Distance(roads[roads.Count - 1].transform.position, _globalSystem.GameSingleton.PlayerController.transform.position) : 0;
				yield return null;
				//等待生成
				if (lastDistance < 50)
				{
					Spawn();
					yield return null;
				}
				Debug.Log($"道路数量：{roads.Count}");
				//等待回收
				while (roads.Count > 0)
				{
					RoadController road = roads[0];
					float firstDistance = Mathf.Abs(_globalSystem.GameSingleton.PlayerController.transform.position.z - roads[0].transform.position.z);
					if (firstDistance < 50)
					{
						break;
					}
					Debug.Log($"回收道路{road.roadType}");
					roads.RemoveAt(0);
					roadPools[road.roadType].Recycle(road);
					Debug.Log($"回收后道路数量：{roads.Count}");
					yield return null;
				}
			}
			yield return null;
		}
	}

	public void StopRoad()
	{
		if(roadCoroutine != null)
		{
			MonoService.Instance.StopCoroutine(roadCoroutine);
		}
	}

	public Coroutine InitRoad()
	{
		return MonoService.Instance.StartCoroutine(InitRoadAsync());
	}

	/// <summary>
	/// 加载最初始道路，或是一个固定的开始道路场景
	/// </summary>
	IEnumerator InitRoadAsync()
	{
		float lastDistance = roads.Count > 0 ? Mathf.Abs(_globalSystem.GameSingleton.PlayerController.transform.position.z - roads[roads.Count - 1].transform.position.z) : 0;
		while(lastDistance < 50)
		{
			Spawn();
			lastDistance = roads.Count > 0 ? Mathf.Abs(_globalSystem.GameSingleton.PlayerController.transform.position.z - roads[roads.Count - 1].transform.position.z) : 0;
			yield return null;
		}
	}


	private RoadController GetRoad(string roadType)
	{
		if(!roadPools.ContainsKey(roadType))
		{
			MonoPool<RoadController> pool = new MonoPool<RoadController>(Resources.Load<RoadController>(roadType), roadParent);
			roadPools.Add(roadType, pool);
		}
		return roadPools[roadType].Get();	
	}

	//随机获取道路的规则
	private string GetRandomRoadType()
	{
		string[] roadTypes = {"Road_Empty","Road_001"};
		int index = Random.Range(0, roadTypes.Length);
		return roadTypes[index];
	}


	public void RecycleAllRoad()
	{
		foreach (var road in roads)
		{
			roadPools[road.roadType].Recycle(road);
		}
		roads.Clear();
	}


}
