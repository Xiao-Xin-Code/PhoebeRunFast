using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

public class PropertyController : BaseController
{
	[SerializeField] PropertyView _view;

	[SerializeField] LevelUpgradeController starLevel;
	[SerializeField] LevelUpgradeController healthLevel;
	[SerializeField] LevelUpgradeController energyLevel;
	[SerializeField] LevelUpgradeController defenseLevel;
	[SerializeField] LevelUpgradeController cooldownReductionLevel;



	protected override void OnInit()
	{
		base.OnInit();
		_view.StateInit();

		starLevel.RegisterUpgradePressed(() => this.SendCommand(new UpGradeLevelCommand(Consts.Star)));
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
			case Consts.Star:
				starLevel.SetLevel(evt.levels[1], evt.levels[2]);
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

}
