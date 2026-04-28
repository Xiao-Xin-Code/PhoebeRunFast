using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class LevelUpgradeView : MonoBehaviour
{
	[SerializeField] Button upgradeBtn;



	#region Register

	public void RegisterUpGradePressed(UnityAction action)
	{
		upgradeBtn.onClick.AddListener(action);
	}

	#endregion

	#region UnRegister

	public void UnRegisterUpGradePressed(UnityAction action)
	{
		upgradeBtn.onClick.RemoveListener(action);
	}

	#endregion

}


