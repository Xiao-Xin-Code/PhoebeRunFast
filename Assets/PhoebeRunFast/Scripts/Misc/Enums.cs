/// <summary>
/// 游戏阶段枚举
/// </summary>
public enum Stage
{
	Boot = 0,  // 启动
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
public enum GoodsType
{
	/// <summary>
	/// 资源
	/// </summary>
	Resource,
	/// <summary>
	/// 道具
	/// </summary>
	Prop,
	/// <summary>
	/// 特殊物品
	/// </summary>
	Special
}


public enum Rarity
{
	/// <summary>
	/// 1星
	/// </summary>
	OneStar,
	/// <summary>
	/// 2星
	/// </summary>	
	TwoStar,
	/// <summary>
	/// 3星
	/// </summary>	
	ThreeStar,
	/// <summary>
	/// 4星
	/// </summary>	
	FourStar,
	/// <summary>
	/// 5星
	/// </summary>	
	FiveStar
}





