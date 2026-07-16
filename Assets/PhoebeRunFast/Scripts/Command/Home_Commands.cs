using QMVC;

/// <summary>
/// 小鸟上升命令
/// </summary>
public class BirdUpCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<BirdUpEvent>();
	}
}


public class SignLoginActiveCommand : AbstractCommand
{
	private bool isActive;

	public SignLoginActiveCommand(bool isActive)
	{
		this.isActive = isActive;
	}

    protected override void OnExecute()
    {	
        this.SendEvent(new SignLoginActiveEvent(this.isActive));
    }		
}