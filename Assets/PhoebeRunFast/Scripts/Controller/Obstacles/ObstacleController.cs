using UnityEngine;

public abstract class ObstacleController : BaseController
{
    public string obstacleType;

    [SerializeField] private CollisionArea collisionArea;
    [SerializeField] private TriggerArea triggerArea;


    protected override void OnInit()
    {
        base.OnInit();

        collisionArea?.RegisterEnterEvent(CollisionEnter);
        collisionArea?.RegisterExitEvent(CollisionExit);
        triggerArea?.RegisterEnterEvent(TriggerEnter);
        triggerArea?.RegisterExitEvent(TriggerExit);
    }


    protected virtual void CollisionEnter()
    {
        //碰撞进入
        Debug.Log($"碰撞进入{obstacleType}");
    }

    protected virtual void CollisionExit()
    {
        //碰撞退出
        Debug.Log($"碰撞退出{obstacleType}");
    }

    protected virtual void TriggerEnter()
    {
        //触发进入
        Debug.Log($"触发进入{obstacleType}");
    }

    protected virtual void TriggerExit()
    {
        //触发退出
        Debug.Log($"触发退出{obstacleType}");
    }

}