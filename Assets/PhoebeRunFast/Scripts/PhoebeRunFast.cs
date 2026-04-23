using QMVC;

/// <summary>
/// 项目架构入口类
/// </summary>
public class PhoebeRunFast : Architecture<PhoebeRunFast>
{
	/// <summary>
	/// 初始化架构，注册模型和系统
	/// </summary>
	protected override void Init()
	{
		// 注册模型
		RegisterModel(new GlobalModel());

		// 注册系统
		RegisterSystem(new StageSystem());
		RegisterSystem(new GlobalSystem());
	}
}
