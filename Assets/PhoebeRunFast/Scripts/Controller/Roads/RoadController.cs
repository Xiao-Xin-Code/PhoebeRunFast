using UnityEngine;

public class RoadController : BaseController
{
    public float distance = 20;

    protected override void OnInit()
    {
        base.OnInit();
    }


    public void Spawn()
    {
        //生成障碍物
        Debug.Log("生成障碍物");
    }

}