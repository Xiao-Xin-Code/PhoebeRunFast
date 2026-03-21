using System.Collections;
using System.Collections.Generic;
using Frame;
using QMVC;
using UnityEngine;

public class RoadSystem : AbstractSystem
{
	List<MonoPool<RoadController>> roadPools = new List<MonoPool<RoadController>>();


	protected override void OnInit()
	{
		//根据不同的Road创建不同的Pool


		Debug.Log("Roads");
		pool = new MonoPool<RoadController>(Resources.Load<RoadController>("Prefabs/Roads/Road"));


		for (int i = 0; i < 3; i++)
		{
			SpawnRoad();
		}


		MonoService.Instance.AddUpdateListener(RoadSystemRun);
	}

	MonoPool<RoadController> pool;

	public PlayerController playerController;

	public Vector3 curSpawn;
	float spawnDistance = 50;

	List<RoadController> roadControllers = new List<RoadController>();

	//随机生成Road


	private void SpawnRoad()
	{
		RoadController road = pool.Get();

		Vector3 spawnPosition = curSpawn;
		Quaternion spawnRotation = Quaternion.identity;

		if (roadControllers.Count > 0)
		{
			RoadController last = roadControllers[roadControllers.Count - 1];

			spawnPosition = last.endPoint.position;
			spawnRotation = last.endPoint.rotation;
		}

		road.transform.position = spawnPosition;
		road.transform.rotation = spawnRotation;
		roadControllers.Add(road);
		curSpawn = road.endPoint.position;

		road.Spawn();
	}


	private float GetDistanceFromPlayerToLast()
	{
		if (roadControllers.Count == 0) return 0;
		RoadController last = roadControllers[roadControllers.Count - 1];
		//Get Player的位置
		return Vector3.Distance(playerController.transform.position, last.endPoint.position);
	}

	private void CheckAndDespawn()
	{
		while (roadControllers.Count > 0)
		{
			RoadController first = roadControllers[0];
			float distance = Vector3.Distance(playerController.transform.position, first.startPoint.position);


			if (distance > spawnDistance)
			{
				roadControllers.RemoveAt(0);
				pool.Recycle(first);
			}
			else
			{
				break;
			}

		}
	}



	private void RoadSystemRun()
	{
		var distance = GetDistanceFromPlayerToLast();
		if(distance< spawnDistance)
		{
			SpawnRoad();
		}

		CheckAndDespawn();
	}




	private void SpawnObstacleAndCollectible()
	{
		//随机取占用数量
		
		//当占用全部时，需要随机取出至少一个可通过操作通过的，避免死路

		//其它情况下可以随意取

		//生成后需要更新之后的可生成范围，避免重叠生成

		//可随机取，需要判断此位置是否在可经过范围内
	}

	

}
