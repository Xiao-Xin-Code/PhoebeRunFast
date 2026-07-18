using QMVC;

public class PlayerStatusEntity : IEntity
{



    public IArchitecture GetArchitecture()
    {
        return PhoebeRunFast.Interface;
    }

}