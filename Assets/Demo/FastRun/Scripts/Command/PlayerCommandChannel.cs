using QMVC;

public class PlayerCommandChannel
{

	public class TurnLeftTriggerCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			this.SendEvent<PlayerEventChannel.TurnLeftTriggerEvent>();
		}
	}

	public class TurnRightTriggerCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			this.SendEvent<PlayerEventChannel.TurnRightTriggerEvent>();
		}
	}


}
