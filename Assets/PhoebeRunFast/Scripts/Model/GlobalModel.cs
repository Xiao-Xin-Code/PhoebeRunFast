using QMVC;

public class GlobalModel : AbstractModel
{
    public BindableProperty<Stage> Stage = new BindableProperty<Stage>(global::Stage.Boot);

    UserJson userJson;

    RoleJson[] roleJsons;
    GoodsJson[] goodsJsons;

    AccountJson accountJson;

    SettingJson settingJson;

    public UserJson UserJson => userJson;
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