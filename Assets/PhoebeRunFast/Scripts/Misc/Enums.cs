/// <summary>
/// 游戏阶段枚举
/// </summary>
public enum Stage
{
	Home = 1,  // 主页
	Main = 2,  // 主界面
	Game = 3   // 游戏
}

/// <summary>
/// 菜单枚举
/// </summary>
public enum Menu
{
	Role = 1,      // 角色
	BackPack = 2,  // 背包
	Battle = 3,    // 战斗
	Shop = 4,      // 商店
	Lottery = 5    // 抽奖
}

/// <summary>
/// 飞行小鸟状态枚举
/// </summary>
public enum FlyBirdState
{
	Ready,  // 准备
	Run,    // 运行
	Over    // 结束
}

/// <summary>
/// 游戏状态枚举
/// </summary>
public enum GameState
{
	Ready,   // 准备
	Running, // 运行中
	Paused,  // 暂停
	Over     // 结束
}


/// <summary>
/// 物品类型枚举
/// </summary>
public enum Goods
{
	Resource,
	Prop,
	QuestItem
}







