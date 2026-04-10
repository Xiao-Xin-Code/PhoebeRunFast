

using System.Collections;
using DG.Tweening;

public class OpenTransitionEvent
{
	public TweenCallback action;

	public OpenTransitionEvent(TweenCallback action)
	{
		this.action = action;
	}
}

public class CloseTransitionEvent
{
	public TweenCallback action;

	public CloseTransitionEvent(TweenCallback action)
	{
		this.action = action;
	}
}

public class UpdateProgressEvent
{
	public float progress;

	public UpdateProgressEvent(float progress)
	{
		this.progress = progress;
	}
}

public class UpdateProgressDeltaEvent
{
	public float delta;

	public UpdateProgressDeltaEvent(float delta)
	{
		this.delta = delta;
	}
}



public class LoadHomeEvent
{
	public IEnumerator enumerator;
}

public class UnLoadHomeEvent
{
	public IEnumerator enumerator;
}


public class LoadMainEvent
{
	public IEnumerator enumerator;
}

public class UnLoadMainEvent
{
	public IEnumerator enumerator;
}


public class LoadGameEvent
{
	public IEnumerator enumerator;
}

public class UnLoadGameEvent
{
	public IEnumerator enumerator;
}