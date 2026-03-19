using QMVC;
using UnityEngine;

public abstract class BaseController : MonoBehaviour, IController
{
	protected abstract void Init();

	protected virtual void Start()
	{
		Init();
	}

	public IArchitecture GetArchitecture()
	{
		return FastRun.Interface;
	}
}
