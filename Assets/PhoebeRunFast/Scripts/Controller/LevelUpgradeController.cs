using QMVC;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelUpgradeController : BaseController
{
	[SerializeField] LevelUpgradeView _view;

    MonoPool<LevelUnitController> _pool;

	public LevelUnitController unit;

	protected override void OnInit()
	{
		base.OnInit();

		_pool = new MonoPool<LevelUnitController>(unit, transform);

	}


	public void SetLevel(int cur,int max)
	{
		_pool.RecycleAll();
		for(int i = 0; i < max; i++)
		{
			LevelUnitController levelUnit = _pool.Get();
			levelUnit.transform.SetSiblingIndex(i);
			levelUnit.IsOn.Value = i < cur;
		}
	}


	public void RegisterUpgradePressed(UnityAction action)
	{
		_view.RegisterUpGradePressed(action);
	}

	public void UnRegisterUpGradePressed(UnityAction action)
	{
		_view.UnRegisterUpGradePressed(action);
	}
}