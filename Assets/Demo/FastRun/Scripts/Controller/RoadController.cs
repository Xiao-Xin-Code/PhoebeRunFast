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
		public List<Transform> areaPoints;
		public ObstacleController[] obstacles;
		public bool[] canTos;
	}

	public float length = 20;
	public Transform startPoint;
	public Transform endPoint;

	public List<ObstacleController> obstacles = new List<ObstacleController>();
	public List<CollectibleController> collectibles = new List<CollectibleController>();

	public RoadLine[] roadLines = new RoadLine[] { };



	public ObstacleController fullObstacle;
	public ObstacleController halfObstacle;



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


		foreach (var item in roadLines)
		{
			foreach (var point in item.areaPoints)
			{
				if (point != null)
					Gizmos.DrawWireSphere(point.transform.position, 0.2f);
			}

		}
	}


	public void Spawn()
	{
		int minSpace = 2;

		NonRepeatRandom nonRepeat = new NonRepeatRandom(0, roadLines.Length - 1);
		int[] lastFrags = new int[roadLines.Length];

		for (int i = 0; i < lastFrags.Length; i++) 
		{
			lastFrags[i] = -minSpace;
		}

		for (int i = 2; i < length - 2; i++) 
		{
			nonRepeat.Reset();

			for(int j = 0; j < roadLines.Length; j++)
			{
				int curCol = nonRepeat.Next();
				bool canPlace = CheckPlace(i, curCol);
				if (!canPlace) 
				{
					if (roadLines[curCol].obstacles[i] != null)
					{
						Destroy(roadLines[curCol].obstacles[i].gameObject);
						roadLines[curCol].obstacles[i] = null;
					}
					continue;
				}
				if (i - lastFrags[curCol] < minSpace)
				{
					if (roadLines[curCol].obstacles[i] != null)
					{
						Destroy(roadLines[curCol].obstacles[i].gameObject);
						roadLines[curCol].obstacles[i] = null;
					}
					continue;
				}
				bool success = ProbabilityHelper.PercentSuccess(50);
				if (!success) 
				{
					if (roadLines[curCol].obstacles[i] != null)
					{
						Destroy(roadLines[curCol].obstacles[i].gameObject);
						roadLines[curCol].obstacles[i] = null;
					}
					continue;
				}

				int mode = UnityEngine.Random.Range(0, 2);
				if (roadLines[curCol].obstacles[i] != null)
				{
					Destroy(roadLines[curCol].obstacles[i].gameObject);
					roadLines[curCol].obstacles[i] = null;
				}

				ObstacleController temp = mode == 0 ? Instantiate(halfObstacle, roadLines[curCol].areaPoints[i].position, Quaternion.identity) : Instantiate(fullObstacle, roadLines[curCol].areaPoints[i].position, Quaternion.identity);
				roadLines[curCol].obstacles[i] = temp;
				lastFrags[curCol] = i;
				UpdateLineCanTos(i);
			}
			UpdateLineCanTos(i);
		}

	}




	private bool CheckPlace(int curRow,int curCol)
	{
		bool left = false;
		for (int i = 0; i < curCol; i++)
		{
			if (curRow > 0)
			{
				if (roadLines[i].canTos[curRow] && roadLines[i].canTos[curRow - 1]) left = true;
			}
			else
			{
				if (roadLines[i].canTos[curRow]) left = true;
			}
		}

		bool right = false;
		for (int i = curCol + 1; i < 3; i++)
		{
			if (curRow > 0)
			{
				if (roadLines[i].canTos[curRow] && roadLines[i].canTos[curRow - 1]) left = true;
			}
			else
			{
				if (roadLines[i].canTos[curRow]) left = true;
			}
		}
		return left || right;
	}


	private void UpdateLineCanTos(int row)
	{
		// 第一行
		if (row == 0)
		{
			for (int i = 0; i < 3; i++)
			{
				roadLines[i].canTos[row] = roadLines[i].obstacles[row] == null || roadLines[i].obstacles[row].mode != 1;
				Debug.Log($"行{row}列{i}: 障碍物={roadLines[i].obstacles[row]}, 可达={roadLines[i].canTos[row]}");
			}
			return;
		}

		for (int curCol = 0; curCol < 3; curCol++)
		{
			// 有障碍物（1），不可达
			if (roadLines[curCol].obstacles[row] != null && roadLines[curCol].obstacles[row].mode == 1) 
			{
				roadLines[curCol].canTos[row] = false;
				Debug.Log($"行{row}列{curCol}: 有障碍物({roadLines[curCol].obstacles[row]})，不可达");
				continue;
			}

			bool canReach = false;

			// 1. 检查上一行同列
			if (roadLines[curCol].canTos[row - 1])
			{
				canReach = true;
				Debug.Log($"行{row}列{curCol}: 上一行同列可达，直接可达");
			}

			// 2. 尝试从左侧横向移动过来
			if (!canReach)
			{
				bool hasLeft = false;
				for (int leftCol = curCol - 1; leftCol >= 0; leftCol--)
				{
					if (roadLines[leftCol].obstacles[row] != null) 
					{
						hasLeft = false;
						break;
					}
					else
					{
						if (roadLines[leftCol].canTos[row - 1])
						{
							hasLeft = true;
							break;
						}
						else
						{
							hasLeft = false;
							continue;
						}

					}
				}
				canReach = hasLeft;
			}

			// 3. 尝试从右侧横向移动过来
			if (!canReach)
			{
				bool hasRight = false;
				for (int rightCol = curCol + 1; rightCol < 3; rightCol++)
				{
					if (roadLines[rightCol].obstacles[row] != null) 
					{
						hasRight = false;
						break;
					}
					else
					{
						if (roadLines[rightCol].canTos[row - 1])
						{
							hasRight = true;
							break;
						}
						else
						{
							hasRight = false;
							continue;
						}
					}
				}
				canReach = hasRight;
			}

			roadLines[curCol].canTos[row] = canReach;

			if (!canReach)
			{
				Debug.Log($"行{row}列{curCol}: 无法从任何方向到达，不可达");
			}
		}
	}








	public class NonRepeatRandom
	{
		private List<int> pool;
		private List<int> originalPool;

		public NonRepeatRandom(int min, int max)
		{
			originalPool = new List<int>();
			for (int i = min; i <= max; i++)
			{
				originalPool.Add(i);
			}
			Reset();
		}

		public void Reset()
		{
			pool = new List<int>(originalPool);
		}

		public int Next()
		{
			if (pool.Count == 0)
			{
				Reset();
			}

			int index = UnityEngine.Random.Range(0, pool.Count);
			int value = pool[index];
			pool.RemoveAt(index);
			return value;
		}

		public int RemainingCount => pool.Count;
	}

	public static class ProbabilityHelper
	{
		/// <summary>
		/// 百分比成功率判定
		/// </summary>
		/// <param name="percent">0-100</param>
		/// <returns>是否成功</returns>
		public static bool PercentSuccess(float percent)
		{
			return UnityEngine.Random.Range(0f, 100f) < percent;
		}
	}

}