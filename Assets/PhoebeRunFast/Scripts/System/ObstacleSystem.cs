using System;
using System.Collections.Generic;
using QMVC;

public class ObstacleSystem : AbstractSystem
{
	Dictionary<Type, MonoPool<ObstacleController>> obstaclePools = new Dictionary<Type, MonoPool<ObstacleController>>();



    /// <summary>
    /// 获取障碍物控制器
    /// </summary>
    /// <param name="obstacle">障碍物类型</param>
    /// <returns>障碍物控制器</returns>
    public ObstacleController GetObstacle(Type obstacle)
    {
        if (obstaclePools.ContainsKey(obstacle))
        {
            return obstaclePools[obstacle].Get();
        }
        else
        {
            //MonoPool<ObstacleController> pool = new MonoPool<ObstacleController>();
            //obstaclePools.Add(obstacle, pool);
            return null;
        }
    }


	protected override void OnInit()
	{
		
	}
}