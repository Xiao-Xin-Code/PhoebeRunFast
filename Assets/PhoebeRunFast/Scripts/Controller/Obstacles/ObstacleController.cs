using UnityEngine;

public abstract class ObstacleController : BaseController
{
    //障碍物类型，更偏向于是那个障碍物，即唯一id 标记，不代表障碍物的分类
    public string obstacleType;

    [SerializeField] private TriggerArea collisionArea;
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