using QMVC;

public class GlobalModel : AbstractModel
{
    public BindableProperty<Stage> Stage = new BindableProperty<Stage>(global::Stage.Home);

    public BindableProperty<int> OutRoleTableId = new BindableProperty<int>();


    protected override void OnInit()
    {
        
    }
}