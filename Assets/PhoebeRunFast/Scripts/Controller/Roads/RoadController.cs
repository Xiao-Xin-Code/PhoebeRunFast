using UnityEngine;

public abstract class RoadController : BaseController
{
    public float distance = 20;
    public string roadType;

    protected override void OnInit()
    {
        base.OnInit();
    }


    public abstract void Spawn();

}