
public abstract class CollectibleController : BaseController
{
    protected override void OnInit()
    {
        base.OnInit();
    }

    public enum CollectibleType
    {
        Resource,//资源
        Prop,//道具
        Effect,//效果
        Special,//特殊
    }

    //收集品类型
    //资源：金币，星声
    //道具：
    //效果：加速，减速
    //特殊：
    public CollectibleType type { get; private set; }
}