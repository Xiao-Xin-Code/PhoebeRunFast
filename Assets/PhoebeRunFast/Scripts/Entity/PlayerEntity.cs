using QMVC;

public class PlayerEntity : IEntity
{
    public BindableProperty<float> Health = new BindableProperty<float>();
    public BindableProperty<float> Mana = new BindableProperty<float>();
    


    public float baseSpeed;
    public float maxSpeed;


    public float jumpForce;
    public float gravity;
    public float jumpBufferTime;
    public float groundCheckRadius;








    public IArchitecture GetArchitecture()
    {
        return PhoebeRunFast.Interface;
    }
}