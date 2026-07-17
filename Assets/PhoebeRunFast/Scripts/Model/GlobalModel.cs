using QMVC;

public class GlobalModel : AbstractModel
{
    public BindableProperty<Stage> Stage = new BindableProperty<Stage>(global::Stage.Boot);

    UserJson userJson;

    RoleJson[] roleJsons;
    GoodsJson[] goodsJsons;
    AccountJson[] accountJsons;

    SettingJson settingJson;

    public UserJson UserJson => userJson;
    public RoleJson[] RoleJsons => roleJsons;
    public GoodsJson[] GoodsJsons => goodsJsons;
    public AccountJson[] AccountJsons => accountJsons;

    public void SetRoleJsons(RoleJson[] jsons)
    {
        roleJsons = jsons;
    }

    public void SetGoodsJsons(GoodsJson[] jsons)
    {
        goodsJsons = jsons;
    }

    public void SetAccountJsons(AccountJson[] jsons)
    {
        accountJsons = jsons;
    }

    public void SetUserJson(UserJson json)
    {
        userJson = json;
    }


    public SettingJson SettingJson => settingJson;


    protected override void OnInit()
    {

    }
}