using QMVC;

public class GlobalSystem : AbstractSystem
{

    GlobalModel _globalModel;

    public GlobalModel GlobalModel => _globalModel;


    #region 伪单例，方便在全局访问其它阶段总控
    BootController bootSingleton;
    HomeController homeSingleton;
    MainController mainSingleton;
    GameController gameSingleton;

    public BootController BootSingleton => bootSingleton;
    public HomeController HomeSingleton => homeSingleton;
    public MainController MainSingleton => mainSingleton;
    public GameController GameSingleton => gameSingleton;

    public void SetBootSingleton(BootController bootSingleton)
    {
        this.bootSingleton = bootSingleton;
    }

    public void SetHomeSingleton(HomeController homeSingleton)
    {
        this.homeSingleton = homeSingleton;
    }

    public void SetMainSingleton(MainController mainSingleton)
    {
        this.mainSingleton = mainSingleton;
    }

    public void SetGameSingleton(GameController gameSingleton)
    {
        this.gameSingleton = gameSingleton;
    }

    #endregion


    protected override void OnInit()
    {
        _globalModel = this.GetModel<GlobalModel>();
    }
}