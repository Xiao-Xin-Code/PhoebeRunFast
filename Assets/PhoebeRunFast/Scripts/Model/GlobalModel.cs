using QMVC;

public class GlobalModel : AbstractModel
{
    public BindableProperty<Stage> Stage = new BindableProperty<Stage>(global::Stage.Home);

    public BindableProperty<int> OutRoleTableId = new BindableProperty<int>();


    RoleJson[] roleJsons;
    GoodsJson[] goodsJsons;

    public RoleJson[] RoleJsons => roleJsons;
    public GoodsJson[] GoodsJsons => goodsJsons;

    public void SetRoleJsons(RoleJson[] jsons)
    {
        roleJsons = jsons;
    }

    public void SetGoodsJsons(GoodsJson[] jsons)
    {
        goodsJsons = jsons;
    }



    protected override void OnInit()
    {
        
    }
}