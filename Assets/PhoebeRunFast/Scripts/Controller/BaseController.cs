using QMVC;
using UnityEngine;

public abstract class BaseController : MonoBehaviour, IController
{
	public IArchitecture GetArchitecture()
	{
		return PhoebeRunFast.Interface;
	}

	private void Start()
	{
		OnInit();
	}

	private void OnDestroy()
	{
		OnDeInit();
	}

	protected virtual void OnInit() { }

	protected virtual void OnDeInit() { }
}
