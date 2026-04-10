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

