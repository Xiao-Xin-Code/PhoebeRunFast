using DG.Tweening;
using UnityEngine;

public class RoleView : MonoBehaviour
{
	[SerializeField] Transform leftPoint;
	[SerializeField] Transform centerPoint;
	[SerializeField] Transform rightPoint;

	public Transform Left => leftPoint;
	public Transform Center => centerPoint;
	public Transform Right => rightPoint;

	[SerializeField] PropertyView propertyView;
	[SerializeField] SwitchView switchView;


	public Sequence SwitchSequence(bool isOpen) => switchView.SwitchSequence(isOpen);

	public Sequence PropertySequence(bool isOpen) => propertyView.PropertySequence(isOpen);


	public void SetPropertyActive(bool isActive)
	{
		propertyView.StateInit();
		propertyView.gameObject.SetActive(isActive);
	}

	public void SetSwitchActive(bool isActive)
	{
		switchView.StateInit();
		switchView.gameObject.SetActive(isActive);
	}

}
