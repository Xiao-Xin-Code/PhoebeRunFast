using DG.Tweening;
using UnityEngine;

/// <summary>
/// 角色视图
/// </summary>
public class RoleSystemView : MonoBehaviour
{
	[SerializeField] Transform leftPoint;
	[SerializeField] Transform centerPoint;
	[SerializeField] Transform rightPoint;

	/// <summary>
	/// 左侧位置
	/// </summary>
	public Transform Left => leftPoint;
	
	/// <summary>
	/// 中心位置
	/// </summary>
	public Transform Center => centerPoint;
	
	/// <summary>
	/// 右侧位置
	/// </summary>
	public Transform Right => rightPoint;

	[SerializeField] PropertyView propertyView;
	[SerializeField] SwitchView switchView;

	/// <summary>
	/// 切换动画序列
	/// </summary>
	/// <param name="isOpen">是否打开</param>
	/// <returns>动画序列</returns>
	public Sequence SwitchSequence(bool isOpen) => switchView.SwitchSequence(isOpen);

	/// <summary>
	/// 属性动画序列
	/// </summary>
	/// <param name="isOpen">是否打开</param>
	/// <returns>动画序列</returns>
	public Sequence PropertySequence(bool isOpen) => propertyView.PropertySequence(isOpen);

	/// <summary>
	/// 设置属性面板激活状态
	/// </summary>
	/// <param name="isActive">是否激活</param>
	public void SetPropertyActive(bool isActive)
	{
		propertyView.StateInit();
		propertyView.gameObject.SetActive(isActive);
	}

	/// <summary>
	/// 设置切换面板激活状态
	/// </summary>
	/// <param name="isActive">是否激活</param>
	public void SetSwitchActive(bool isActive)
	{
		switchView.StateInit();
		switchView.gameObject.SetActive(isActive);
	}

}
