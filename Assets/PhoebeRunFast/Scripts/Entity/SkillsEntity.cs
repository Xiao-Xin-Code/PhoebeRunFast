using QMVC;

public class SkillsEntity : IEntity
{
    public float ultimateCooldown = 0;
    public float skillCooldown = 0;

    public float ultimatePrice = 0;
    public float skillPrice = 0;

    public IArchitecture GetArchitecture()
    {
        return PhoebeRunFast.Interface;
    }
}