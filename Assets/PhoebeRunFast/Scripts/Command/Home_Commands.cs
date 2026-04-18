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