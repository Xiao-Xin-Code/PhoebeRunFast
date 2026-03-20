using System;
using System.Collections.Generic;
using UnityEngine;


public class RoadController : BaseController
{
	protected override void Init()
	{
	
		

	}

	[Serializable]
	public struct RoadLine
	{
		public List<Transform> points;
	}

	public float length = 20;
	public Transform startPoint;
	public Transform endPoint;

	public List<ObstacleController> obstacles = new List<ObstacleController>();
	public List<CollectibleController> collectibles = new List<CollectibleController>();

	public List<RoadLine> points = new List<RoadLine>();



	private void OnDrawGizmos()
	{
		// 可视化起点和终点
		if (startPoint != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(startPoint.position, 0.5f);
		}

		if (endPoint != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(endPoint.position, 0.5f);
		}

		// 可视化障碍物生成点
		Gizmos.color = Color.yellow;
		foreach (var point in obstacles)
		{
			if (point != null)
				Gizmos.DrawWireCube(point.transform.position, Vector3.one * 0.3f);
		}

		// 可视化收集物生成点
		Gizmos.color = Color.cyan;
		foreach (var point in collectibles)
		{
			if (point != null)
				Gizmos.DrawWireSphere(point.transform.position, 0.2f);
		}


		foreach (var item in points)
		{
			foreach (var point in item.points)
			{
				if (point != null)
					Gizmos.DrawWireSphere(point.transform.position, 0.2f);
			}

		}
	}



	//放置期望

	//当前位置
	//上一次放置位置（占用的最大位置）

	//设置间隔

	//根据期望设置
	//1，期望放置时：根据当前位置与上一次位置判断间隔是否允许
	//允许的话就随机放置模式，之后更新上一次放置位置，放置时需要判断剩余的位置是否可以放下当前的生成物，如果无法放置时就重新生成，多次后无法放置就不进行放置
	//
	//2，不期望时直接进行下一个




	public void Spawn()
	{
		int maxLength = 10;
		int minSpace = 2;
		//当前
		int cur = 0;
		//上一次放置
		int last = 5;

		while (cur < maxLength) 
		{
			//直接更新到后面的位置
			if (cur < last)
			{
				cur = last + 1;
			}
			if (cur < maxLength)
			{
				//取期望
				bool expect = true;

				int space = cur - last;

				if (space > minSpace)
				{
					if (expect)
					{
						//生成
						//更新last与cur
						//cur = last+1;
					}
					else
					{
						cur++;
					}
				}
				else
				{
					cur++;	
				}
			}
		}

		
		

		




	}


}
