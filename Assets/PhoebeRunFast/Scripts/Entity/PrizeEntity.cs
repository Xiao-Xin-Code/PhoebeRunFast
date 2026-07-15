

using QMVC;

public class PrizeEntity : IEntity
{
    public bool canOperation = false;

    public bool isTriggerFlip = false;

    public bool isFlipped = false;

    public IArchitecture GetArchitecture()
    {
        return PhoebeRunFast.Interface;
    }
}