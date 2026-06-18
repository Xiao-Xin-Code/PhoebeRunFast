using UnityEngine;

public abstract class RoadController : BaseController
{
    public float distance = 20;

    protected override void OnInit()
    {
        base.OnInit();
    }


    public abstract void Spawn()
    {
        //生成障碍物
        Debug.Log("生成障碍物");
        Debug.Log("生成收集物");
    }

}