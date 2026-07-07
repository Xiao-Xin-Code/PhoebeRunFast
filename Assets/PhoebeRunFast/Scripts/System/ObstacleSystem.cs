using System;
using System.Collections.Generic;
using UnityEngine;
using QMVC;

public class ObstacleSystem : AbstractSystem
{
	Dictionary<string, MonoPool<ObstacleController>> obstaclePools = new Dictionary<string, MonoPool<ObstacleController>>();

    public Transform obstacleParent;



    /// <summary>
    /// 获取障碍物控制器
    /// </summary>
    /// <param name="obstacleType">障碍物类型</param>
    /// <returns>障碍物控制器</returns>
    public ObstacleController GetObstacle(string obstacleType)
    {
        if(!obstaclePools.ContainsKey(obstacleType))
        {
            MonoPool<ObstacleController> pool = new MonoPool<ObstacleController>(Resources.Load<ObstacleController>(obstacleType), obstacleParent);
            obstaclePools.Add(obstacleType, pool);
        }
        return obstaclePools[obstacleType].Get();
    }

    /// <summary>
    /// 回收障碍物控制器
    /// </summary>
    /// <param name="obstacle">障碍物控制器</param>
    public void RecycleObstacle(ObstacleController obstacle)
    {
        obstaclePools[obstacle.obstacleType].Recycle(obstacle);
    }


	protected override void OnInit()
	{
		obstacleParent = new GameObject("ObstaclesParent").transform;
	}

}