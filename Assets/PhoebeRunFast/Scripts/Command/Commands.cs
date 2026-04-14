using System.Collections;
using DG.Tweening;
using QMVC;

public class OpenTransitionCommand : AbstractCommand
{
	private TweenCallback action;

	public OpenTransitionCommand(TweenCallback action)
	{
		this.action = action;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new OpenTransitionEvent(action));
	}
}

public class CloseTransitionCommand : AbstractCommand
{
	private TweenCallback action;

	public CloseTransitionCommand(TweenCallback action)
	{
		this.action = action;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new CloseTransitionEvent(action));
	}
}

public class UpdateProgressCommand : AbstractCommand
{
	private float progress;

	public UpdateProgressCommand(float progress)
	{
		this.progress = progress;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new UpdateProgressEvent(progress));
	}
}

public class UpdateProgressDeltaCommand : AbstractCommand
{
	private float delta;

	public UpdateProgressDeltaCommand(float delta)
	{
		this.delta = delta;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new UpdateProgressDeltaEvent(delta));
	}
}


public class LoadHomeCommand : AbstractCommand<IEnumerator>
{
	protected override IEnumerator OnExecute()
	{
		LoadHomeEvent loadHomeEvent = new LoadHomeEvent();
		this.SendEvent<LoadHomeEvent>();
		yield return loadHomeEvent.enumerator;
		//return loadHomeEvent.enumerator;
	}
}

public class UnLoadHomeCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<UnLoadHomeEvent>();
	}
}

public class LoadMainCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<LoadMainEvent>();
	}
}

public class UnLoadMainCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<UnLoadMainEvent>();
	}
}

public class LoadGameCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<LoadGameEvent>();
	}
}

public class UnLoadGameCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<UnLoadGameEvent>();
	}
}



public class SettingActiveCommand : AbstractCommand
{
	bool isActive;

	public SettingActiveCommand(bool isActive)
	{
		this.isActive = isActive;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new SettingActiveEvent(isActive));
	}
}


public class RoleMenuActiveCommand : AbstractCommand
{
	bool isActive;

	public RoleMenuActiveCommand(bool isActive)
	{
		this.isActive = isActive;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new RoleMenuActiveEvent(isActive));
	}
}

public class BackPackActiveCommand : AbstractCommand
{
	bool isActive;

	public BackPackActiveCommand(bool isActive)
	{
		this.isActive = isActive;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new BackPackActiveEvent(isActive));
	}
}

public class BattleActiveCommand : AbstractCommand
{
	bool isActive;

	public BattleActiveCommand(bool isActive)
	{
		this.isActive = isActive;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new BattleActiveEvent(isActive));
	}
}

public class ShopActiveCommand : AbstractCommand
{
	bool isActive;

	public ShopActiveCommand(bool isActive)
	{
		this.isActive = isActive;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new ShopActiveEvent(isActive));
	}
}

public class LotteryActiveCommand : AbstractCommand
{
	bool isActive;

	public LotteryActiveCommand(bool isActive)
	{
		this.isActive = isActive;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new LotteryActiveEvent(isActive));
	}
}




public class ToLeftRoleCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<ToLeftRoleEvent>();
	}
}

public class ToRightRoleCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<ToRightRoleEvent>();
	}
}