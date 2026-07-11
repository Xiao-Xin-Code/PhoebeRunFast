
using UnityEngine;

public class Road_Empty : RoadController
{
    protected override void OnInit()
    {
        base.OnInit();
    }

    public override void Spawn()
    {
        //Debug.Log("生成障碍物");
        //Debug.Log("生成收集物");
        //Debug.Log("但是我是空的，我就不生成了");
        //目前使用直接预制方式生成道路
    }
}
