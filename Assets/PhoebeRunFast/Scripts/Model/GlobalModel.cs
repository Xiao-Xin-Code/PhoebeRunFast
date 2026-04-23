using QMVC;

public class GlobalModel : AbstractModel
{
    public BindableProperty<Stage> Stage = new BindableProperty<Stage>(global::Stage.Home);

    protected override void OnInit()
    {
        
    }
}