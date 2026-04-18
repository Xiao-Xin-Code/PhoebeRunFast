using QMVC;

public class SkillsEntity : IEntity
{
    public float ultimateCooldown = 0;
    public float normalSkillCooldown = 0;

    public float ultimatePrice = 0;
    public float normalSkillPrice = 0;

    public IArchitecture GetArchitecture()
    {
        return PhoebeRunFast.Interface;
    }
}