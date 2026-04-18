using QMVC;

public class PlayerStatusEntity : IEntity
{
    public BindableProperty<float> Health = new BindableProperty<float>();
    public BindableProperty<float> Mana = new BindableProperty<float>();



    public IArchitecture GetArchitecture()
    {
        return PhoebeRunFast.Interface;
    }

}