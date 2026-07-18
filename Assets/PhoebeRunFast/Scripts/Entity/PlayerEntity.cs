using QMVC;

public class PlayerEntity : IEntity
{
    public BindableProperty<float> MaxHealth = new BindableProperty<float>();
    public BindableProperty<float> CurMaxHealth = new BindableProperty<float>();
    public BindableProperty<float> CurHealth = new BindableProperty<float>();

    public BindableProperty<float> MaxEnergy = new BindableProperty<float>();
    public BindableProperty<float> CurEnergy = new BindableProperty<float>();

    public BindableProperty<float> CurAttack = new BindableProperty<float>();

    public BindableProperty<float> CurDefense = new BindableProperty<float>();

    public BindableProperty<float> BaseSpeed = new BindableProperty<float>();
    public BindableProperty<float> CurSpeed = new BindableProperty<float>();
    public BindableProperty<float> MaxSpeed = new BindableProperty<float>();

    public BindableProperty<float> CurCooldownReduction = new BindableProperty<float>();
    public BindableProperty<float> CurEnergyRecoveryRate = new BindableProperty<float>();



    public float jumpForce;
    public float gravity;
    public float jumpBufferTime;
    public float groundCheckRadius;








    public IArchitecture GetArchitecture()
    {
        return PhoebeRunFast.Interface;
    }
}