using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

public class PropertyController : BaseController
{
	[SerializeField] PropertyView _view;

	[SerializeField] LevelUpgradeController chainLevel;
	[SerializeField] LevelUpgradeController healthLevel;
	[SerializeField] LevelUpgradeController energyLevel;
	[SerializeField] LevelUpgradeController defenseLevel;
	[SerializeField] LevelUpgradeController cooldownReductionLevel;



	protected override void OnInit()
	{
		base.OnInit();
		_view.StateInit();

		chainLevel.RegisterUpgradePressed(() => this.SendCommand(new UpGradeLevelCommand(Consts.Chain)));
		healthLevel.RegisterUpgradePressed(() => this.SendCommand(new UpGradeLevelCommand(Consts.Health)));
		energyLevel.RegisterUpgradePressed(() => this.SendCommand(new UpGradeLevelCommand(Consts.Energy)));
		defenseLevel.RegisterUpgradePressed(() => this.SendCommand(new UpGradeLevelCommand(Consts.Defense)));
		cooldownReductionLevel.RegisterUpgradePressed(() => this.SendCommand(new UpGradeLevelCommand(Consts.CooldownReduction)));

		this.RegisterEvent<ShowLevelEvent>(OnShowLevel);

		gameObject.SetActive(false);
	}


	private void OnShowLevel(ShowLevelEvent evt)
	{
		switch (evt.levelType)
		{
			case Consts.Chain:
				chainLevel.SetLevel(evt.levels[1], evt.levels[2]);
				break;
			case Consts.Health:
				healthLevel.SetLevel(evt.levels[1], evt.levels[2]);
				break;
			case Consts.Energy:
				energyLevel.SetLevel(evt.levels[1], evt.levels[2]);
				break;
			case Consts.Defense:
				defenseLevel.SetLevel(evt.levels[1], evt.levels[2]);
				break;
			case Consts.CooldownReduction:
				cooldownReductionLevel.SetLevel(evt.levels[1], evt.levels[2]);
				break;
		}
	}


	protected override void OnDeInit()
	{
		base.OnDeInit();
		chainLevel.UnRegisterUpGradePressedAllEvent();
		healthLevel.UnRegisterUpGradePressedAllEvent();
		energyLevel.UnRegisterUpGradePressedAllEvent();
		defenseLevel.UnRegisterUpGradePressedAllEvent();
		cooldownReductionLevel.UnRegisterUpGradePressedAllEvent();
		this.UnRegisterEvent<ShowLevelEvent>(OnShowLevel);
	}
}
